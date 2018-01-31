using UnityEngine;
using UnityEngine.UI;

public class RestartLevel : MonoBehaviour {

    Button resetButton;
    AudioSource backgroundMusic;
    GameObject scoreText;

    int coinNumber;
    int coinsRead;

    // Use this for initialization
    void Start ()
    {
        resetButton = GetComponent<Button>();
        backgroundMusic = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        scoreText = GameObject.Find("ScoreText");
        coinNumber = scoreText.GetComponent<ScoreScript>().coinNumber;

        resetButton.onClick.AddListener(ResetLevel);
    }
    
    void ResetLevel()
    {
        GameObject.Find("Player").GetComponent<Manager>().ResetLevel();
        Cursor.visible = false;
        GameObject.Find("PauseMenuCanvas").SetActive(false);
        Time.timeScale = 1.0f;
        CameraMovement.gamePaused = false;
        Manager.gamePaused = false;
        UpdateScore();

        backgroundMusic.time = 0.0f;
        backgroundMusic.Play();
    }

    void UpdateScore()
    {
        scoreText.GetComponent<ScoreScript>().coinNumber = 0;
        scoreText.GetComponent<Text>().text = scoreText.GetComponent<ScoreScript>().coinNumber.ToString();

        if (PlayerPrefs.HasKey("CoinsNumber"))
        {
            coinsRead = PlayerPrefs.GetInt("CoinsNumber");
            PlayerPrefs.SetInt("CoinsNumber", coinsRead - coinNumber);
        }
    }
}
