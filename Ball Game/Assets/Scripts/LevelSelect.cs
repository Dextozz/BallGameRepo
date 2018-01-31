using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

	public void Level(int levelIndex)
	{
		if (levelIndex > PlayerPrefs.GetInt("FurthestLevel"))
		{
			//Placeholder
			Debug.Log("Level locked!");

			return;
		}

		SceneManager.LoadScene(levelIndex);
	}
}