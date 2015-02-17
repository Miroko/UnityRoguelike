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
			GameManager.gameMap.MoveEntity (GameManager.playerHandler.playerEntity, moveDirection);
			lastEntityMoving = GameManager.playerHandler.playerEntity;
			// Player moved, end player turn
			playerTurn = false;
		}
	}

	private void TakeEnemyTurn(){
		foreach(Character entity in GameManager.gameMap.entityInstances){
			if(entity != GameManager.playerHandler.playerEntity){
				GameManager.ai.WanderRandomly(entity);
				if(entity.isMoving){
					lastEntityMoving = entity;
				}
			}
		}
		// Enemies moved, start player turn
		playerTurn = true;
	}

}

