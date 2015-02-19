using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class MapGenerator
{
	private Vector2 cursor;
	private Vector2 cursorDirection;

	public GameObject playerTemplate;
	public GameObject[] enemyTemplates;
	public GameObject[] itemTemplates;
	public GameObject[] floorTemplates;
	public GameObject[] wallTemplates;
	public GameObject[] roomTemplates;

	public MapGenerator(GameObject playerTemplate, GameObject[] floorTemplates, GameObject[] wallTemplates,
	                    GameObject[] enemyTemplates, GameObject[] itemTemplates){
		this.playerTemplate = playerTemplate;
		this.enemyTemplates = enemyTemplates;
		this.floorTemplates = floorTemplates;
		this.wallTemplates = wallTemplates;
		this.itemTemplates = itemTemplates;
	}

	public GameMap NewGameMap(int width, int height, int seed, int rooms, int corridors, int roomSize, int corridorLenght,
	                          float enemyChance, float itemChance){

		GameMap map = new GameMap(width, height);
		Vector2 center = new Vector2 (width/2, height/2);
		UnityEngine.Random.seed = seed;

		cursor = center;

		SpawnPlayer (map, cursor);

		int roomsToPlace = rooms; 
		int corridorsToPlace = corridors; 
		float total = roomsToPlace + corridorsToPlace;
		for (int loop = 0; loop < total; loop++) {		
			float roomPlaceChance = roomsToPlace / total;
			float corridorPlaceChance = corridorsToPlace / total;

			//Debug.Log("corridorChance " + corridorPlaceChance );
			//Debug.Log("roomChance " + roomPlaceChance );

			float random = UnityEngine.Random.value;
			//Debug.Log("random " + random );
			// Corridor
			if(random < corridorPlaceChance){
				//Debug.Log("corridor");
				if(!BuildCorridor (map, Direction.RandomWeighted(cursorDirection, -0.8f), corridorLenght)){
					foreach(Vector2 direction in Direction.Directions){
						if (BuildCorridor (map, direction, corridorLenght)){
							corridorsToPlace--;
							break;
						}
					}
				}
				else if(roomsToPlace != 0){
					BuildRoom(map, cursor, roomSize);
					roomsToPlace--;
				}
				else {
					break;
				}
			}
			// Room
			if(random < roomPlaceChance){
				//Debug.Log(loop + "room");
				BuildRoom(map, cursor, roomSize);
				roomsToPlace--;
			}
			else{
				//Debug.Log(loop + " placed none");
			}
			total = roomsToPlace + corridorsToPlace;
		}
		BuildWalls (map);
		SpawnEnemies (map, enemyChance);
		SpawnItems (map, itemChance);
		return map;
	}

	private void SpawnPlayer(GameMap map, Vector2 position){
		GameManager.playerHandler.playerEntity = (Character)map.SpawnEntity (position, playerTemplate);
		map.SpawnFloor(position, floorTemplates[0]);
		cursor = position;
	}

	private bool BuildCorridor(GameMap map, Vector2 direction, int corridorLenght){
		if (map.heightMap.ScanForLine (true, cursor, direction, corridorLenght)) {		
			foreach (Vector2 position in map.heightMap.LineIterator(cursor, direction, corridorLenght)) {
				map.heightMap.SetLow (position);
				map.SpawnFloor (position, floorTemplates [0]);
				cursor = position;
				cursorDirection = direction;
			}
			return true;
		} else
			return false;
	}

	private void BuildRoom(GameMap map, Vector2 position, int size){
		for (float x = position.x; x < position.x + size; x++) {
			for (float y = position.y; y < position.y + size; y++) {
				Vector2 floorPosition = new Vector2(x,y);
				if(map.heightMap.Contains(floorPosition)){
					map.heightMap.SetLow(floorPosition);
					map.SpawnFloor(floorPosition, floorTemplates[0]);
					cursor = position;
				}
			}
		}

		// Door pos
		float value = UnityEngine.Random.value;
		if (value < 0.25f) {
			cursor.Set(position.x + (int)(size/2), position.y);
		} else if (value < 0.50f) {
			cursor.Set(position.x + (int)(size/2),  position.y + (int)(size/2));
		} else if (value < 0.750f) {
			cursor.Set(position.x + size - 1, position.y + (int)(size/2));
		} else {
			cursor.Set(position.x + (int)(size/2), position.y + size - 1);
		}
	}

	private void BuildWalls(GameMap map){
		for (float x = -5; x < map.heightMap.width + 5; x++) {
			for (float y = -5; y < map.heightMap.height + 5; y++) {
				Vector2 position = new Vector2(x,y);
				if(map.heightMap.Contains(position)){
					if(map.heightMap.IsLow(position) == false){
						map.SpawnWall(position, wallTemplates[0]);
					}
				}
				else{
					map.SpawnWall(position, wallTemplates[0]);
				}
			}
		}
	}

	private void SpawnEnemies(GameMap map, float chance){
		for (float x = 0; x < map.heightMap.width; x++) {
			for (float y = 0; y < map.heightMap.height; y++) {
				Vector2 position = new Vector2(x,y);
				if(map.heightMap.IsLow(position)){
					if(RandomHelper.Chance(chance)){
						map.SpawnEntity(position, enemyTemplates[0]);
					}
				}			
			}
		}
	}

	private void SpawnItems(GameMap map, float chance){
		for (float x = 0; x < map.heightMap.width; x++) {
			for (float y = 0; y < map.heightMap.height; y++) {
				Vector2 position = new Vector2(x,y);
				if(map.heightMap.IsLow(position)){
					if(RandomHelper.Chance(chance)){
						map.SpawnItem(position, itemTemplates[0]);
					}
				}			
			}
		}
	}
}

