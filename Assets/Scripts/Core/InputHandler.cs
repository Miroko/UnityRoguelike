using UnityEngine;
using System.Collections;

public class InputHandler
{

	public Vector2 GetMoveDirection(){	
		float x = Input.GetAxisRaw ("Horizontal");
		float y = Input.GetAxisRaw ("Vertical");

		if (x != 0 || y != 0) {
			if (x != 0) {
				if (x > 0) {
					return new Vector2 (1, 0);
				} else {
					return new Vector2 (-1, 0);
				}
			} else if (y != 0) {
				if (y > 0) {
					return new Vector2 (0, 1);
				} else {
					return new Vector2 (0, -1);
				}
			}		
		} 
		return Vector2.zero;
	}
}

