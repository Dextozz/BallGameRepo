using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLocked : MonoBehaviour {

    public Sprite unlockedTexture;
    public Sprite lockedTexture;

    Image buttonImage;

    //An int that coresponds to the index of the level this button activates
    public int levelIndex;

    //Furthest level reached, refer to EndLevelBehaviour
    int furthestLevelIndex;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
        furthestLevelIndex = PlayerPrefs.GetInt("FurthestLevel");
        ControlLock();

    }

    void ControlLock()
    {
        if (levelIndex == 1)
            return;

        if (levelIndex <= furthestLevelIndex)
        {
            gameObject.tag = "Unlocked";
            buttonImage.sprite = unlockedTexture;
        }
        else
        {
            gameObject.tag = "Locked";
            buttonImage.sprite = lockedTexture;
        }
    }

}
