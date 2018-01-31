using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {

    List<GameObject> getChildrenList = new List<GameObject>();
    GameObject[,] childrenArray;

    public static int iLength;
    public static int jLength;

    //Look at StartArrowsButton
    [HideInInspector]
    public static bool newCombination;
    [HideInInspector]
    public static bool resetComb;

    public int setILength;
    public int setJLength;

    string safeTile = "SafeTile";

    enum NextMove
    {
        forward,
        left,
        right
    };

    void Awake()
    {
        iLength = setILength;
        jLength = setJLength;

        childrenArray = new GameObject[iLength, jLength];
    }

    // Use this for initialization
    void Start ()
    {
        //Adds all the children to getChildrenList
        foreach (Transform child in transform)
            getChildrenList.Add(child.gameObject);

        //Temp variable that goes through all the children in getChildrenList
        int k = 0;
        for (int i = 0; i < iLength; i++)
        {
            for (int j = 0; j < jLength; j++)
            {
                childrenArray[i,j] = getChildrenList[k++];  
            }
        }
    }

    void Update()
    {
        if (newCombination)
        {
            //Return all false combination
            foreach (GameObject child in childrenArray)
            {
                child.tag = "Untagged";
            }

            SetFields(ref childrenArray);

            //This is for the big board that shows the path
            TileBoardScript.childrenArray = this.childrenArray;

            //Do it only once
            newCombination = false;
        }

        //Reset combination if the player died
        if(resetComb)
        {
            //Return all false combination
            foreach (GameObject child in childrenArray)
            {
                child.tag = "Untagged";
            }

            resetComb = false;
        }
    }

    //Moves to a location depending on Where
    void Move(int enumValue, GameObject[,] childrenArray, ref int i, ref int j)
    {
        if(enumValue == (int)NextMove.forward)
        {
            childrenArray[++i, j].tag = safeTile;
        }
        else if(enumValue == (int)NextMove.left)
        {
            childrenArray[i, --j].tag = safeTile;
        }
        else if(enumValue == (int)NextMove.right)
        {
            childrenArray[i, ++j].tag = safeTile;
        }
    }

    //Decides where the tag will be moved ("set")
    int Where(int i,int j, GameObject[,] childrenArray)
    {
        int rand = Random.Range(1, 3);

        if (i % 2 != 0)
        {
            if (j == jLength - 1)
            {
                if (childrenArray[i, j - 1].tag == safeTile)
                {
                    return (int)NextMove.forward;
                }
                else
                {
                    return (int)NextMove.left;
                }
            }
            else if (j == 0)
            {
                if (childrenArray[i, j + 1].tag == safeTile)
                {
                    return (int)NextMove.forward;
                }
                else
                {
                    return (int)NextMove.right;
                }
            }
            else
            {
                if (rand == (int)NextMove.left && childrenArray[i, j - 1].tag != safeTile)
                    return (int)NextMove.left;
                else if (rand == (int)NextMove.right && childrenArray[i, j + 1].tag != safeTile)
                    return (int)NextMove.right;
                else
                    return (int)NextMove.forward;
            }
        }

        return (int)NextMove.forward;
    }

    void SetFields(ref GameObject[,] childrenArray)
    {
        //Select one field to be random
        int rand = Random.Range(0, jLength);
        childrenArray[0, rand].tag = safeTile;

        int i = 0;
        int j = rand;
        Move((int)NextMove.forward, childrenArray, ref i, ref j);
        while(i != iLength - 1)
        {
            int enumValue = Where(i, j, childrenArray);
            Move(enumValue, childrenArray, ref i, ref j);
        }
    }
}