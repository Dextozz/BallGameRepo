using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RespawnScript : MonoBehaviour {

	GameObject player;
	Rigidbody rb;
	Collider col;
	MeshRenderer playerMesh;
	Animator animButton;
	Animator animTile;
	Image deathScreenColorImage;
	TextMeshProUGUI checkpointText;

	[HideInInspector]
	public static bool resetTile;
	Vector3 firstRespawn;

    List<GameObject> checkpoints;
	Vector3 respawnLoc;

	int upHash = Animator.StringToHash("bones|button up");
	bool isSorted;
    bool firstCheckpoint;

	// Use this for initialization
	void Start ()
	{
		if(SceneManager.GetActiveScene().buildIndex == 1)
		{
			animButton = GameObject.Find("dugmefbx").GetComponent<Animator>();
			animTile = GameObject.Find("PressurePlatesTileFBX").GetComponent<Animator>();
		}

        firstCheckpoint = true;
        checkpoints = new List<GameObject>();
		player = GameObject.Find("Player");
		rb = player.GetComponent<Rigidbody>();
		col = player.GetComponent<Collider>();
		playerMesh = player.GetComponent<MeshRenderer>();
		deathScreenColorImage = GameObject.Find("DeathScreenPanel").GetComponent<Image>();
		checkpointText = GameObject.Find("CheckpointText").GetComponent<TextMeshProUGUI>();
        checkpointText.enabled = false;

	}
	
	public void Respawn()
	{
		//Unlock everything
		//player.transform.position = respawnCheckpoint.transform.position + Vector3.up;
		player.transform.position = respawnLoc + Vector3.up;
		player.transform.rotation = Quaternion.Euler(0,0,0);
		player.GetComponent<Rigidbody>().velocity = Vector3.zero;
		player.GetComponent<Movement>().isAlive = true;
		player.GetComponent<Rigidbody>().drag = 1;
		rb.useGravity = true;
		col.enabled = true;
		deathScreenColorImage.color = new Color(deathScreenColorImage.color.r, deathScreenColorImage.color.g, deathScreenColorImage.color.b, 0.0f);
		CameraMovement.cameraLocked = false;
		DeathBehaviour.repeat = true;
		DeathBehaviour.hasDiedByTrap = false;

		if (SceneManager.GetActiveScene().buildIndex == 1)
		{
			//Reset arrow trap
			ShootArrows.shoot = false;
			PressurePlate.resetComb = true;
			TileBoardScript.newCombination = true;
			CountdownTimer.start = false;
			animButton.Play(upHash);
			StartArrowsButton.boxCollider.enabled = true;
			//Refer to MoveTile script
			resetTile = true;
		}

		//Trap respawn
		playerMesh.enabled = true;
		DeathBehaviour.hasDied = false;
		Movement.respawn = true;
	}

	void OnTriggerEnter(Collider other)
	{
		//Check if player hit a checkpoint and if he was alive
		if(other.gameObject.tag == "Checkpoint" && player.GetComponent<Movement>().isAlive)
		{
            //Set that as the respawn location
			respawnLoc = other.transform.position;
            //Add it to the list of used checkpoints so we can reset them later
            checkpoints.Add(other.gameObject);
            //Disable that game object so he cant hit it again
			other.gameObject.SetActive(false);

            //Show checkpoint text
            if (!firstCheckpoint)
            {
                StartCoroutine(showCheckpointText());
            }
            else
            {
                firstRespawn = other.transform.position;
            }

            firstCheckpoint = false;
		}
	}

	public void ResetCheckpoints()
	{
        respawnLoc = firstRespawn;

        foreach (GameObject checkpoint in checkpoints)
        {
            checkpoint.SetActive(true);
        }
	}

	IEnumerator showCheckpointText()
	{
		checkpointText.enabled = true;
		yield return new WaitForSeconds(1.5f);
		checkpointText.enabled = false;
	}

	//Comparsion method
	int Sort(GameObject x, GameObject y)
	{
		return x.name.CompareTo(y.name);
	}
}