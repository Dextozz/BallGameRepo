using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuBehaviour : MonoBehaviour {

	public Canvas optionsMenu;
    public Canvas levelSelectCanvas;
	public GameObject loadingPanel;

	Button startButton;
    Button levelSelectButton;
	Button optionsButton;
	Button quitButton;

	// Use this for initialization
	void Start ()
	{
		startButton = GameObject.Find("PlayButton").GetComponent<Button>();
        levelSelectButton = GameObject.Find("LevelSelectButton").GetComponent<Button>();
		optionsButton = GameObject.Find("OptionsButton").GetComponent<Button>();
		quitButton = GameObject.Find("QuitButton").GetComponent<Button>();

		startButton.onClick.AddListener(StartGame);
        levelSelectButton.onClick.AddListener(LevelSelect);
		optionsButton.onClick.AddListener(Options);
		quitButton.onClick.AddListener(Quit);
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void StartGame()
	{
		if (PlayerPrefs.HasKey("FurthestLevel"))
			SceneManager.LoadScene(PlayerPrefs.GetInt("FurthestLevel"));
		else
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

		//Show loading screen
		loadingPanel.SetActive(true);
	}

	public void Options()
	{
		optionsMenu.gameObject.SetActive(true);
	}

    public void LevelSelect()
    {
        levelSelectCanvas.gameObject.SetActive(true);
    }
}