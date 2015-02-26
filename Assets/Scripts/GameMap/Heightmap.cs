using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Heightmap
{
	// True = Down
	// False = Up
	private bool[,] positions;
	public int width, height;
	
	public Heightmap(int width, int height){
		this.width = width;
		this.height = height;
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
	
	public void SetLow(float x, float y){
		positions [(int)x, (int)y] = true;
	}
	
	public void SetLow(Vector2 position){
		SetLow (position.x, position.y);
	}
	
	public void SetHigh(float x, float y){
		positions [(int)x, (int)y] = false;
	}
	
	public void SetHigh(Vector2 position){
		SetHigh (position.x, position.y);
	}
	
	public bool[,] IsLow(Bounds bounds){
		int width = (int)(bounds.max.x - bounds.min.x);
		int height = (int)(bounds.max.y - bounds.min.y);
		bool[,] inBounds = new bool[width, height];
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if(IsLow(x, y)){
					inBounds[x,y] = true;
				}
			}
		}
		return inBounds;
	}

	public bool ScanForOpenLine(bool highBreaks, Vector3 from, Vector3 direction, int lenght){
		foreach (Vector2 positionOnLine in LineIterator(from, direction, lenght)) {
			if(!IsLow(positionOnLine) == highBreaks){
				return false;
			}		
		}
		return true;
	}

	public bool ScanForOpenLine(bool highBreaks, Vector3 from, Vector3 destination){
		foreach (Vector2 positionOnLine in LineIterator(from, destination)) {
			if(!IsLow(positionOnLine) == highBreaks){				
				return false;
			}
		}
		return true;
	}

	private const int maxScanDistance = 10;
	public IEnumerable<Vector3> LineIterator(Vector3 from, Vector3 direction, int lenght){
		Vector3 current = from;
		Vector3 destination = new Vector2 (from.x + (direction.x * lenght), from.y + (direction.y * lenght));
		if (Vector3.Distance (from, destination) < maxScanDistance) {
			while (current != destination) {
				current = Vector3.MoveTowards (current, destination, 1);				
				if(Contains(current)){
					yield return current;	
				}	
			}
		}
	}

	public IEnumerable<Vector3> LineIterator(Vector3 from, Vector3 destination){
		Vector3 current = from;
		if (Vector3.Distance (from, destination) < maxScanDistance) {
			while (current != destination) {
				current = Vector3.MoveTowards (current, destination, 1);		
				if(Contains(current)){
					yield return current;	
				}
			}
		}
	}
}


