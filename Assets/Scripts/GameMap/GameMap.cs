using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMap
{
	public List<Entity> entityInstances = new List<Entity>();
	//public List<Floor> floorInstances = new List<Floor> ();
	//public List<Wall> wallInstances = new List<Wall> ();
	public List<Item> itemInstances = new List<Item> ();

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
	
	public ArrayList GetEntitiesAt(Vector3 point){
		ArrayList query = new ArrayList();
		foreach (Character entity in entityInstances) {
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

	public Entity SpawnEntity(Vector2 position, GameObject template){
		Entity entity = GameManager.instantiator.Instantiate(position, template).gameObject.GetComponent<Entity>();
		entityInstances.Add (entity);
		return entity;
	}


}

