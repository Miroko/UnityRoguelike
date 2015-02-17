using UnityEngine;
using System.Collections;

public abstract class Entity : Tile
{
	public override bool Blocks (Tile tile)
	{
		if (tile is Entity) {
			return true;
		}
		return false;
	}

}

