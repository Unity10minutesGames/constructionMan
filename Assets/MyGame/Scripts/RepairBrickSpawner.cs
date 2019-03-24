using UnityEngine;
using UnityEngine.UI;

public class RepairBrickSpawner : MonoBehaviour
{
    private bool testing = false;
    public UndergroundBrick brick;
    public GameObject parentContainer;
    float timer = 0.0f;
    float spawnDelay = 4.0f;
    int maxSpawnableObjects = 5;
    int indexRepairBrick = 0;

    GameObject[] spawnableObjects;

    private void Start()
    {
        spawnableObjects = GetSpawnableObjectPool(maxSpawnableObjects);
    }

    public void DoSetSpawnDelay(float delay)
    {
        spawnDelay = delay;
    }

    public void spawnRepairBrick()
    {
       for (int i = 0; i < spawnableObjects.Length; i++)
        {
            if (!spawnableObjects[i].activeSelf && spawnableObjects[i].GetComponent<UndergroundBrick>().GetBrickState() == UndergroundBrick.BrickState.RepairInactive)
            {
                spawnableObjects[i].gameObject.GetComponent<UndergroundBrick>().SwitchState(UndergroundBrick.BrickState.RepairActive);
                spawnableObjects[i].transform.localPosition = new Vector3(Random.Range(0f, 1180f), 1225f, -0.1f);
                if (testing)
                {
                    spawnableObjects[i].gameObject.transform.localPosition = new Vector3(0.0f, 1225f, -0.1f);
                }
                spawnableObjects[i].gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                spawnableObjects[i].gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                spawnableObjects[i].gameObject.SetActive(true);
                return;
            }
        }
    }

    private GameObject GetRepairBrick()
    {
        GameObject go = brick.GetComponent<UndergroundBrick>().GetUndergroundBrick(parentContainer.transform);
        go.GetComponent<UndergroundBrick>().DoSetupRepairBrick();
        go.GetComponent<UndergroundBrick>().SetName("Brick" + indexRepairBrick);
        go.GetComponent<UndergroundBrick>().DoSetBrickColliders();
        indexRepairBrick++;
        return go;
    }

    private GameObject[] GetSpawnableObjectPool(int maxElements)
    {
        GameObject[] elements = new GameObject[maxElements]; 

        for (int i = 0; i < maxElements; i++)
        {
            elements[i] = GetRepairBrick();
        }

        return elements;
    }
	
	void Update () {
        timer += Time.deltaTime;
        if (timer > spawnDelay)
        {
            spawnRepairBrick();
            timer = 0.0f;
        }
	}
}
