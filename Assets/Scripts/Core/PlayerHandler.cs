using UnityEngine;
using System.Collections;

public class PlayerHandler
{
	public Character playerCharacter;

	public void SetPlayerCharacter(Character playerCharacter){
		this.playerCharacter = playerCharacter;
		GameManager.turnHandler.lastCharacterMoving = playerCharacter;
	}

}

