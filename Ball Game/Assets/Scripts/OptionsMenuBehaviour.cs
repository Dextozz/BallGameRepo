using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuBehaviour : MonoBehaviour {

	Button onButton;
	Button offButton;
	Button resetProgressButton;
	Button backButton;
	Button LGButton;
	Button MGButton;
	Button HGButton;
	Slider fovSlider;
	TextMeshProUGUI sliderValueText;

	int diedTimes;
	int playingTime;
	float fastestTime;
	string formatedPlayingTime;
	string formatedFastestTime;
	string deadliestTrap;
	int coinsNumber;
	int skinsOwned;

	void Awake()
	{
		onButton = GameObject.Find("OnButtonCamera").GetComponent<Button>();
		offButton = GameObject.Find("OffButtonCamera").GetComponent<Button>();
		resetProgressButton = GameObject.Find("ResetProgressButton").GetComponent<Button>();
		fovSlider = GameObject.Find("Slider").GetComponent<Slider>();
		sliderValueText = GameObject.Find("FovSliderValueText").GetComponent<TextMeshProUGUI>();
		backButton = GameObject.Find("BackButton").GetComponent<Button>();
		LGButton = GameObject.Find("LowButton").GetComponent<Button>();
		MGButton = GameObject.Find("MediumButton").GetComponent<Button>();
		HGButton = GameObject.Find("HighButton").GetComponent<Button>();
	}

	// Use this for initialization
	void Start ()
	{
		onButton.onClick.AddListener(InvertedCamera);
		offButton.onClick.AddListener(NonInvertedCamera);
		resetProgressButton.onClick.AddListener(ResetProgress);
		backButton.onClick.AddListener(BackButton);
		LGButton.onClick.AddListener(SetLow);
		MGButton.onClick.AddListener(SetMed);
		HGButton.onClick.AddListener(SetHigh);

		//Set the min and max value of slider
		fovSlider.minValue = 60.0f;
		fovSlider.maxValue = 90.0f;

		InvertedCameraSection();
		GraphicsSection();

		Statistics();
	}

	void GraphicsSection()
	{
		if (PlayerPrefs.HasKey("Graphics"))
		{
			if (PlayerPrefs.GetInt("Graphics") == 0)
				SetLow();
			else if (PlayerPrefs.GetInt("Graphics") == 2)
				SetMed();
			else
				SetHigh();
		}
		else
			SetHigh();
	}

	void SetLow()
	{
		QualitySettings.SetQualityLevel(0);

		PlayerPrefs.SetInt("Graphics", 0);

		SetBlackColor(LGButton);
		SetWhiteColor(MGButton);
		SetWhiteColor(HGButton);
	}

	void SetMed()
	{
		QualitySettings.SetQualityLevel(2);

		PlayerPrefs.SetInt("Graphics", 2);

		SetWhiteColor(LGButton);
		SetBlackColor(MGButton);
		SetWhiteColor(HGButton);
	}

	void SetHigh()
	{
		QualitySettings.SetQualityLevel(3);

		PlayerPrefs.SetInt("Graphics", 3);

		SetWhiteColor(LGButton);
		SetWhiteColor(MGButton);
		SetBlackColor(HGButton);
	}

	void InvertedCameraSection()
	{
		if (PlayerPrefs.HasKey("BlackButtonName"))
		{
			if (PlayerPrefs.GetString("BlackButtonName") == "onButton")
			{
				//Everything the on button does
				InvertedCamera();
			}
			else
			{
				//Everything the off button does
				NonInvertedCamera();
			}
		}
	}

	void Update()
	{
		//Set the text value
		sliderValueText.SetText(Math.Truncate(fovSlider.value).ToString());
		//Set the camera FOV
		Camera.main.fieldOfView = (float)Math.Truncate(fovSlider.value);
	}

	void InvertedCamera()
	{
		SetBlackColor(onButton);
		SetWhiteColor(offButton);
		PlayerPrefs.SetString("BlackButtonName", "onButton");
	}

	void NonInvertedCamera()
	{
		SetBlackColor(offButton);
		SetWhiteColor(onButton);
		PlayerPrefs.SetString("BlackButtonName", "offButton");
	}

	void SetBlackColor(Button but)
	{
		ColorBlock cb = but.colors;
		cb.normalColor = Color.black;
		but.colors = cb;
	}

	void SetWhiteColor(Button but)
	{
		ColorBlock cb = but.colors;
		cb.normalColor = Color.white;
		but.colors = cb;
	}

	//Button event
	void ResetProgress()
	{
		PlayerPrefs.DeleteAll();
	}

	//Button event
	void BackButton()
	{
		GameObject.Find("OptionsCanvas").SetActive(false);
	}

	void Statistics()
	{
		GetPrefs();
		SetPrefs();
	}

	void SetPrefs()
	{
		GameObject.Find("DiedTimes").GetComponent<TextMeshProUGUI>().SetText(diedTimes.ToString());
		GameObject.Find("PlayingTime").GetComponent<TextMeshProUGUI>().SetText(formatedPlayingTime);
		GameObject.Find("CoinsNumber").GetComponent<TextMeshProUGUI>().SetText(coinsNumber.ToString());
		GameObject.Find("SkinsNumber").GetComponent<TextMeshProUGUI>().SetText(skinsOwned.ToString());
		GameObject.Find("RecordTime").GetComponent<TextMeshProUGUI>().SetText(formatedFastestTime);
		GameObject.Find("TrapName").GetComponent<TextMeshProUGUI>().SetText(deadliestTrap);
	}

	void GetPrefs()
	{
		GetDeath();
		GetPlayingTime();
		GetCoinsNumber();
		GetSkinsOwned();
		GetFastestTime();
		GetDeadliestTrap();
	}

	void GetDeath()
	{
		//Get death
		if (PlayerPrefs.HasKey("DiedTimes"))
			diedTimes = PlayerPrefs.GetInt("DiedTimes");
		else
		{
			PlayerPrefs.SetInt("DiedTimes", 0);
			diedTimes = 0;
		}
	}

	void GetPlayingTime()
	{
		//Get playingTime
		if (PlayerPrefs.HasKey("PlayingTime"))
		{
			playingTime = PlayerPrefs.GetInt("PlayingTime");
			formatedPlayingTime = FormatTime(playingTime);
		}
		else
		{
			PlayerPrefs.SetInt("PlayingTime", 0);
			formatedPlayingTime = "0h0m0sec";
		}
	}

	string FormatTime(int playingTime)
	{
		int h;
		int m;
		int sec;

		h = playingTime / 3600;
		m = (playingTime - h * 3600) / 60;
		sec = (playingTime - h * 3600) % 60;

		string formatedString = h + "h" + m + "m" + sec + "sec";
		return formatedString;
	}

	void GetCoinsNumber()
	{
		//Get coinsNumber
		if (PlayerPrefs.HasKey("CoinsNumber"))
			coinsNumber = PlayerPrefs.GetInt("CoinsNumber");
		else
		{
			PlayerPrefs.SetInt("CoinsNumber", 0);
			coinsNumber = 0;
		}
	}

	void GetSkinsOwned()
	{
		//Get skinsOwned
		if (PlayerPrefs.HasKey("SkinsOwned"))
			skinsOwned = PlayerPrefs.GetInt("SkinsOwned");
		else
		{
			PlayerPrefs.SetInt("SkinsOwned", 0);
			skinsOwned = 0;
		}
	}

	void GetFastestTime()
	{
		//Get fastestTime
		if (PlayerPrefs.HasKey("FastestTime"))
		{
			fastestTime = PlayerPrefs.GetFloat("FastestTime");
			formatedFastestTime = FormatTime((int)fastestTime);
		}
		else
		{
			PlayerPrefs.SetFloat("FastestTime", 0);
			fastestTime = 0;
		}
	}

	void GetDeadliestTrap()
	{
		//Get deadliestTrap
		if (PlayerPrefs.HasKey("DeadliestTrap"))
			deadliestTrap = PlayerPrefs.GetString("DeadliestTrap");
		else
		{
			PlayerPrefs.SetString("DeadliestTrap", "None");
			deadliestTrap = "None";
		}
	}
}