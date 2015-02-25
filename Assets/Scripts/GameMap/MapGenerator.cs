using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class MapGenerator
{
	private Vector2 cursorPosition;
	private Vector2 cursorDirection;

	public  int currentLevel;
	private int width, height, seed, rooms, corridors, roomSize, corridorLenght;
	private float enemyChance, itemChance;

	public GameObject exitTemplate;
	public GameObject breakableWallTemplate;
	public GameObject[] enemyTemplates;
	public GameObject[] itemTemplates;
	public GameObject[] floorTemplates;
	public GameObject[] wallTemplates;
	public GameObject[] roomTemplates;

	public MapGenerator(int width, int height, int seed, int rooms, int corridors, int roomSize, int corridorLenght,
	                    float enemyChance, float itemChance, int currentLevel,
	                    GameObject exitTemplate, GameObject breakableWallTemplate,
	                    GameObject[] floorTemplates, GameObject[] wallTemplates, GameObject[] enemyTemplates,
	                    GameObject[] itemTemplates){
		this.width = width;
		this.height = height;
		this.seed = seed;
		this.rooms = rooms;
		this.corridors = corridors;
		this.roomSize = roomSize;	
		this.corridorLenght = corridorLenght;
		this.enemyChance = enemyChance;
		this.itemChance = itemChance;
		this.currentLevel = currentLevel;

		this.exitTemplate = exitTemplate;
		this.breakableWallTemplate = breakableWallTemplate;
		this.enemyTemplates = enemyTemplates;
		this.floorTemplates = floorTemplates;
		this.wallTemplates = wallTemplates;
		this.itemTemplates = itemTemplates;
	}

	public GameMap NewGameMap(int level){
		GameMap map = new GameMap(width, height);
		Vector2 center = new Vector2 (width/2, height/2);
		UnityEngine.Random.seed = (seed * level);

		cursorPosition = center;

		map.SpawnFloor(cursorPosition, floorTemplates[0]);
		SpawnPlayer (map, cursorPosition);

		int roomsToPlace = rooms; 
		int corridorsToPlace = corridors; 
		float total = roomsToPlace + corridorsToPlace;
		float left = total;
		for (int loop = 0; loop < total; loop++) {		
			float roomPlaceChance = roomsToPlace / left;
			float corridorPlaceChance = corridorsToPlace / left;
			float random = UnityEngine.Random.value;
			if(random <= corridorPlaceChance){
				BuildCorridor(map, Direction.Random, corridorLenght);
				corridorsToPlace--;
			}
			else{
				BuildRoom(map, cursorPosition, roomSize);
				roomsToPlace--;
			}
			left = roomsToPlace + corridorsToPlace;
		}
		SpawnExit (map, cursorPosition);
		BuildWalls (map);
		BuildBreakableWalls (map);
		SpawnEnemies (map, enemyChance);
		SpawnItems (map, itemChance);
		BuildOuterWall (map);
		return map;
	}

	private void BuildOuterWall(GameMap map){
		for (float x = -5; x < map.heightMap.width + 5; x++) {
			for (float y = -5; y < map.heightMap.height + 5; y++) {
				if(map.heightMap.Contains(x,y) == false){		
					Vector2 position = new Vector2(x,y);
					GameManager.instantiator.Instantiate(position, wallTemplates[0]);
				}
			}
		}
	}

	private void SpawnExit(GameMap map, Vector2 position){
		map.SpawnExit (position, exitTemplate);
	}

	private void SpawnPlayer(GameMap map, Vector2 position){
		GameManager.playerHandler.playerCharacter.transform.position = position;
		GameManager.playerHandler.playerCharacter.SetMoveDestination (position);
		map.characterInstances.Add (GameManager.playerHandler.playerCharacter);
	}

	private void BuildCorridor(GameMap map, Vector2 direction, int corridorLenght){
		foreach (Vector2 position in map.heightMap.LineIterator(cursorPosition, direction, corridorLenght)) {
			if(map.heightMap.Contains(position.x, position.y)){
				map.heightMap.SetLow (position);
				map.SpawnFloor (position, floorTemplates [0]);
				cursorPosition = position;
			}
		}
		cursorDirection = direction;
	}

	private void BuildRoom(GameMap map, Vector2 position, int size){
		for (float x = position.x - (size/2); x <= position.x + (size/2); x++) {
			for (float y = position.y - (size/2); y <= position.y + (size/2); y++) {
				Vector2 floorPosition = new Vector2(x,y);
				if(map.heightMap.Contains(floorPosition.x, floorPosition.y)){
					map.heightMap.SetLow(floorPosition);
					map.SpawnFloor(floorPosition, floorTemplates[0]);
				}
			}
		}
		if (map.heightMap.Contains (position)) {
			cursorPosition = position;
		}
	}

	private void BuildWalls(GameMap map){
		for (float x = 0; x < map.heightMap.width; x++) {
			for (float y = 0; y < map.heightMap.height; y++) {
				Vector2 position = new Vector2(x,y);
				if(map.heightMap.IsLow(position) == false){
					map.SpawnWall(position, wallTemplates[0]);
				}
			}
		}
	}

	private void BuildBreakableWalls(GameMap map){
		for (float x = 0; x < map.heightMap.width; x++) {
			for (float y = 0; y < map.heightMap.height; y++) {
				Vector3 position = new Vector3(x,y);
				if(map.heightMap.IsLow(position) && position != GameManager.playerHandler.playerCharacter.transform.position){
					if(RandomHelper.Chance(0.6f)){
						if(map.GetFunctionalsAt(position).Count == 0){
							int surroundingWalls = 0;
							for(float surroundingX = x - 1; surroundingX <= x + 1; surroundingX++){
								for(float surroundingY = y - 1; surroundingY <= y + 1; surroundingY++){
									if(map.heightMap.Contains(surroundingX, surroundingY)){
										if(map.heightMap.IsLow(surroundingX, surroundingY) == false){
											if(map.GetFunctionalsAt(new Vector2(surroundingX, surroundingY)).Count == 0){
												surroundingWalls++;
											}
										}
									}
								}
							}
							if(surroundingWalls > 3){
								map.SpawnBreakableWall(position, breakableWallTemplate);
							}
						}
					}
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
						map.SpawnCharacter(position, enemyTemplates[0]);
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

