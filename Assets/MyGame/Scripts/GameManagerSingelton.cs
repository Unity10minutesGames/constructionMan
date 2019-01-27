using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerSingelton : MonoBehaviour {
    public static GameManagerSingelton instance = null;

    public TextMeshProUGUI levelText;
    public bool displayNextLevel = false;
    public int currentLevel = 0;

    private void Awake()
    {
        if (instance == null)
        {
            Debug.Log("Create Instance");
            instance = this;
        }
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        //displayNextLevel = true;
        levelText.enabled = false;
	}

    public void SwitchLevel()
    {
        displayNextLevel = false;
        IncreaseLevel();
        DisplayNextLevel(currentLevel);
        StartCoroutine(WaitDisableLevelInfo());
    }

    public void PauseGame()
    {
        //Disable spawner blocks
        //Disable spawner stones
        Debug.Log("PauseGame");
    }


    public void IncreaseLevel()
    {
        currentLevel++;
    }

    public void DisplayNextLevel(int level)
    {
        PauseGame();
        levelText.enabled = true;
        levelText.SetText("Level "+level.ToString());
    }

    IEnumerator WaitDisableLevelInfo()
    {
        yield return new WaitForSeconds(3);
        levelText.enabled = false;
    }

    // Update is called once per frame
    void Update () {
        if (displayNextLevel)
        {
            SwitchLevel();
        }
	}
}
