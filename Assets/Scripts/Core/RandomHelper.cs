using UnityEngine;
using System.Collections;

public class RandomHelper
{

	public static bool Chance(float chance){
		if (UnityEngine.Random.value < chance)
			return true;
		else return false;
	}

}

