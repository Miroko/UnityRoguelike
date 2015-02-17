using UnityEngine;
using System.Collections;

public class Potion : Item
{
	public int healAmount;

	public override void OnPickup (Character picker)
	{
		picker.health += healAmount;
		GameManager.gameMap.itemInstances.Remove (this);
		Destroy (gameObject);
	}

}

