using UnityEngine;
using System.Collections;

public class MapGenerator
{
	private GameMap map;

	private Vector2 builderPosition;
	private Vector2 builderDirection;

	private bool[,] closedPositions;

	private Bounds floorBounds;

	public MapGenerator(GameMap map, Vector2 builderStartPosition){
		this.map = map;
		this.builderPosition = builderStartPosition;
		closedPositions = new bool[(int)map.bounds.size.x + 1, (int)map.bounds.size.y + 1];
		floorBounds = map.bounds;
		floorBounds.min = new Vector2 (floorBounds.min.x + 1, floorBounds.min.x + 1);
		floorBounds.max = new Vector2 (floorBounds.max.x - 1, floorBounds.max.y - 1);

	}

	private void newBuilderDirection(){
		Vector2 direction = RandomHelper.randomDirection;
		while (direction == builderDirection) {
			direction = RandomHelper.randomDirection;
		}
		builderDirection = direction;
	}

	public void BuildCorridor(int corridorLenght){
		// Change direction
		newBuilderDirection ();

		// If open build corridor floor
		if (ScanForOpenLine (builderPosition, builderDirection, corridorLenght)) {
			BuildFloor (builderPosition, builderDirection, corridorLenght);

			// Set new position
			builderPosition = new Vector2(builderPosition.x + (builderDirection.x * corridorLenght),
			                              builderPosition.y + (builderDirection.y * corridorLenght));
		}
	}

	public void BuildRoom(int width, int height){
		Bounds area = new Bounds ();
		area.SetMinMax (builderPosition, new Vector2 (builderPosition.x + width, builderPosition.y + height));
		builderPosition = new Vector2(builderPosition.x + width - 1, builderPosition.y + height);

		BuildRoomFloor(area);
	}

	public void BuildRoomFloor(Bounds area){
		if (floorBounds.Contains (area.min) && floorBounds.Contains (area.max)) {
			for(float x = area.min.x; x < area.max.x; x++){
				for(float y = area.min.y; y < area.max.y; y++){
					map.SpawnFloor(new Vector2(x,y), map.floorTemplates[0]);
					closedPositions[(int)x,(int)y] = true;
				}
			}
		}
	}

	public void BuildFloor(Vector2 from, Vector2 direction, int corridorLenght){
		Vector2 to = new Vector2 (from.x + (direction.x * corridorLenght), from.y + (direction.y * corridorLenght));
		if (floorBounds.Contains (from) && floorBounds.Contains (to)) {				
			Vector2 current = from;
			int forceBreak = 0;
			while (current != to) {								
				map.SpawnFloor(current, map.floorTemplates[0]);
				closedPositions[(int)current.x, (int)current.y] = true;
				current = Vector2.MoveTowards (current, to, 1);	
				forceBreak++;
				if (forceBreak >= corridorLenght) break;				
			}		
		}
	}

	public void BuildWalls(){
		for(float x = floorBounds.min.x; x <= floorBounds.max.x; x++){
			for(float y = floorBounds.min.y; y <= floorBounds.max.y; y++){
				if(closedPositions[(int)x,(int)y] == true){
					BuildWallAroundFloor(x,y);
				}
			}
		}
	}

	public void SpawnEnemies(float chance){
		for(float x = floorBounds.min.x; x <= floorBounds.max.x; x++){
			for(float y = floorBounds.min.y; y <= floorBounds.max.y; y++){
				if(closedPositions[(int)x,(int)y] == true){
					if(RandomHelper.Chance(chance)){
						map.SpawnEntity(new Vector2(x,y), map.enemyTemplates[0]);
					}
				}
			}
		}
	}

	public void SpawnItems(float chance){
		for(float x = floorBounds.min.x; x <= floorBounds.max.x; x++){
			for(float y = floorBounds.min.y; y <= floorBounds.max.y; y++){
				if(closedPositions[(int)x,(int)y] == true){
					if(RandomHelper.Chance(chance)){
						map.SpawnItem(new Vector2(x,y), map.itemTemplates[0]);
					}
				}
			}
		}
	}
	
	private void BuildWallAroundFloor(float x, float y){
		Vector2 foorPosition = new Vector2 (x, y);
		Vector2 wallPosition;

		wallPosition = foorPosition + Direction.Up;
		if (map.bounds.Contains (wallPosition)) {
			if (closedPositions [(int)wallPosition.x, (int)wallPosition.y] == false) {
				map.SpawnWall (wallPosition, map.wallTemplates [0]);
			}
		}
		wallPosition = foorPosition + Direction.Down;
		if (map.bounds.Contains (wallPosition)) {
			if (closedPositions [(int)wallPosition.x, (int)wallPosition.y] == false) {
				map.SpawnWall (wallPosition, map.wallTemplates [0]);
			}
		}
		wallPosition = foorPosition + Direction.Left;
		if (map.bounds.Contains (wallPosition)) {
			if (closedPositions [(int)wallPosition.x, (int)wallPosition.y] == false) {
				map.SpawnWall (wallPosition, map.wallTemplates [0]);
			}
		}
		wallPosition = foorPosition + Direction.Right;
		if (map.bounds.Contains (wallPosition)) {
			if (closedPositions [(int)wallPosition.x, (int)wallPosition.y] == false) {
				map.SpawnWall (wallPosition, map.wallTemplates [0]);
			}		
		}
	}
	
	public bool ScanForOpenLine(Vector2 from, Vector2 direction, int corridorLenght){
		bool open = true;
		Vector2 to = new Vector2 (from.x + (direction.x * corridorLenght), from.y + (direction.y * corridorLenght));
		if (floorBounds.Contains (from) && floorBounds.Contains (to)) {				
			Vector2 current = from;
			int forceBreak = 0;
			while (current != to) {
				current = Vector2.MoveTowards (current, to, 1);					
				if (closedPositions [(int)current.x, (int)current.y] == true) {
					open = false;
					break;
				}
				forceBreak++;
				if (forceBreak >= corridorLenght) break;
			}		
		} else {
			open = false;
		}
		return open;
	}

}

