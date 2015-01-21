using UnityEngine;
using System.Collections;

public class Kirby_personal_space : MonoBehaviour {

	public Transform target;
	public Kirby kirby;

	// Use this for initialization
	void Start () {
		renderer.enabled = false;
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

	//Something is within Kirby's boundary
	void OnTriggerStay(Collider col){
		if (col.gameObject.name != "Kirby") {
			if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Comma)) {
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
						print ("ENEMY GONE!");
						col.gameObject.SetActive(false);
						col.gameObject.renderer.enabled = false;
					}
				}
			}
		}
	}

	void MoveObjectTowardsKirby(Collider col){
		float speed = 3f;
		Vector3 kirbyPosition = transform.position; 
		Vector3 targetPosition = col.gameObject.transform.position;
		Vector3 moveTowardsPosition = kirbyPosition - targetPosition;
		moveTowardsPosition.Normalize();

		col.gameObject.transform.Translate(
			(moveTowardsPosition.x * speed * Time.deltaTime),
			(moveTowardsPosition.y * speed * Time.deltaTime),
			(moveTowardsPosition.z * speed * Time.deltaTime),
			Space.World);
	}
}
