using UnityEngine;
using System.Collections;

public class BreakableWall : Functional
{
	public int durablility;

	public void TakeDamage(int amount){
		durablility -= amount;

		if (durablility <= 0) {
			GameManager.gameMap.functionalInstances.Remove(this);
			GameManager.gameMap.heightMap.SetLow(transform.position);
			Destroy(gameObject);
		}
	}

	public override bool Blocks (Tile tile)
	{
		return true;
	}

}

