using UnityEngine;
using System.Collections;

public abstract class Character : Tile
{
	public int health;
	public int damage;

	public float movePerSecond;
	public Animator animator;


	public Vector3 currentDirection;

	private Vector3 moveDestination;
	public Vector3 getMoveDestination(){
		return moveDestination;
	}

	public bool isMoving = false;

	public abstract void HandleCollision (Tile tile);

	public void Start(){
		moveDestination = transform.position;
		currentDirection = Direction.Random;
	}

	public void takeDamage(int amount){
		animator.SetTrigger ("onTakeDamage");
		health -= amount;
		if (health <= 0) {
			GameManager.gameMap.characterInstances.Remove(this);
			Destroy(gameObject);
		}
	}

	public void attack(Character entity){
		animator.SetTrigger ("onAttack");
		entity.takeDamage (damage);
	}

	public bool Move(GameMap map, Vector3 direction){
		currentDirection = direction;
		Vector3 currentPosition = gameObject.transform.position;
		Vector3 destination = currentPosition + currentDirection;
		if (map.heightMap.Contains (destination)) {
			if (map.heightMap.IsLow (destination)) {
				foreach (Character entity in map.GetCharactersAt(destination)) {
					if (entity != this) {
						if (entity.Blocks (this)) {	
							HandleCollision (entity);
							return false;
						}
					}
				}
				SetMoveDestination (destination);
				foreach (Item item in map.GetItemsAt(destination)) {
					HandleCollision (item);
				}
				return true;
			}
		}
		return false;
	}

	private void SetMoveDestination(Vector3 destination){
		moveDestination = destination;
		isMoving = true;
	}

	private void MoveTowardsDestination(){
		if (moveDestination != transform.position) {
			transform.position = Vector3.MoveTowards(transform.position, moveDestination, Time.deltaTime * movePerSecond);
			if(Vector3.Distance(transform.position, moveDestination) < 0.01f){
				transform.position = moveDestination;
				isMoving = false;
			}
		}
		
	}

	public void Update(){
		MoveTowardsDestination();
	}	

}

