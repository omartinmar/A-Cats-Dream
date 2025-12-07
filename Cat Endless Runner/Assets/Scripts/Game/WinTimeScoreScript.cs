using TMPro;
using UnityEngine;

public class WinTimeScoreScript : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI timeText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int min = Mathf.FloorToInt(Timer.instance.timePassed/60);
        int sec = Mathf.FloorToInt(Timer.instance.timePassed%60);
        //Display on text
        timeText.text = string.Format("{0:00}:{1:00}", min, sec);
    }
}
