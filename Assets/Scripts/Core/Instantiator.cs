using UnityEngine;
using System.Collections;

public class Instantiator
{	
	public GameObject Instantiate(Vector2 position, GameObject template){
		return MonoBehaviour.Instantiate(template, position, Quaternion.identity) as GameObject;
	}
	/*
	public Item InstantiateItem(Vector2 position, GameObject template){
		GameObject gameObject = MonoBehaviour.Instantiate(template, position, Quaternion.identity) as GameObject;
		return (Item)gameObject.GetComponent (typeof(Item)) as Item;
	}

	public Entity InstantiateEntity(Vector2 position, GameObject template){
		GameObject gameObject = MonoBehaviour.Instantiate(template, position, Quaternion.identity) as GameObject;
		return (Entity)gameObject.GetComponent (typeof(Entity)) as Entity;
	}	

	public Floor InstantiateFloor(Vector2 position, GameObject template){
		GameObject gameObject = MonoBehaviour.Instantiate(template, position, Quaternion.identity) as GameObject;
		return (Floor)gameObject.GetComponent (typeof(Floor)) as Floor;
	}	

	public Wall InstantiateWall(Vector2 position, GameObject template){
		GameObject gameObject = MonoBehaviour.Instantiate(template, position, Quaternion.identity) as GameObject;
		return (Wall)gameObject.GetComponent (typeof(Wall)) as Wall;
	}	
*/
}

