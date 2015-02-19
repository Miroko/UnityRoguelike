using UnityEngine;
using System.Collections;

public class RandomHelper
{

	public static bool Chance(float chance){
		if (chance < 0) {
			if ((UnityEngine.Random.value - chance) < chance)
				return true;
			else return false;
		}
		else if (UnityEngine.Random.value < chance)
			return true;
		else return false;
	}
}

