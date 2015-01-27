using UnityEngine;
using System.Collections;

public enum power_type{
	none, spark, beam, fire, star
}

public class Enemy_1 : MonoBehaviour {

	public Transform kirby;
	public power_type power;
	public float 	leftAndRightEdge = 10f;
	public float	chanceToChangeDirections = 0.02f;
	public float 	speed = .5f;
	public float spawnTime = 3f; 
	private float distance;

	// Use this for initialization
	void Start () {
		renderer.enabled = false;
		distance = Mathf.Abs (kirby.position.x - transform.position.x);
	}
	
	// Update is called once per frame
	void Update () {
		//Basic Movement
		Vector3 pos = transform.position;
		pos.x += speed * Time.deltaTime;
		transform.position = pos;
		
		//Changing Directions
		if (pos.x < -leftAndRightEdge) {
			speed = Mathf.Abs (speed); //Move Right
		} else if (pos.x > leftAndRightEdge) {
			speed = -Mathf.Abs (speed); //Move left
		}

		Spawn ();
		//if kirby is nearby, use attack
		if (distance < 2) {
			//go towards kirby and attack
			Attack (); 
		}
	}

	void Spawn(){
		float distance = Mathf.Abs (kirby.position.x - transform.position.x);
		if (!renderer.enabled && distance > 2f) {
				gameObject.SetActive (true);
		}
	}

	void FixedUpdate(){
		if(Random.value < chanceToChangeDirections){
			speed *= -1; //Change Direction
		}
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Ground") {
			// GameObject thePE_Obj = GameObject.Find ("PE_Obj");
			PE_Obj my_obj = gameObject.GetComponent<PE_Obj> ();
			my_obj.acc = Vector3.zero; 
			my_obj.vel = Vector3.zero; 
//			my_obj.reached_ground = true; 
		}
//		if (col.gameObject.tag == "Player") {
//			Kirby kirb = kirby.gameObject.GetComponent<Kirby>();
//			kirb.Got_Attacked(); 
//		}
	}

	void Attack(){
		switch (power) {
			case power_type.none: 
				print ("ENEMY DOES NOTHING");
				break;
			case power_type.beam:
				print ("ENEMY ATTACKS WITH BEAM");
				break;
			case power_type.fire:
				print ("ENEMY ATTACKS WITH FIRE");
				break;
			case power_type.spark:
				print ("ENEMY ATTACKS WITH SPARK");
				break;
			}
		//TODO:
		//call kirby's got_attacked() if he got hit
		//maybe put it on the projectile
	}
		
}
