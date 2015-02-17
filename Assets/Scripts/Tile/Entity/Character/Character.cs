using UnityEngine;
using System.Collections;

public abstract class Character : Entity
{
	public int health;
	public int damage;

	public float movePerSecond;
	public Animator animator;

	private Vector3 moveDestination;
	public Vector3 getMoveDestination(){
		return moveDestination;
	}

	public bool isMoving = false;

	public abstract void HandleCollision (Tile tile);

	public void Start(){
		moveDestination = transform.position;
	}

	public void takeDamage(int amount){
		animator.SetTrigger ("onTakeDamage");
		health -= amount;
		if (health <= 0) {
			GameManager.gameMap.entityInstances.Remove(this);
			Destroy(gameObject);
		}
	}

	public void attack(Character entity){
		animator.SetTrigger ("onAttack");
		entity.takeDamage (damage);
	}

	public void SetMoveDestination(Vector3 destination){
		moveDestination = destination;
		isMoving = true;
	}

	public void Update(){
		MoveTowardsDestination();
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
}

