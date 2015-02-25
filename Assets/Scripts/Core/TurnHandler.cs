using UnityEngine;
using System.Collections;

public class TurnHandler
{
	public bool playerTurn = true;

	public Character lastCharacterMoving;

	public void PlayTurn(){
		if (lastCharacterMoving.isMoving == false) {
			if (playerTurn){
				TakePlayerTurn ();
			}
			else {
				TakeEnemyTurn ();
			}
		}
	}

	private void TakePlayerTurn(){
		Vector2 moveDirection = GameManager.inputHandler.GetMoveDirection ();
		if (moveDirection != Vector2.zero) {
			GameManager.playerHandler.playerCharacter.Move(GameManager.gameMap, moveDirection);
			lastCharacterMoving = GameManager.playerHandler.playerCharacter;
			// Player moved, end player turn
			new WaitForSeconds (0.2f);
			playerTurn = false;
		}
	}

	private void TakeEnemyTurn(){
		foreach(Character character in GameManager.gameMap.characterInstances){	
			if(character != GameManager.playerHandler.playerCharacter){
				if(!GameManager.ai.MoveTowardsEntityIfInView(character, GameManager.playerHandler.playerCharacter, 3)){
					if(!RandomHelper.Chance(0.2f)){ // Skip chance
						GameManager.ai.Wander(character, 1f);
					}
				}
				if(character.isMoving){
					lastCharacterMoving = character;
				}
			}
		}
		// Enemies moved, start player turn
		new WaitForSeconds (0.2f);
		playerTurn = true;
	}

}

