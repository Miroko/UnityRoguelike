using UnityEngine;
using System.Collections;

public class Player : Character {

	public int collectedFood;
	public int turnLoseFood;
	public int damageLoseFood;

	void Awake(){
		DontDestroyOnLoad (gameObject);
	}

	private void DecreaseFood(int amount){
		collectedFood -= amount;
		if (collectedFood <= 0) {
			Debug.Log("Died of hunger");
		}
	}

	override public void TakeDamage(int amount){
		DecreaseFood(damageLoseFood);
		base.TakeDamage (amount);
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
			GameManager.NextLevel();
		}

	}
}