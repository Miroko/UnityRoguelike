using UnityEngine;
using System.Collections;

public class TurnHandler
{
	public bool playerTurn = true;

	private Character lastCharacterMoving;

	public TurnHandler(){
		lastCharacterMoving = GameManager.playerHandler.playerCharacter;
	}

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
			playerTurn = false;
		}
	}

	private void TakeEnemyTurn(){
		foreach(Character character in GameManager.gameMap.characterInstances){
			if(character != GameManager.playerHandler.playerCharacter){
				if(!GameManager.ai.MoveTowardsEntityIfInView(character, GameManager.playerHandler.playerCharacter, 3)){
					GameManager.ai.Wander(character, 0.8f);
				}
				if(character.isMoving){
					lastCharacterMoving = character;
				}
			}
		}
		// Enemies moved, start player turn
		playerTurn = true;
	}

}

