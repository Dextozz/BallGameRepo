﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysPuzzle : MonoBehaviour {

	[HideInInspector]
	public static bool resetTile;
	[HideInInspector]
	public static bool detectPress;
	[HideInInspector]
	public static string pressedTile;

	public int length = 7;
	public MeshRenderer[] indicators;
	public GameObject[] tiles;
	public Material[] brightTiles;

	GameObject[] bones;
	Vector3[] firstPos;
	Material[] darkTiles;
	int[] correctCombination;
	List<int> playerCombination;
	int currentIndex;
	int counter;
	//If this is false, reset the circle and empty the list playerComb, if it's true, don't do anything
	bool whenToResetTrap;

	//It might be possible that the script work properly only if i press the button, get off it, and press another one

	// Use this for initialization
	void Awake () {
		whenToResetTrap = true;
		counter = length - 3;
		currentIndex = 0;
		correctCombination = new int[length];
		bones = new GameObject[4];
		firstPos = new Vector3[4];
		darkTiles = new Material[4];
		playerCombination = new List<int>();

		for (int i = 0; i < 4; i++)
		{
			darkTiles[i] = tiles[i].GetComponent<SkinnedMeshRenderer>().material;
		}
	}

	void Start()
	{
		GetTilesBones();
		GetFirstPosOfFields();
		StartCoroutine(ShowCombination());
	}

	// Update is called once per frame
	void Update () {
		WhenToResetTile();
		WhenToAddCombination();

		//If the player made a mistake, reset
		if(!whenToResetTrap)
		{
			//Play "wrong" sound
			playerCombination = new List<int>();
			GenerateCombination();
			whenToResetTrap = true;
		}
	}

	bool Compare()
	{
		if(playerCombination[playerCombination.Count - 1] == correctCombination[playerCombination.Count - 1])
		{
			return true;
		}

		return false;
	}

	//TODO: When the player does 3 correct ones, don't generate new
	void AddCorrect()
	{
		for (int i = 0; i < indicators.Length; i++)
		{
			if(indicators[i].material.color != Color.white)
			{
				indicators[i].material.color = Color.white;
				//Reset the players combinations
				playerCombination = new List<int>();

				return;
			}
		}
	}

	void WhenToAddCorrect()
	{
		//If the list is long enough to compare and if the last ones are the same
		if(playerCombination.Count == counter && Compare())
		{
			AddCorrect();
			counter++;
		}
	}

	void WhenToAddCombination()
	{
		//When a tile is pressed
		if(detectPress)
		{
			for (int i = 0; i < tiles.Length; i++)
			{
				//Determine which tile is pressed
				if(tiles[i].transform.parent.name == pressedTile)
				{
					playerCombination.Add(i);
					detectPress = false;
					pressedTile = null;
					whenToResetTrap = Compare();
				}
			}

			WhenToAddCorrect();
		}
	}

	void WhenToResetTile()
	{
		//When the player leaves the trigger
		if (resetTile)
		{
			//Now that we know that there is a tile that should be reset, we need to find which one it is, we do this
			//by checking the positions of all tiles and finding the one that's not where it should be
			for (int i = 0; i < tiles.Length; i++)
			{
				if (bones[i].transform.position != firstPos[i] && bones[i].tag != "DoNotResetTile")
				{
					ResetTile(tiles[i]);
					resetTile = false;
				}
			}
		}
	}

	void ResetTile(GameObject tile)
	{
		tile.transform.parent.GetComponentInChildren<MoveTile>().playUp();
	}

	//This gets the starting positions of triggers for the buttons
	void GetFirstPosOfFields()
	{
		for (int i = 0; i < tiles.Length; i++)
		{
			firstPos[i] = bones[i].transform.position;
		}
	}

	IEnumerator ShowCombination()
	{
		//For all tiles so far, change material to the bright one
		for (int i = 0; i < currentIndex; i++)
		{
			tiles[correctCombination[i]].GetComponent<SkinnedMeshRenderer>().material = brightTiles[correctCombination[i]];
			yield return new WaitForSeconds(0.8f);
			tiles[correctCombination[i]].GetComponent<SkinnedMeshRenderer>().material = darkTiles[correctCombination[i]];
			yield return new WaitForSeconds(0.2f);
		}
	}

	public void GenerateCombination()
	{
		//Generate new combo at currentIndex
		correctCombination[currentIndex] = Random.Range(0, 4);

		if(currentIndex < length - 1)
			currentIndex++;

		StartCoroutine(ShowCombination());
	}

	public void GenerateNew()
	{
		//-4 because we have 5 buttons to complete, this is for the first one
		for (int i = 0; i < length - 4; i++)
		{
			int rnd = Random.Range(0, 4);

			correctCombination[i] = rnd;
		}

		//We have generated 3 combinations, start next generation at the 4th place
		currentIndex = length - 3;

		StartCoroutine(ShowCombination());
	}

	void GetTilesBones()
	{
		for (int i = 0; i < tiles.Length; i++)
		{
			bones[i] = tiles[i].transform.parent.Find("Armature").transform.Find("Bone").gameObject;
		}
	}
}