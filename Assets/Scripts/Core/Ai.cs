using UnityEngine;
using System.Collections;

public class Ai
{
	public void WanderRandomly(Character entity){
		entity.Move (GameManager.gameMap, Direction.Random);
	}

	public void Wander(Character entity, float directionKeepChance){
		Vector2 direction = Direction.RandomWeighted(entity.currentDirection, directionKeepChance);
		if (!entity.Move (GameManager.gameMap, direction)) {
			entity.Move (GameManager.gameMap, Direction.Random);
		}
	}

	public bool MoveTowardsEntityIfInView(Character moving, Character towards, int viewDistance){	
		foreach (Vector3 direction in Direction.Directions) {
			foreach(Vector3 position in GameManager.gameMap.heightMap.LineIterator(moving.transform.position, direction, viewDistance)){																								
				//Debug.DrawLine(position, new Vector2(position.x, position.y + 0.2f), Color.green, float.MaxValue);
				if(towards.transform.position == position){
					moving.Move(GameManager.gameMap, direction);
					//Debug.DrawLine(position, new Vector2(position.x, position.y + 0.2f), Color.red, float.MaxValue);
					return true;
				}
			}	
		}
		return false;
	}

}

