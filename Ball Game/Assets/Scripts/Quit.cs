using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour
{
	Button QuitButton;

	void Start()
	{
		QuitButton = GetComponent<Button>();
		QuitButton.onClick.AddListener(QuitGame);
	}

	void QuitGame()
	{
		SceneManager.LoadScene(0);
	}
	
}