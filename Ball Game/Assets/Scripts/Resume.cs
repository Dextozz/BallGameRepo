using UnityEngine;
using UnityEngine.UI;

public class Resume : MonoBehaviour {

	Button resumeButton;
	Canvas pauseMenuCanvas;

	// Use this for initialization
	void Start () {
		resumeButton = GetComponent<Button>();
		pauseMenuCanvas = GameObject.Find("PauseMenuCanvas").GetComponent<Canvas>();

		resumeButton.onClick.AddListener(ResumeGame);
	}
	
	public void ResumeGame()
	{
		//Show cursor
		Cursor.visible = false;
		//Unpause
		Time.timeScale = 1.0f;
		//Turn off canvas
		pauseMenuCanvas.gameObject.SetActive(false);
		CameraMovement.gamePaused = false;
		Manager.gamePaused = false;
	}
}