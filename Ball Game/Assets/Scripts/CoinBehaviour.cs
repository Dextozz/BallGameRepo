using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour {

	int totalCoins;

	AudioSource audioSource;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update ()
	{
		//Rotation
		transform.Rotate(Vector3.forward, 45.0f * Time.deltaTime);
	}

	void OnTriggerEnter(Collider collider)
	{
		//If he was hit by the player and if the player was alive
		if (collider.gameObject.tag == "Player" && GameObject.Find("Player").GetComponent<Movement>().isAlive)
		{
			audioSource.Play();

			//I use a coroutine here because if i deactive the object instantly the sound is will not be played
			StartCoroutine(Deactivate());

			//Increment score
			GameObject.Find("ScoreText").GetComponent<ScoreScript>().increaseScore = true;

			//Increment stats
			if(PlayerPrefs.HasKey("CoinsNumber"))
			{
				totalCoins = PlayerPrefs.GetInt("CoinsNumber");
				PlayerPrefs.SetInt("CoinsNumber", ++totalCoins);
			}
			else
			{
				PlayerPrefs.SetInt("CoinsNumber", 0);   
				totalCoins = 0;
			}
		}
	}

	IEnumerator Deactivate()
	{
		yield return new WaitForSeconds(0.1f);
		gameObject.SetActive(false);
	}
}