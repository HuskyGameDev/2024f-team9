using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 0;
    public bool timeIsRunning = true;
    public TMP_Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        timeIsRunning = true;

        if (timeText == null){
        Debug.LogError("timeText is not assigned in the Inspector!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeIsRunning){
            if (timeRemaining >= 0){
                timeRemaining += Time.deltaTime;
                DisplayTime(timeRemaining);
            }

        }
    }
    void DisplayTime (float timeToDisplay){

        if (timeText == null)
    {
        Debug.LogError("timeText is null in DisplayTime!");
        return;
    }
    
        timeToDisplay +=1;
        float minutes = Mathf.FloorToInt (timeToDisplay / 60);
        float seconds = Mathf.FloorToInt (timeToDisplay % 60);
        timeText.text = string.Format ("{0:00} : {1:00}", minutes, seconds);
    }
}
