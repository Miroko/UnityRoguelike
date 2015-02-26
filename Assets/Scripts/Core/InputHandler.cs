using UnityEngine;
using System.Collections;

public class InputHandler
{

	public bool MoveToDirection(){	
		float x = Input.GetAxisRaw ("Horizontal");
		float y = Input.GetAxisRaw ("Vertical");

		if (x != 0 || y != 0) {
			if (x != 0) {
				if (x > 0) {
					GameManager.playerHandler.playerCharacter.Move(GameManager.gameMap, Direction.Right);
				} else {
					GameManager.playerHandler.playerCharacter.Move(GameManager.gameMap, Direction.Left);
				}
			} else if (y != 0) {
				if (y > 0) {
					GameManager.playerHandler.playerCharacter.Move(GameManager.gameMap, Direction.Up);
				} else {
					GameManager.playerHandler.playerCharacter.Move(GameManager.gameMap, Direction.Down);
				}
			}		
			return true;
		} 
		return false;
	}

	public bool Skip(){
		if (Input.GetButton ("Jump")) {
			GameManager.playerHandler.playerCharacter.Move(GameManager.gameMap, new Vector2(0,0));
			return true;
		}
		return false;
	}

}

