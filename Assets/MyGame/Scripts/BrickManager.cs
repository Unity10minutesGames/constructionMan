using UnityEngine;

public class BrickManager : MonoBehaviour
{
    private bool testing = false;
    private float timer;
    private int maxBrickY = 3;
    private int maxBrickX = 10;
    private float pixelSizeImageX = 144.0f;
    private float pixelSizeImageY = 72.0f;
    private UndergroundBrick undergroundBrick;

    [SerializeField] private float delay = 5.0f;

    public GameObject brick;
    public GameObject parentContainer;
    public GameObject[,] underground;

    int tempindex = -1;
	void Start ()
    {
        timer = 0.0f;
        undergroundBrick = brick.GetComponent<UndergroundBrick>();
        underground = new GameObject[maxBrickX,maxBrickY];

        BuildUnderground(maxBrickX, maxBrickY);
    }

    public void ChangeStateToRandomUndergroundBrick(UndergroundBrick.BrickState brickState)
    {
        float randomX = Random.Range(0,10);
        float randomY = Random.Range(0,3);

        if (brickState == UndergroundBrick.BrickState.Brocken)
        {
            SetBrickBrocken((int)randomX, (int)randomY);
        }

        if (brickState == UndergroundBrick.BrickState.Destroyed)
        {
            if (undergroundBrick.GetBrickState() == UndergroundBrick.BrickState.Brocken)
            {
                undergroundBrick.SwitchState(UndergroundBrick.BrickState.Destroyed);
            }
        }
    } 

    private void BuildUnderground(int nbrRows, int nbrColumns)
    {
        for (int i = 0; i < nbrRows; i++)
        {
            for (int j = 0; j < nbrColumns; j++)
            {
                var go = undergroundBrick.GetUndergroundBrick (parentContainer.transform);
                go.transform.localPosition = new Vector3(i * pixelSizeImageX, j*pixelSizeImageY, -0.1f);
                go.GetComponent<UndergroundBrick>().SetName("wall-r"+i+"-c"+j);
                underground[i,j] = go;
            }
        }
    }

    private void SetBrickBrocken(int row, int column)
    {
        if (underground[row, column].GetComponent<UndergroundBrick>().GetBrickState() == UndergroundBrick.BrickState.Intact)
        {
            underground[row, column].GetComponent<UndergroundBrick>().SwitchState(UndergroundBrick.BrickState.Brocken);
        }
    }


    private void SetBrickDestroyed(int row, int column)
    {
        if (underground[row, column].GetComponent<UndergroundBrick>().GetBrickState() == UndergroundBrick.BrickState.Brocken)
        {
            underground[row, column].GetComponent<UndergroundBrick>().SwitchState(UndergroundBrick.BrickState.Destroyed);
        }
    }

    void Update ()
    {
        timer += Time.deltaTime;
        if (timer > delay)
        {
            tempindex++;
            timer = 0.0f;
            if (!testing)
            {
                ChangeStateToRandomUndergroundBrick(UndergroundBrick.BrickState.Brocken);
            }
            if (testing)
            {
                if (tempindex == 0)
                {
                    SetBrickBrocken(0, 2);
                    SetBrickDestroyed(0, 2);
                }
                else if (tempindex == 1)
                {
                    SetBrickBrocken(0, 1);
                    SetBrickDestroyed(0, 1);
                }
                else if (tempindex == 2)
                {
                    SetBrickBrocken(0, 0);
                    SetBrickDestroyed(0, 0);
                }
            }
           
            
        }
    }
}
