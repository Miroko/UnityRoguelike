using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class MapGenerator
{
	private class HeightMap{
		// True = Down
		// False = Up
		private bool[,] positions;
		public int width, height;
		public Vector2 center;
		public Vector2 cursor;
			
		public HeightMap(int width, int height){
			this.width = width;
			this.height = height;
			center = new Vector2(width / 2, height / 2);
			cursor = center;
			positions = new bool[width,height];
		}

		public bool Contains(float x, float y){
			return (x >= 0 && x < width
			        && y >= 0 && y < height);
		}

		public bool Contains(Vector2 position){
			return Contains (position.x, position.y);
		}

		public bool IsLow(float x, float y){
			return positions [(int)x, (int)y];	
		}	

		
		public bool IsLow(Vector2 position){
			return IsLow ((int)position.x, (int)position.y);
		}

		public void Lower(float x, float y){
			positions [(int)x, (int)y] = true;
		}

		public void Lower(Vector2 position){
			Lower (position.x, position.y);
		}
	
		public bool ScanForLine(bool blocksWhenLow, Vector2 from, Vector2 direction, int lenght){
			foreach(Vector2 position in LineIterator(from, direction, lenght + 1)){	
				if(Contains(position)){
					if (IsLow(position) == blocksWhenLow) {
						return false;
					}
				}
				else{	
					return false;
				}
			}
			return true;
		}

		public IEnumerable<Vector2> LineIterator(Vector2 from, Vector2 direction, int lenght){
			Vector2 current = from;
			Vector2 to = current + new Vector2(direction.x * lenght, direction.y * lenght);

			int forceBreak = 0;
			while(current != to || forceBreak > 10){
				current = Vector2.MoveTowards (current, to, 1);
				forceBreak++;
				yield return current;
			}
		}

	}

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

		HeightMap heightMap = new HeightMap (width, height);
		GameMap map = new GameMap();
		SpawnPlayer (map, heightMap, heightMap.cursor);

		UnityEngine.Random.seed = seed;
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
				if(!BuildCorridor (map, heightMap, Direction.Random, corridorLenght)){
					foreach(Vector2 direction in Direction.Directions){
						if (BuildCorridor (map, heightMap, direction, corridorLenght)){
							corridorsToPlace--;
							break;
						}
					}
				}
				else if(roomsToPlace != 0){
					corridorsToPlace--;
				}
				else break;
			}
			// Room
			if(random < roomPlaceChance){
				//Debug.Log(loop + "room");
				BuildRoom(map, heightMap, heightMap.cursor, roomSize);
				roomsToPlace--;
			}
			else{
				//Debug.Log(loop + " placed none");
			}
			total = roomsToPlace + corridorsToPlace;
		}
		BuildWalls (map, heightMap);
		SpawnEnemies (map, heightMap, enemyChance);
		SpawnItems (map, heightMap, itemChance);
		return map;
	}

	private void SpawnPlayer(GameMap map, HeightMap heightMap, Vector2 position){
		GameManager.playerHandler.playerEntity = (Character)map.SpawnEntity (position, playerTemplate);
		heightMap.Lower(position);
		map.SpawnFloor(position, floorTemplates[0]);
		heightMap.cursor = position;
	}

	private bool BuildCorridor(GameMap map, HeightMap heightMap, Vector2 direction, int corridorLenght){
		if (heightMap.ScanForLine (true, heightMap.cursor, direction, corridorLenght)) {		
			foreach (Vector2 position in heightMap.LineIterator(heightMap.cursor, direction, corridorLenght)) {
				heightMap.Lower (position);
				map.SpawnFloor (position, floorTemplates [0]);
				heightMap.cursor = position;
			}
			return true;
		} else
			return false;
	}

	private void BuildRoom(GameMap map, HeightMap heightMap, Vector2 position, int size){
		for (float x = position.x; x < position.x + size; x++) {
			for (float y = position.y; y < position.y + size; y++) {
				Vector2 floorPosition = new Vector2(x,y);
				if(heightMap.Contains(floorPosition)){
					heightMap.Lower(floorPosition);
					map.SpawnFloor(floorPosition, floorTemplates[0]);
					heightMap.cursor = position;
				}
			}
		}

		// Cursor to random corner
		float value = UnityEngine.Random.value;
		if (value < 0.25f) {
			heightMap.cursor.Set(position.x, position.y);
		} else if (value < 0.50f) {
			heightMap.cursor.Set(position.x + size - 1, position.y);
		} else if (value < 0.750f) {
			heightMap.cursor.Set(position.x, position.y + size - 1);
		} else {
			heightMap.cursor.Set(position.x + size - 1, position.y + size - 1);
		}
	}

	private void BuildWalls(GameMap map, HeightMap heightMap){
		for (float x = -5; x < heightMap.width + 5; x++) {
			for (float y = -5; y < heightMap.height + 5; y++) {
				Vector2 position = new Vector2(x,y);
				if(heightMap.Contains(position)){
					if(heightMap.IsLow(position) == false){
						map.SpawnWall(position, wallTemplates[0]);
					}
				}
				else{
					map.SpawnWall(position, wallTemplates[0]);
				}
			}
		}
	}

	private void SpawnEnemies(GameMap map, HeightMap heightMap, float chance){
		for (float x = 0; x < heightMap.width; x++) {
			for (float y = 0; y < heightMap.height; y++) {
				Vector2 position = new Vector2(x,y);
				if(heightMap.IsLow(position)){
					if(RandomHelper.Chance(chance)){
						map.SpawnEntity(position, enemyTemplates[0]);
					}
				}			
			}
		}
	}

	private void SpawnItems(GameMap map, HeightMap heightMap, float chance){
		for (float x = 0; x < heightMap.width; x++) {
			for (float y = 0; y < heightMap.height; y++) {
				Vector2 position = new Vector2(x,y);
				if(heightMap.IsLow(position)){
					if(RandomHelper.Chance(chance)){
						map.SpawnItem(position, itemTemplates[0]);
					}
				}			
			}
		}
	}
}

