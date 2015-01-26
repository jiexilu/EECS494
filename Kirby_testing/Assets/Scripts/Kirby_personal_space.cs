using UnityEngine;
using System.Collections;

public class Kirby_personal_space : MonoBehaviour {

	public Transform target;
	public Kirby kirby;
	public bool on_right;

	// Use this for initialization
	void Start () {
		renderer.enabled = false;
		on_right = true;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tp = transform.position;
		tp = target.position;
		transform.position = tp;
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.name != "Kirby") {
				print ("bumped personal_space");
		}
	}
	//TODO: modify to work with different sides of kirby
	//Something is within Kirby's boundary
	void OnTriggerStay(Collider col){
		if (col.gameObject.tag == "Enemy") {
			kirby.near_enemy = true;
			if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Comma)) {
				if (col.gameObject.tag == "Enemy") {
					kirby.near_enemy = true;
					if(kirby.power == power_type.none && kirby.is_floating == false && kirby.has_enemy == false){
						MoveObjectTowardsKirby(col);
					}
					//hit it
					else{
						if(kirby.has_enemy){
							//shoot star out of mouth
							print ("shoot star");
							kirby.near_enemy = false;
							kirby.has_enemy = false;
						}
						else if(kirby.is_floating){
							//shoot puff ball
							print ("shoot puff ball");
							kirby.near_enemy = false;
						}
						else if(kirby.power != power_type.none){
							//attack
							print ("use special attack");
							kirby.near_enemy = false;
						}
					}
				}
			}
		}
	}

	void MoveObjectTowardsKirby(Collider col){
		Vector3 kirbyPosition = transform.position; 
		Vector3 targetPosition = col.gameObject.transform.position;
		Vector3 moveTowardsPosition = kirbyPosition - targetPosition;
		float speed = 4f;
		if((moveTowardsPosition.x < 0 && kirby.previous_direction == Buttons.right) ||
		   (moveTowardsPosition.x > 0 && kirby.previous_direction == Buttons.left)) {
			moveTowardsPosition.Normalize();
			col.gameObject.transform.Translate(
				(moveTowardsPosition.x * speed * Time.deltaTime),
				(moveTowardsPosition.y * speed * Time.deltaTime),
				(moveTowardsPosition.z * speed * Time.deltaTime),
				Space.World);
		}
	}
}
