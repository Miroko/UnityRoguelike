using UnityEngine;
using System.Collections;

public class Exit : Functional {

	public void GoToNextLevel(){
		GameManager.mapGenerator.currentLevel++;
		GameManager.instance.LoadNextLevel ();
	}

}

