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


