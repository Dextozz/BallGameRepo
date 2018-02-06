using System;
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

	[HideInInspector]
	public static GameObject respawnCheckpoint;
	[HideInInspector]
	public static GameObject[] checkpointArray;
	[HideInInspector]
	public static bool resetTile;
	[HideInInspector]
	public static int furthestCheckpoint;

	int upHash = Animator.StringToHash("bones|button up");
	bool isSorted;

	// Use this for initialization
	void Start ()
	{
		if(SceneManager.GetActiveScene().buildIndex == 1)
		{
			animButton = GameObject.Find("dugmefbx").GetComponent<Animator>();
			animTile = GameObject.Find("PressurePlatesTileFBX").GetComponent<Animator>();
		}

		player = GameObject.Find("Player");
		rb = player.GetComponent<Rigidbody>();
		col = player.GetComponent<Collider>();
		playerMesh = player.GetComponent<MeshRenderer>();
		checkpointArray = GameObject.FindGameObjectsWithTag("Checkpoint");
		deathScreenColorImage = GameObject.Find("DeathScreenPanel").GetComponent<Image>();

		//If the player dies before reaching any checkpoints
		respawnCheckpoint = checkpointArray[0];
		//First checkpoint 
		furthestCheckpoint = 0;

		//Sorting the array becase FindGameObjectsWithTag is inconsistent in sorting the objects on its own
		if(!isSorted)
		{
			Array.Sort(checkpointArray, Sort);
			isSorted = true;
		}

	}
	
	public void Respawn()
	{
		//Unlock everything
		player.transform.position = respawnCheckpoint.transform.position + Vector3.up;
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
			//Go through every checkpoint
			for (int i = furthestCheckpoint; i < checkpointArray.Length; i++)
			{
				//Check every checkpoint name to see if it matches the name of the checkpoint we hit
				if (String.Compare(other.gameObject.name, checkpointArray[i].name) == 0)
				{
					//Set that checkpoint as our new checkpoint
					respawnCheckpoint = checkpointArray[i];
					//In case the player falls down and goes through the previous checkpoint, it keeps the furthest one
					furthestCheckpoint = i;
					break;
				}
			}
		}
	}

	//Comparsion method
	int Sort(GameObject x, GameObject y)
	{
		return x.name.CompareTo(y.name);
	}
}