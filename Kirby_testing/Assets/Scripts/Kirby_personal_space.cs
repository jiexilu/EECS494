using UnityEngine;
using System.Collections;

public class Kirby_personal_space : MonoBehaviour {

	public Transform target;

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
			if (Input.GetKey(KeyCode.B)) {
				if (col.gameObject.tag == "Enemy") {
					//print ("suck in enemy");

					MoveObjectTowardsKirby(col);
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
