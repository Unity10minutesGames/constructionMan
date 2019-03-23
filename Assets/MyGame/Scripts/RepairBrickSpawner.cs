using UnityEngine;
using UnityEngine.UI;

public class RepairBrickSpawner : MonoBehaviour
{
    public UndergroundBrick brick;
    public GameObject parentContainer;
    float timer = 0.0f;
    float delay = 4.0f;
    int maxSpawnableObjects = 5;

    GameObject[] spawnableObjects;

    private void Start()
    {
        spawnableObjects = GetSpawnableObjectPool(maxSpawnableObjects);
    }

    public void spawnRepairBrick()
    {
       for (int i = 0; i < spawnableObjects.Length; i++)
        {
            if (!spawnableObjects[i].activeSelf && spawnableObjects[i].GetComponent<UndergroundBrick>().GetBrickState() == UndergroundBrick.BrickState.Waiting)
            {
                spawnableObjects[i].SetActive(true);
                spawnableObjects[i].transform.localPosition = new Vector3(Random.Range(0f, 13316f), 1225f, -0.1f);
                spawnableObjects[i].transform.localPosition = new Vector3(0.0f, 1225f, -0.1f);
                spawnableObjects[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                spawnableObjects[i].GetComponent<UndergroundBrick>().SetBrickState(UndergroundBrick.BrickState.Repair);
                return;
            }
        }
    }

    private GameObject GetRepairBrick()
    {
        GameObject go = brick.GetComponent<UndergroundBrick>().GetUndergroundBrick(parentContainer.transform);
        go.GetComponent<UndergroundBrick>().SetupRepairBrick();
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
        if (timer > delay)
        {
            spawnRepairBrick();
            timer = 0.0f;
           
        }
	}
}
