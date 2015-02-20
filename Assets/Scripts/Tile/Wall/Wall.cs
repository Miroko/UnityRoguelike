using UnityEngine;
using System.Collections;

public class Wall : Tile
{
	public override bool Blocks (Tile tile)
	{
		if (tile is Character) {
			return true;
		}
		return false;
	}

}

