using UnityEngine;
using System.Collections;

public class Food : Item
{
	public int foodAmount;

	public override void OnPickup (Player picker)
	{
		picker.collectedFood += foodAmount;

		GameManager.gameMap.itemInstances.Remove (this);
		Destroy (gameObject);
	}

}

