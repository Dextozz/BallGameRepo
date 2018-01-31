using System;
using UnityEngine;

public class StatisticsTimer : MonoBehaviour {

	int totalTime;
	float currentTime;

	// Use this for initialization
	void Start ()
	{
		if (PlayerPrefs.HasKey("PlayingTime"))
			totalTime = PlayerPrefs.GetInt("PlayingTime");
		else
		{
			PlayerPrefs.SetInt("PlayingTime", 0);
			totalTime = 0;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		currentTime += Time.deltaTime;

		//Set total playing time
		PlayerPrefs.SetInt("PlayingTime", totalTime + (int)currentTime);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Finish")
		{
			//Check record time
			if(currentTime < PlayerPrefs.GetFloat("FastestTime"))
			{
				PlayerPrefs.SetFloat("FastestTime", (float)Math.Round(currentTime, 2));
			}
		}
	}
}
