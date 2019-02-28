using UnityEngine;
using TMPro;

public class TimeKeeper : MonoBehaviour {

    private float timer;
    private TextMeshProUGUI timerText;

    void Start ()
    {
        timer = 0;
        timerText = GetComponent<TextMeshProUGUI>();
	}
	
	void Update ()
    {
        timerText.text = UpdateTimer();
	}

    string UpdateTimer()
    {
        timer += Time.deltaTime;

        return FormatTime(timer); ; 
    }

    string FormatTime(float myTime)
    {
        string formatedTime =
            string.Format("{0:00}:{1:00}",
                            Mathf.Floor(myTime / 60),
                            Mathf.Floor(myTime) % 60);

        return formatedTime;
    }

}
