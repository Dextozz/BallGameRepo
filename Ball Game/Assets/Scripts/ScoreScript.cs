using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
	[HideInInspector]
	public bool increaseScore;

	[HideInInspector]
	public int coinNumber;

	// Use this for initialization
	void Start ()
	{
		coinNumber = 0;
		GetComponent<Text>().text = coinNumber.ToString();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(increaseScore)
		{
			coinNumber++;
			increaseScore = false;

			GetComponent<Text>().text = coinNumber.ToString();
		}
	}
}

//IncreaseScore is connected to coinBehaviour