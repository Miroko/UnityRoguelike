using UnityEngine;
using System.Collections;

public abstract class Character : Tile
{
	public int health;
	public int damage;

	public float movePerSecond;
	public Animator animator;

	[HideInInspector] public Vector3 currentDirection;
	[HideInInspector] public bool isMoving = false;

	private Vector3 moveDestination;
	public Vector3 getMoveDestination(){
		return moveDestination;
	}

	public abstract void HandleCollision (Tile tile);

	public void Start(){
		moveDestination = transform.position;
		currentDirection = Direction.Random;
	}

	public virtual void TakeDamage(int amount){
		animator.SetTrigger ("onTakeDamage");
		health -= amount;
		if (health <= 0) {
			GameManager.gameMap.characterInstances.Remove(this);
			Destroy(gameObject);
		}
	}

	public void Attack(Tile tile){
		animator.SetTrigger ("onAttack");
		if (tile is Character) {
			((Character)tile).TakeDamage (damage);
		}else if(tile is BreakableWall){
			((BreakableWall)tile).TakeDamage(damage);
		}
	}

	public virtual bool Move(GameMap map, Vector3 direction){
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
				foreach (Item item in map.GetItemsAt(destination)) {
					HandleCollision (item);
				}
				foreach (Functional functional in map.GetFunctionalsAt(destination)) {
					HandleCollision (functional);	
					if(functional.Blocks(this)) return false;
				}
				SetMoveDestination (destination);
				return true;
			}		
			else{
				foreach (Functional functional in map.GetFunctionalsAt(destination)) {
					HandleCollision (functional);	
					if(functional.Blocks(this)) return false;
				}
			}
		}
		return false;
	}

	public void SetMoveDestination(Vector3 destination){
		moveDestination = destination;
		isMoving = true;
	}

	private void MoveTowardsDestination(){
		if (Vector3.Distance (transform.position, moveDestination) < 0.001f) {
			transform.position = moveDestination;
			isMoving = false;
		} else {
			transform.position = Vector3.MoveTowards(transform.position, moveDestination, Time.deltaTime * movePerSecond);
		}
	}

	public void Update(){
		MoveTowardsDestination();
	}	

}

