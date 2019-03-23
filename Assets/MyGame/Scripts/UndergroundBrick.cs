using UnityEngine;
using UnityEngine.UI;

public class UndergroundBrick : MonoBehaviour
{
    [System.Serializable] public enum BrickState { Intact, Brocken, Destroyed, RepairActive, RepairInactive };

    private const string COLLIDERINNER = "ColliderInner";
    private const string COLLIDEROUTER = "ColliderOuter";
    private const float delayToRepair = 5.0f;
    private Image img;
    private BoxCollider2D colliderOuter = null;
    private BoxCollider2D colliderInner = null;
    private Vector3 initPosRepairBrick = new Vector3 (0.0f, 1225.0f, -0.1f);
    private bool switchState = false;
    private float timerBrocken = 0.0f;
    private float timerRepair = 0.0f;

    [SerializeField] private string brickName = "none";

    //Default state
    [SerializeField] private BrickState brickState = BrickState.Intact;

    public bool restoreRepairBrick = false;
    public bool restoreWallBrick = false;
    public bool restoreOnHold = false;

    void Start ()
    {
        DoInitializeColliders();
    }

    public void SetName (string name)
    {
        brickName = name;
    }

    public string GetName ()
    {
        return brickName;
    }

    public void SwitchState (BrickState tmpBrickState)
    {
        switchState = true;
        DoSetState (tmpBrickState);
        DoSetBrickColor ();
        DoSetBrickColliders ();
        DoResetTimers (tmpBrickState);
        DoSetSpecialBrickProperties (tmpBrickState);
    }

    public BrickState GetBrickState ()
    {
        return brickState;
    }

    public GameObject GetUndergroundBrick (Transform parentTransform)
    {
        var go = Instantiate (gameObject, Vector3.zero, Quaternion.identity, parentTransform);
        return go;
    }

    public void DoSetupRepairBrick ()
    {
        if (gameObject.GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
        }

        DoResetRepairBrick();
    }

    public void DoResetRepairBrick ()
    {
        gameObject.SetActive (false);
        transform.localPosition = initPosRepairBrick;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        SwitchState (BrickState.RepairInactive);
    }

    public void DoSetBrickColliders ()
    {
        if (colliderInner == null || colliderOuter == null)
        {
            return;
        }

        if (brickState == BrickState.RepairActive || brickState == BrickState.RepairInactive)
        {
            colliderInner.enabled = true;
            colliderOuter.enabled = true;
        }
        else if (brickState == BrickState.Intact || brickState == BrickState.Brocken)
        {
            colliderInner.enabled = false;
            colliderOuter.enabled = true;
        }
        else if (brickState == BrickState.Destroyed)
        {
            colliderInner.enabled = true;
            colliderOuter.enabled = false;
        }
    }

    private void DoSetState (BrickState tmpBrickState)
    {
        brickState = tmpBrickState;
    }

    private void DoResetTimers (BrickState tmpBrickState)
    {
        switch (tmpBrickState)
        {
            case BrickState.Brocken:
                timerBrocken = 0.0f;
                break;
            case BrickState.RepairActive:
                timerRepair = 0.0f;
                break;
            case BrickState.Destroyed:
                timerRepair = 0.0f;
                break;
        }
    }

    private void DoInitializeColliders ()
    {
        foreach (var item in GetComponentsInChildren<BoxCollider2D>())
        {
            if (item.name == COLLIDERINNER)
            {
                colliderInner = item;
            }
            else if (item.name == COLLIDEROUTER)
            {
                colliderOuter = item;
            }
        }
    }

    private void OnTriggerExit2D (Collider2D collision)
    {
        gameObject.GetComponent<UndergroundBrick>().restoreOnHold = false;
        collision.GetComponentInParent<UndergroundBrick>().restoreOnHold = false;

        if (restoreWallBrick)
        {
            UndergroundBrick ubrick = collision.gameObject.GetComponentInParent<UndergroundBrick>();
            DoResetRepairBrick ();
            restoreWallBrick = false;
            ubrick.SwitchState (BrickState.Intact);
            ubrick.restoreRepairBrick = false;
        }
    }

    private void OnTriggerStay2D (Collider2D collision)
    {
        if (brickState == BrickState.RepairActive && collision.GetComponentInParent<UndergroundBrick>().GetBrickState () == BrickState.Destroyed)
        {
            gameObject.GetComponent<UndergroundBrick>().restoreOnHold = true;
            collision.GetComponentInParent<UndergroundBrick>().restoreOnHold = true;
        }
    }

    private void DoSetSpecialBrickProperties (BrickState tmpState)
    {
        switch (tmpState)
        {
            case BrickState.RepairInactive:
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                break;
            case BrickState.Intact:
                colliderOuter.enabled = true;
                break;
        }
    }

    private void DoSetBrickColor ()
    {
        if (!switchState)
        {
            return;
        }

        switch (brickState)
        {
            case BrickState.RepairInactive:
                gameObject.GetComponent<Image>().color = Color.gray;
                break;
            case BrickState.RepairActive:
                gameObject.GetComponent<Image>().color = Color.yellow;
                break;
            case BrickState.Intact:
                gameObject.GetComponent<Image>().color = Color.white;
                SetImageSolid ();
                break;
            case BrickState.Brocken:
                gameObject.GetComponent<Image>().color = Color.black;
                break;
            case BrickState.Destroyed:
                SetImageTransparent ();
                break;
        }

        switchState = false;
    }

    private void SetImageTransparent ()
    {
        Color tempColor = GetTemporalColor (0.0f);
        gameObject.GetComponent<Image>().color = tempColor;
    }

    private void SetImageSolid()
    {
        Color tempColor = GetTemporalColor (1.0f);
        gameObject.GetComponent<Image>().color = tempColor;
    }

    private Color GetTemporalColor (float alpha)
    {
        var tempColor = gameObject.GetComponent<Image>().color;
        tempColor.a = alpha;
        return tempColor;
    }

    void Update()
    {
        timerBrocken += Time.deltaTime;
        timerRepair += Time.deltaTime;

        if (restoreOnHold)
        {
            if (timerRepair > delayToRepair)
            {
                restoreRepairBrick = true;
                restoreOnHold = false;
                DoResetTimers (brickState);
            }
        }

        if (brickState == BrickState.Brocken && !switchState)
        {
            if (timerBrocken > delayToRepair)
            {
                DoResetTimers (brickState);
                SwitchState (BrickState.Destroyed);
            }
        }

        if (restoreRepairBrick)
        {
            switch (brickState)
            {
                case BrickState.Destroyed:
                    restoreRepairBrick = false;
                    break;
                case BrickState.RepairActive:
                    restoreRepairBrick = false;
                    restoreWallBrick = true;
                    colliderOuter.enabled = false;
                    break;
            }
        }
    }
}
