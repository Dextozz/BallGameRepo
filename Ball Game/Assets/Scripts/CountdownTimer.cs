using UnityEngine.UI;
using UnityEngine;
using System;

public class CountdownTimer : MonoBehaviour {

    Text textRef;

    [HideInInspector]
    public static bool start;
    [HideInInspector]
    public static bool resetTime;

    float timeLeft = 10.0f;
    float startingTime;

    void Start()
    {
        textRef = GetComponent<Text>();
        textRef.enabled = false;
        startingTime = timeLeft;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (start && timeLeft > 0)
        {
            textRef.enabled = true;
            Countdown();
        }
        else
        {
            ResetTime();
        }

        if (timeLeft <= 0)
        {
            //Start shooting
            ShootArrows.shoot = true;
        }

        if (resetTime)
        {
            ResetTime();
            resetTime = false;
        }

        if (startingTime - timeLeft > 3)
            TileBoardScript.timeRanOut = true;
        else
            TileBoardScript.timeRanOut = false;
    }

    void Countdown()
    {
        timeLeft -= Time.deltaTime;
        timeLeft = (float)Math.Round(timeLeft, 2);

        string timeLeftString = timeLeft.ToString().Replace('.', ':');
        textRef.text = timeLeftString;
    }

    void ResetTime()
    {
        //Disable the text, set the start to false and reset TimeLeft
        textRef.enabled = false;
        start = false;
        timeLeft = 10.0f;
    }
}
