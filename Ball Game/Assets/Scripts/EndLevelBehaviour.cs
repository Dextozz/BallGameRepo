using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class EndLevelBehaviour : MonoBehaviour {

    public GameObject levelEndCanvas;
    TextMeshProUGUI currentGoldNumb;
    TextMeshProUGUI totalGoldNumb;

    GameObject player;
    Button nextLvlButton;
    Button quitButton;

    float currentGold;
    float totalGold;
    float speed = 15f;
    bool startCount;

    void Awake()
    {
        player = GameObject.Find("Player");
        GetReferences();
    }

    void GetReferences()
    {
        try
        {
            nextLvlButton = GameObject.Find("NextLevelButton").GetComponent<Button>();
            quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
            currentGoldNumb = GameObject.Find("GoldEarned").GetComponent<TextMeshProUGUI>();
            totalGoldNumb = GameObject.Find("TotalGold").GetComponent<TextMeshProUGUI>();

            nextLvlButton.onClick.AddListener(NextLevel);
            quitButton.onClick.AddListener(Quit);
        }
        catch
        {
            Debug.Log("Turn on levelEndPanel");
        }
    }

    void Start()
    {
        levelEndCanvas.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        //If the player hits endLevel collider and if the player is alive
        if (other.tag == "Player" && GameObject.Find("Player").GetComponent<Movement>().isAlive)
        {
            //In case the game chrashes or the player quits, sets the "continue" from next level
            PlayerPrefs.SetInt("FurthestLevel", SceneManager.GetActiveScene().buildIndex + 1);

            //Restrict movement so the player doesn't die by accident
            player.GetComponent<Movement>().isAlive = false;

            //Show endscreen
            levelEndCanvas.SetActive(true);

            //Slow down the body after endscreen so the player doesn't die by accident
            player.GetComponent<Rigidbody>().drag = 3;

            //Show cursor and lock the camera
            Cursor.visible = true;
            GameObject.Find("Main Camera").GetComponent<CameraMovement>().cameraLocked = true;

            //Get currentGold
            currentGold = GameObject.Find("ScoreText").GetComponent<ScoreScript>().coinNumber;

            //Get totalGold from playerPrefs
            if (PlayerPrefs.HasKey("TotalGold"))
            {
                totalGold = PlayerPrefs.GetFloat("TotalGold");
                //Set the totalGold in PlayerPrefs now incase the player clics next level before the counting is over
                PlayerPrefs.SetFloat("TotalGold", totalGold + currentGold);
            }
            else
            {
                totalGold = 0.0f;
                PlayerPrefs.SetFloat("TotalGold", totalGold);
            }

            //Do the update only when needed
            startCount = true;
        }
    }

    void Update()
    {
        //Lower currentGold and increase totalGold at the same time if the player hits endScreen collider(startCount == true)
        if(startCount)
        {
            //Do this as long as currentGold is bigger than 0
            if (currentGold > 0)
            {
                currentGold -= speed * Time.deltaTime;
                totalGold += speed * Time.deltaTime;

                //Write out to the screen
                currentGoldNumb.SetText(Mathf.Round(currentGold).ToString());
                totalGoldNumb.SetText(Mathf.Round(totalGold).ToString());
            }
            //Once it reaches zero (or falls below it) do this
            else if (currentGold <= 0)
            {
                //Set total gold in PlayerPrefs
                PlayerPrefs.SetFloat("TotalGold", totalGold);

                //Set the startCount to false so it only does it when player earned some gold
                startCount = false;
            }
        }
    }

    //Load next level
    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            GameObject.Find("CommingSoonText").GetComponent<Text>().text = "Comming Soon!";
    }

    //Quit
    public void Quit()
    {
        Application.Quit();
    }
}