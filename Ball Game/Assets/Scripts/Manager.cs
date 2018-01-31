using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	List<GameObject> coins;
	GameObject coinsParent;
	AudioSource backgroundMusic;

	public Canvas pauseMenuCanvas;
	public Canvas deathScreenCanvas;

	public static bool gamePaused;

	void Start ()
	{
		coins = new List<GameObject>();
		coinsParent = GameObject.Find("Coins");
		backgroundMusic = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if (!gamePaused)
				PauseGame();
			else
				UnpauseGame();
		}
	}

	void PauseGame()
	{
		Time.timeScale = 0.0f;
		Cursor.visible = true;
		CameraMovement.gamePaused = true;
		pauseMenuCanvas.gameObject.SetActive(true);
		gamePaused = true;
		backgroundMusic.Pause();
	}

	void UnpauseGame()
	{
		Cursor.visible = false;
		Time.timeScale = 1.0f;
		pauseMenuCanvas.gameObject.SetActive(false);
		CameraMovement.gamePaused = false;
		gamePaused = false;
		backgroundMusic.Play();
	}

	public void ResetLevel()
	{
		ResetCoins();
		ResetCheckpoins();
		gameObject.GetComponent<RespawnScript>().Respawn();
	}

	void ResetCheckpoins()
	{
		RespawnScript.furthestCheckpoint = 0;
		RespawnScript.respawnCheckpoint = RespawnScript.checkpointArray[0];
	}

	void ResetCoins()
	{
		GetCoins();
		RespawnCoins();
	}

	void GetCoins()
	{
		foreach (Transform coin in coinsParent.transform)
		{
			coins.Add(coin.gameObject);
		}
	}

	void RespawnCoins()
	{
		foreach (GameObject coin in coins)
		{
			if (!coin.activeSelf)
				coin.SetActive(true);
		}
	}
}