using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIScript : MonoBehaviour
{
	public Text foodText;

 	void Awake(){
		DontDestroyOnLoad (gameObject);
	}

	void Update ()
	{
		foodText.text = "Food: " + GameManager.playerHandler.playerCharacter.collectedFood;
	}
}

