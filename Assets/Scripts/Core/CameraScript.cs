using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
	public float moveSpeed;

	void Awake(){
		DontDestroyOnLoad (gameObject);
	}

	void Update ()
	{
		Vector3 newPos = Vector3.MoveTowards (camera.transform.position,
		                                     GameManager.playerHandler.playerCharacter.transform.position,
		                                     moveSpeed);
		newPos.z = camera.transform.position.z;

		camera.transform.position = newPos;
	}
}

