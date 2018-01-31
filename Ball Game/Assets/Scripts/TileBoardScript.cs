using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoardScript : MonoBehaviour {

    List<GameObject> fieldsList = new List<GameObject>();

    [HideInInspector]
    public static GameObject[,] childrenArray;
    [HideInInspector]
    public static bool newCombination;
    [HideInInspector]
    public static bool buttonWasPressed;
    [HideInInspector]
    public static bool timeRanOut;

    Color currentColor;

    GameObject[,] tileArray;

    void Awake()
    {
        tileArray = new GameObject[PressurePlate.iLength, PressurePlate.jLength];
    }

    // Use this for initialization
    void Start ()
    {
        //Find children
        foreach (Transform child in transform)
            fieldsList.Add(child.gameObject);

        //Create a 2d array from them
        int k = 0;
        for (int i = 0; i < PressurePlate.iLength; i++)
        {
            for (int j = 0; j < PressurePlate.jLength; j++)
            {
                tileArray[i, j] = fieldsList[k++];
            }
        }

        currentColor = tileArray[0, 0].GetComponent<Renderer>().material.color;
	}

    void Update()
    {
        if(newCombination && buttonWasPressed)
        {
            //Set the same tags
            for (int i = 0; i < PressurePlate.iLength; i++)
            {
                for (int j = 0; j < PressurePlate.jLength; j++)
                {
                    tileArray[i, j].tag = childrenArray[i, j].tag;
                }
            }
            //Do it only once
            newCombination = false;
        }

        //Show the color for the first 3 sec (Refer to CountdownTimer.cs
        if (timeRanOut)
            SetColor(currentColor);
        else
            SetColor(Color.red);
    }

    void SetColor(Color fieldColor)
    {

        //Set the color of all safe tiles
        foreach (GameObject tile in tileArray)
        {
            if (tile.tag == "SafeTile")
                tile.GetComponent<Renderer>().material.SetColor("_Color", fieldColor);
            else
                tile.GetComponent<Renderer>().material.SetColor("_Color", currentColor);
        }
    }
}
