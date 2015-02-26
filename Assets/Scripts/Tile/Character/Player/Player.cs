using UnityEngine;
using System.Collections;

public class Player : Character {

	public bool isAlive;

	public int initialCollectedFood;
	public int collectedFood;
	public int turnLoseFood;
	public int damageLoseFood;

	void Awake(){
		DontDestroyOnLoad (gameObject);
		collectedFood = initialCollectedFood;
	}

	private void Die(){
		isAlive = false;
		GameManager.instance.uiScript.ShowDeathPanel ();
	}

	private void DecreaseFood(int amount){
		collectedFood -= amount;
		if (collectedFood <= 0) {
			Die ();
		}
	}

	override public void TakeDamage(int amount){
		DecreaseFood(damageLoseFood);
		//base.TakeDamage (amount);
	}

	override public bool Move(GameMap map, Vector3 direction){
		DecreaseFood(turnLoseFood);
		return base.Move (map, direction);	
	}

	public override bool Blocks (Tile tile)
	{
		if (tile is Character) {
			return true;
		}
		return false;
	}

	public override void HandleCollision (Tile tile)
	{
		if (tile is Enemy)
			Attack (tile);
		else if (tile is BreakableWall)
			Attack (tile);
		else if (tile is Item)
			((Item)tile).OnPickup (this);
		else if (tile is Exit) {
			((Exit)tile).GoToNextLevel();
		}

	}
}