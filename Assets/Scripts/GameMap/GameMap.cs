using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMap
{
	public List<Entity> entityInstances = new List<Entity>();
	public List<Floor> floorInstances = new List<Floor> ();
	public List<Wall> wallInstances = new List<Wall> ();
	public List<Item> itemInstances = new List<Item> ();

	public ArrayList GetFloorsIn(Bounds bounds){
		ArrayList query = new ArrayList ();
		foreach(Floor floor in floorInstances){
			if(bounds.Contains(floor.transform.position)){
				query.Add(floor);
			}
		}
		return query;
	}
	
	public ArrayList GetWallsIn(Bounds bounds){
		ArrayList query = new ArrayList ();
		foreach(Wall wall in wallInstances){
			if(bounds.Contains(wall.transform.position)){
				query.Add(wall);
			}
		}
		return query;
	}

	public ArrayList GetWallsAt(Vector3 point){
		ArrayList query = new ArrayList();
		foreach (Wall wall in wallInstances) {	
			if (wall.transform.position == point){
				query.Add(wall);
			}
		}
		return query;
	}

	public ArrayList GetItemsIn(Bounds bounds){
		ArrayList query = new ArrayList ();
		foreach(Item item in itemInstances){
			if(bounds.Contains(item.transform.position)){
				query.Add(item);
			}
		}
		return query;
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
				Floor floor = GameManager.instantiator.Instantiate(new Vector2(x, y), template).gameObject.GetComponent<Floor>();
				floorInstances.Add(floor);
			}
		}	
	}
	
	public void SpawnWalls(Bounds bounds, GameObject template){
		for (float x = bounds.min.x; x <= bounds.max.x; x++) {
			for (float y = bounds.min.y; y <= bounds.max.y; y++) {
				Wall wall = GameManager.instantiator.Instantiate(new Vector2(x, y), template).gameObject.GetComponent<Wall>();	
				wallInstances.Add(wall);
			}
		}	
	}

	public Item SpawnItem(Vector2 position, GameObject template){
		Item item = GameManager.instantiator.Instantiate(position, template).gameObject.GetComponent<Item>();
		itemInstances.Add (item);
		return item;
	}

	public Floor SpawnFloor(Vector2 position, GameObject template){
		Floor floor = GameManager.instantiator.Instantiate(position, template).gameObject.GetComponent<Floor>();
		return floor;
	}

	public Wall SpawnWall(Vector2 position, GameObject template){
		Wall wall = GameManager.instantiator.Instantiate(position, template).gameObject.GetComponent<Wall>();
		wallInstances.Add (wall);
		return wall;
	}

	public Entity SpawnEntity(Vector2 position, GameObject template){
		Entity entity = GameManager.instantiator.Instantiate(position, template).gameObject.GetComponent<Entity>();
		entityInstances.Add (entity);
		return entity;
	}

	public void MoveEntity(Character entityToMove, Vector3 direction){
		bool canMove = true;

		Vector3 destination = new Vector3 ();
		destination.Set (entityToMove.transform.position.x + direction.x, entityToMove.transform.position.y + direction.y, entityToMove.transform.position.z);

		foreach (Wall wall in GetWallsAt(destination)) {
			if (wall.Blocks (entityToMove)) {
				canMove = false;
				return;
			}
		}

		foreach (Character entity in GetEntitiesAt(destination)) {
			if(entity != entityToMove){
				if (entity.Blocks (entityToMove)) {
					canMove = false;
				}
				entityToMove.HandleCollision (entity);
			}
		}

		if (canMove) {
			foreach(Item item in GetItemsAt(destination)){
				entityToMove.HandleCollision(item);
			}

			entityToMove.SetMoveDestination (destination);
		}
	}

}

