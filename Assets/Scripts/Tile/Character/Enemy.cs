using UnityEngine;
using System.Collections;

public class Enemy : Character
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
		if (tile is Player) {
			attack((Character)tile);
		}

	}

}