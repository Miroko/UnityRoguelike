using UnityEngine;
using System.Collections;

public class TurnHandler
{
	public bool playerTurn = true;


	private Character lastEntityMoving;

	public TurnHandler(){
		lastEntityMoving = GameManager.playerHandler.playerEntity;
	}

	public void PlayTurn(){
		if (lastEntityMoving.isMoving == false) {
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
			GameManager.playerHandler.playerEntity.MoveIfNotBlocked(GameManager.gameMap, moveDirection);
			lastEntityMoving = GameManager.playerHandler.playerEntity;
			// Player moved, end player turn
			playerTurn = false;
		}
	}

	private void TakeEnemyTurn(){
		foreach(Character entity in GameManager.gameMap.entityInstances){
			if(entity != GameManager.playerHandler.playerEntity){
				GameManager.ai.Wander(entity, 0.8f);
				if(entity.isMoving){
					lastEntityMoving = entity;
				}
			}
		}
		// Enemies moved, start player turn
		playerTurn = true;
	}

}

