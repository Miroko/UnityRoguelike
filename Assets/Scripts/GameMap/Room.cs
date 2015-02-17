using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour
{
	public int width, height;

	public int enemyChance;

	public GameObject floorTemplate;
	public GameObject wallTemplate;
	public GameObject enemyTemplate;

	public void Generate(Bounds bounds){
		GameManager.gameMap.SpawnFloors (bounds, floorTemplate);
	}

}

