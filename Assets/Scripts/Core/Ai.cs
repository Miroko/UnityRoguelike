using UnityEngine;
using System.Collections;

public class Ai
{

	public void WanderRandomly(Character entity){
		float direction = UnityEngine.Random.value;
		if(direction < 0.25f)
			GameManager.gameMap.MoveEntity(entity, new Vector3(-1,0));
		else if(direction < 0.50f)
			GameManager.gameMap.MoveEntity(entity, new Vector3(1,0));
		else if(direction < 0.75f)
			GameManager.gameMap.MoveEntity(entity, new Vector3(0,1));
		else if(direction <= 1.00f)
			GameManager.gameMap.MoveEntity(entity, new Vector3(0,-1));			
	}


}

