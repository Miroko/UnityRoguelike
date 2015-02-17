using UnityEngine;
using System.Collections;

public abstract class Item : Entity
{

	public abstract void OnPickup (Character picker);

}

