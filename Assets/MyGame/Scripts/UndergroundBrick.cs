using UnityEngine;
using UnityEngine.UI;

public class UndergroundBrick : MonoBehaviour
{
    [System.Serializable] public enum BrickState { Intact, Brocken, Destroyed, Repair, Waiting };

    private Image img;
    private BoxCollider2D colliderOuter = null;
    private BoxCollider2D colliderInner = null;
    private Vector3 initPosRepairBrick = new Vector3(0.0f, 1225.0f, -0.1f);
    private bool switchState = false;
    private float timerBrocken = 0.0f, timerRepair = 0.0f;

    [SerializeField] private string bickName = "none";
    [SerializeField] private BrickState brickState = BrickState.Intact;

    public bool restoreRepairBrick = false;
    public bool restoreWallBrick = false;
    public bool restoreOnHold = false;

    public void SetName(string name)
    {
        bickName = name;
    }

    public string GetName()
    {
        return bickName;
    }

    public void SetBrickState (BrickState tmpBrickState)
    {
        brickState = tmpBrickState;
        switchState = true;

        SetCollidersEnabled();
        ResetTimers(brickState);
    }

    private void ResetTimers(BrickState tmpBrickState)
    {
        if (tmpBrickState == BrickState.Brocken)
        {
            timerBrocken = 0.0f;
        }
        else if (tmpBrickState == BrickState.Repair)
        {
            timerRepair = 0.0f;
        }
        else if (tmpBrickState == BrickState.Destroyed)
        {
            timerRepair = 0.0f;
        }
    }

    public BrickState GetBrickState()
    {
        return brickState;
    }

    public GameObject GetUndergroundBrick (Transform parentTransform)
    {
        var go = Instantiate (gameObject, Vector3.zero, Quaternion.identity, parentTransform);
        return go;
    }

    public void SetupRepairBrick()
    {
        if (gameObject.GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
        }

        ResetRepairBrick();
    }

    public void ResetRepairBrick()
    {
        gameObject.SetActive(false);
        transform.localPosition = initPosRepairBrick;
        SetBrickState(BrickState.Waiting);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Debug.Log("---------------------------------------------------------- reset Repair Brick");
    }

    public void ResetIntactBrick()
    {
        Debug.Log("name "+GetName());
        SetBrickState(BrickState.Intact);
        SetImageSolid();
        Debug.Log("---------------------------------------------------------- reset Intact Brick");
    }

    void Start ()
    {
        foreach (var item in GetComponentsInChildren<BoxCollider2D>())
        {
            if (item.name == "ColliderInner")
            {
                colliderInner = item;
            }
            else if (item.name == "ColliderOuter")
            {
                colliderOuter = item;
            }
        }
    }

    private void SetBrickColliders()
    {
        if (brickState == BrickState.Repair || brickState == BrickState.Waiting)
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

    public void SetCollidersEnabled()
    {
        if (colliderInner == null || colliderOuter == null)
        {
            Debug.Log("colliders null");
            return;
        }

        SetBrickColliders();
        Debug.Log("colliders enabled");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("On Trigger EXIT: " + collision.gameObject.GetComponentInParent<UndergroundBrick>().GetName() + "go: " + bickName);
        Debug.Log("wall brick restore: " + restoreWallBrick);
        gameObject.GetComponent<UndergroundBrick>().restoreOnHold = false;
        collision.GetComponentInParent<UndergroundBrick>().restoreOnHold = false;

        if (restoreWallBrick)
        {
            UndergroundBrick ubrick = collision.gameObject.GetComponentInParent<UndergroundBrick>();
            Debug.Log("in 1 ---------------------------------------------!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            ResetRepairBrick();
            restoreWallBrick = false;
            Debug.Log("in 1.5 ---------------------------------------------!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Debug.Log(ubrick.GetName());
            ubrick.ResetIntactBrick();
            ubrick.restoreRepairBrick = false;
            
            Debug.Log("in 2 ---------------------------------------------!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }
       

        //ResetTimers(gameObject.GetComponent<UndergroundBrick>().GetBrickState());
        //ResetTimers(collision.GetComponentInParent<UndergroundBrick>().GetBrickState());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (brickState == BrickState.Repair && collision.GetComponentInParent<UndergroundBrick>().GetBrickState() == BrickState.Destroyed)
        {
            gameObject.GetComponent<UndergroundBrick>().restoreOnHold = true;
            collision.GetComponentInParent<UndergroundBrick>().restoreOnHold = true;
        }
    }

    void Update ()
    {
        timerBrocken += Time.deltaTime;
        timerRepair += Time.deltaTime;

        if (restoreOnHold)
        {
            Debug.Log("Restore on hold in update: " + restoreOnHold + " go name " + this.bickName + " timer " + timerRepair);
            if (timerRepair > 5.0f)
            {
                restoreRepairBrick = true;
                restoreOnHold = false;
                ResetTimers(brickState);
            }
        }

        SetBrickColor();

        if (brickState == BrickState.Brocken && !switchState)
        {
            if (timerBrocken > 5.0f)
            {
                ResetTimers(brickState);
                SetBrickState(BrickState.Destroyed);
            }
        }

        if (brickState == BrickState.Destroyed && restoreRepairBrick)
        {
            restoreRepairBrick = false;
        }

        if (brickState == BrickState.Repair && restoreRepairBrick)
        {
            restoreWallBrick = true;
            restoreRepairBrick = false;

            colliderOuter.enabled = false;
        }

    }

    private void SetBrickColor()
    {
        if (brickState == BrickState.Waiting && switchState)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            gameObject.GetComponent<Image>().color = Color.gray;
            switchState = false;

        }
        else if (brickState == BrickState.Repair && switchState)
        {
            gameObject.GetComponent<Image>().color = Color.yellow;
            switchState = false;
        }
        else if (brickState == BrickState.Intact && switchState)
        {
            gameObject.GetComponent<Image>().color = Color.white;
            colliderOuter.enabled = true;

            switchState = false;
        }
        else if (brickState == BrickState.Brocken && switchState)
        {
            gameObject.GetComponent<Image>().color = Color.black;
            switchState = false;
        }
        else if (brickState == BrickState.Destroyed && switchState)
        {
            SetImageTransparent();
            switchState = false;
        }
    }

    private void SetImageTransparent()
    {
        var tempColor = gameObject.GetComponent<Image>().color;
        tempColor.a = 0.0f;
        gameObject.GetComponent<Image>().color = tempColor;
    }

    private void SetImageSolid()
    {
        var tempColor = gameObject.GetComponent<Image>().color;
        tempColor.a = 1.0f;
        gameObject.GetComponent<Image>().color = tempColor;
    }
}
