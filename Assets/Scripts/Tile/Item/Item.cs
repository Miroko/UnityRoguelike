using UnityEngine;
using System.Collections;

public abstract class Item : Tile
{

	public abstract void OnPickup (Character picker);

	public override bool Blocks (Tile tile)
	{
		return false;
	}



}

