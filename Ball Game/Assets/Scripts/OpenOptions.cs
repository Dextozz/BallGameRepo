using UnityEngine;
using UnityEngine.UI;

public class OpenOptions : MonoBehaviour {

	public Canvas OptionsCanvas;
	Button OptionsButton;

	// Use this for initialization
	void Start () {
		OptionsButton = GetComponent<Button>();

		OptionsButton.onClick.AddListener(OpenOptionsMenu);
	}
	
	void OpenOptionsMenu()
	{
		OptionsCanvas.gameObject.SetActive(true);
	}
}