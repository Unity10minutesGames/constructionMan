using UnityEngine;
using UnityEngine.UI;

public class UndergroundBrick : MonoBehaviour
{
    public enum BrickState { Intact, Brocken, Destroyed, Repair, Waiting };

    private Image img;
    private bool switchState = false;
    private int row = -1;
    private int column = -1;
    float timerBrocken = 0.0f;

    Vector3 initPosRepairBrick = new Vector3(0.0f, 1225.0f, -0.1f);

    [SerializeField] private BrickState brickState;

    public void SetBrickState (BrickState tmpBrickState)
    {
        if (tmpBrickState == BrickState.Brocken)
        {
            timerBrocken = 0.0f;
        }

        brickState = tmpBrickState;
        switchState = true;
    }

    public void SetArrayPos (int row, int column)
    {
        this.row = row;
        this.column = column;
    }

    public int GetArrayPosRow()
    {
        return row;
    }

    public int GetArrayPosColumn()
    {
        return column;
    }

    public BrickState GetBrickState()
    {
        return brickState;
    }

    public GameObject GetUndergroundBrick (Transform parentTransform)
    {
        brickState = BrickState.Intact;
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
        
        gameObject.GetComponent<Image>().color = Color.black;

    }

    public void ResetRepairBrick()
    {
        SetBrickState(BrickState.Waiting);
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        gameObject.SetActive(false);
        gameObject.transform.localPosition = initPosRepairBrick;
    }

    void Start ()
    {
        brickState = (int)BrickState.Intact;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        

        if (brickState == BrickState.Repair && collision.gameObject.tag == "GameOverFloor")
        {
            brickState = BrickState.Intact;
        }

        if (brickState == BrickState.Repair && 
            collision.gameObject.GetComponent<UndergroundBrick>().brickState == BrickState.Destroyed)
        {
            gameObject.SetActive(false);
            gameObject.GetComponent<UndergroundBrick>().SetBrickState(BrickState.Waiting);
            gameObject.transform.localPosition = new Vector3(5f, 1225f, -0.1f);
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }

    void Update ()
    {
        timerBrocken += Time.deltaTime;

        Debug.Log("Timer destroy 2 :" + timerBrocken);

        if (brickState == BrickState.Brocken && !switchState)
        {
            if(timerBrocken > 5.0f)
            {
                timerBrocken = 0.0f;
                SetBrickState(BrickState.Destroyed);
            }
        }

        if (brickState == BrickState.Brocken && switchState)
        {
            gameObject.GetComponent<Image>().color = Color.black;
            switchState = false;
        }

        if (brickState == BrickState.Repair && switchState)
        {
            gameObject.GetComponent<Image>().color = Color.yellow;
            switchState = false;
        }

        if (brickState == BrickState.Intact && switchState)
        {
            gameObject.GetComponent<Image>().color = Color.white;
            switchState = false;
        }

        if (brickState == BrickState.Waiting && switchState)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            gameObject.GetComponent<Image>().color = Color.gray;
            switchState = false;
        }

        if (brickState == BrickState.Destroyed && switchState)
        {
            //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            gameObject.GetComponent<Image>().color = Color.yellow;
            gameObject.SetActive(false);
            switchState = false;
        }

    }
}
