using UnityEngine;
using System.Collections;

public class RandomHelper
{

	public static Vector2 randomDirection{
		get{
			float direction = UnityEngine.Random.value;
			if(direction < 0.25f)
				return Direction.Up;
			else if(direction < 0.50f)
				return Direction.Down;
			else if(direction < 0.75f)
				return Direction.Left;
			else
				return Direction.Right;
		}
	}

	public static bool Chance(float chance){
		if (UnityEngine.Random.value < chance)
			return true;
		else return false;
	}

}

