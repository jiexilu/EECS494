using UnityEngine;
using System.Collections;

public class Kirby_personal_space : MonoBehaviour {
	
	public Transform target;
	public Kirby kirby;
	public bool on_right;
	private Collider nearby_enemy;
	
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
		if (kirby.near_enemy  && kirby.cur_state == State.suck && kirby.power == power_type.none) {
			MoveObjectTowardsKirby(nearby_enemy);
		}
	}
	
	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Enemy") {
			Vector3 kirbyPosition = transform.position; 
			Vector3 targetPosition = col.gameObject.transform.position;
			Vector3 moveTowardsPosition = kirbyPosition - targetPosition;
			if((moveTowardsPosition.x < 0 && kirby.prev_dir == Direction.right) ||
			   (moveTowardsPosition.x > 0 && kirby.prev_dir == Direction.left)){
				kirby.near_enemy = true;
				nearby_enemy = col;	
			}
		}
	}
	
	void OnTriggerStay(Collider col){
		//		if (col.gameObject.tag == "Enemy") {
		//			Vector3 kirbyPosition = transform.position; 
		//			Vector3 targetPosition = col.gameObject.transform.position;
		//			Vector3 moveTowardsPosition = kirbyPosition - targetPosition;
		//			if((moveTowardsPosition.x < 0 && kirby.prev_dir == Direction.right) ||
		//				(moveTowardsPosition.x > 0 && kirby.prev_dir == Direction.left)){
		//				kirby.near_enemy = true;
		//				nearby_enemy = col;	
		//			}
		//		}
	}
	
	void OnTriggerLeave(Collider col){
		if (col.gameObject.tag == "Enemy") {
			kirby.near_enemy = false;
			nearby_enemy = null;
		}
	}
	
	void MoveObjectTowardsKirby(Collider col){
		Vector3 kirbyPosition = transform.position; 
		Vector3 targetPosition = col.gameObject.transform.position;
		Vector3 moveTowardsPosition = kirbyPosition - targetPosition;
		float speed = 5f;
		if((moveTowardsPosition.x < 0 && kirby.prev_dir == Direction.right) ||
		   (moveTowardsPosition.x > 0 && kirby.prev_dir == Direction.left)) {
			moveTowardsPosition.Normalize();
			col.gameObject.transform.Translate(
				(moveTowardsPosition.x * speed * Time.deltaTime),
				(moveTowardsPosition.y * speed * Time.deltaTime),
				(moveTowardsPosition.z * speed * Time.deltaTime),
				Space.World);
		}
	}
}
