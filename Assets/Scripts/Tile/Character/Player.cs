using UnityEngine;
using System.Collections;

public class Player : Character
{
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
			attack ((Character)tile);
		else if (tile is Item)
			((Item)tile).OnPickup (this);
		else if (tile is Exit) {
			GameManager.NextLevel();
		}

	}
}

