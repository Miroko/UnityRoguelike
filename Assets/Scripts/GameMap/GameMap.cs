using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMap
{
	public List<Character> characterInstances = new List<Character>();
	public List<Item> itemInstances = new List<Item> ();
	public List<Functional> functionalInstances = new List<Functional> ();

	public Heightmap heightMap;

	public GameMap(int width, int height){
		heightMap = new Heightmap (width, height);
	}
	
	public ArrayList GetItemsAt(Vector3 point){
		ArrayList query = new ArrayList();
		foreach (Item item in itemInstances) {
			if(item.transform.position == point){
				query.Add(item);
			}
		}
		return query;
	}

	public ArrayList GetFunctionalsAt(Vector3 point){
		ArrayList query = new ArrayList();
		foreach (Functional functional in functionalInstances) {
			if (functional.transform.position == point){
				query.Add(functional);
			}
		}
		return query;
	}
	
	public ArrayList GetCharactersAt(Vector3 point){
		ArrayList query = new ArrayList();
		foreach (Character entity in characterInstances) {
			if(entity.isMoving){
				if(entity.getMoveDestination() == point){
					query.Add(entity);
				}
			}
			else if (entity.transform.position == point){
				query.Add(entity);
			}
		}
		return query;
	}

	public void SpawnFloors(Bounds bounds, GameObject template){
		for (float x = bounds.min.x; x < bounds.max.x; x++) {
			for (float y = bounds.min.y; y < bounds.max.y; y++) {
				SpawnFloor(new Vector2(x,y), template);
			}
		}	
	}
	
	public void SpawnWalls(Bounds bounds, GameObject template){
		for (float x = bounds.min.x; x <= bounds.max.x; x++) {
			for (float y = bounds.min.y; y <= bounds.max.y; y++) {
				Wall wall = GameManager.instantiator.Instantiate(new Vector2(x, y), template).gameObject.GetComponent<Wall>();	
				if(heightMap.Contains(bounds.min) && heightMap.Contains(bounds.max)){
					heightMap.SetHigh(x, y);
				}
			}
		}	
	}

	public Floor SpawnFloor(Vector2 position, GameObject template){
		Floor floor = GameManager.instantiator.Instantiate(position, template).gameObject.GetComponent<Floor>();
		heightMap.SetLow(position.x, position.y);
		return floor;
	}

	public Wall SpawnWall(Vector2 position, GameObject template){
		Wall wall = GameManager.instantiator.Instantiate(position, template).gameObject.GetComponent<Wall>();
		if (heightMap.Contains (position)) {
			heightMap.SetHigh (position.x, position.y);
		}
		return wall;
	}

	public Item SpawnItem(Vector2 position, GameObject template){
		Item item = GameManager.instantiator.Instantiate(position, template).gameObject.GetComponent<Item>();
		itemInstances.Add (item);
		return item;
	}

	public Character SpawnCharacter(Vector2 position, GameObject template){
		Character character = GameManager.instantiator.Instantiate(position, template).gameObject.GetComponent<Character>();
		characterInstances.Add (character);
		return character;
	}

	public Exit SpawnExit(Vector2 position, GameObject template){
		Exit exit = GameManager.instantiator.Instantiate(position, template).gameObject.GetComponent<Exit>();
		functionalInstances.Add (exit);
		return exit;
	}

}

