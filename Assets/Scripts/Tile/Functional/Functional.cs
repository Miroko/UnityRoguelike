using UnityEngine;
using System.Collections;

public abstract class Functional : Tile
{
	public override bool Blocks (Tile tile)
	{
		return false;
	}

}

