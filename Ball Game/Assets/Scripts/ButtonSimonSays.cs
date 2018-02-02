using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSimonSays : MonoBehaviour {

	Animator anim;
	SimonSaysPuzzle simonSays;

	int downHash = Animator.StringToHash("bones|button down");
	int upHash = Animator.StringToHash("bones|button up");
	bool playFirst;

	// Use this for initialization
	void Start () {
		anim = gameObject.transform.parent.parent.GetComponent<Animator>();
		simonSays = GameObject.Find("SimonSaysGround").GetComponent<SimonSaysPuzzle>();
		playFirst = true;
	}

	void OnTriggerEnter(Collider other)
	{
		if (playFirst)
		{
			simonSays.GenerateNew();
			playFirst = false;
		}
		else
		{
			if (SimonSaysPuzzle.buttonGeneratesNew)
			{
				simonSays.GenerateCombination();
				SimonSaysPuzzle.buttonGeneratesNew = false;
			}
			else
			{
				simonSays.ShowCombination();
				//So the player can't just see whats missing
				simonSays.ResetPlayerComb();
			}
		}

		anim.Play(downHash);
	}

	void OnTriggerExit(Collider other)
	{
		anim.Play(upHash);   
	}
}