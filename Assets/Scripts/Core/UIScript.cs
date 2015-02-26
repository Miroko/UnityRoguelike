using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIScript : MonoBehaviour
{
	private GameObject currentPanel;

	public Text foodText;
	public Text infoText;
	public Text deathText;
	public GameObject infoPanel;
	public GameObject deathPanel;
	public GameObject gamePanel;

	public InputField mapSeedInputField;

 	void Awake(){
		DontDestroyOnLoad (gameObject);
		ShowInfoPanel ();
	}

	void Update ()
	{
		if (currentPanel == gamePanel) {
			foodText.text = "Food:" + GameManager.playerHandler.playerCharacter.collectedFood;
		} else if (currentPanel == infoPanel) {

		} else if (currentPanel == deathPanel) {
			deathText.text = "You ran out of food on level:" + GameManager.mapGenerator.currentLevel;
		}
	}

	public void ShowGamePanel(){
		gamePanel.SetActive(true);
		infoPanel.SetActive(false);
		deathPanel.SetActive(false);
		currentPanel = gamePanel;
	}

	public void ShowInfoPanel(){
		gamePanel.SetActive(false);
		infoPanel.SetActive(true);
		deathPanel.SetActive(false);
		currentPanel = infoPanel;
	}

	public void ShowDeathPanel(){
		gamePanel.SetActive(false);
		infoPanel.SetActive(false);
		deathPanel.SetActive(true);
		currentPanel = deathPanel;
	}

	public void SetMapSeed(){
		int seed = int.Parse (mapSeedInputField.text);
		GameManager.mapGenerator.seed = seed;
	}

	public void NewGame(){
		ShowGamePanel ();
		GameManager.instance.NewGame ();
	}

}

