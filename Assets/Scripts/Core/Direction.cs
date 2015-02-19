using UnityEngine;
using System.Collections;

public class Direction
{
	public static Vector2 Up = new Vector2 (0, 1);
	public static Vector2 Down = new Vector2 (0, -1);
	public static Vector2 Left = new Vector2 (-1, 0);
	public static Vector2 Right = new Vector2 (1, 0);

	public static Vector2[] Directions = new Vector2[]{Up, Down, Left, Right};
	
	public static Vector2 Random{
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

	public static Vector2 RandomWeighted(Vector2 direction, float weight){	
		Vector2 returnDirection = direction;
		if (!RandomHelper.Chance (weight)) {	
			while(returnDirection == direction){
				returnDirection = Random;
			}
		}
		return returnDirection;
	}
}

