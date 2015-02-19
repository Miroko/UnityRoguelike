using UnityEngine;
using System.Collections;

public class Ai
{
	public void WanderRandomly(Character entity){
		entity.MoveIfNotBlocked (GameManager.gameMap, Direction.Random);
	}

	public void Wander(Character entity, float directionKeepChance){
		Vector2 direction = Direction.RandomWeighted(entity.currentDirection, directionKeepChance);
		if (!entity.MoveIfNotBlocked (GameManager.gameMap, direction)) {
			entity.MoveIfNotBlocked (GameManager.gameMap, Direction.Random);
		}
	}
}

