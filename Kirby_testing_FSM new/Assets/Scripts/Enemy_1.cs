using UnityEngine;
using System.Collections;

public enum power_type{
	none, spark, beam, fire, star, sing, ouch
}

public enum enemy_state{
	wander, chase, attack
}

//TODO: adjust velocities not their positions
//TODO: raycasting for going after kirby
public class Enemy_1 : MonoBehaviour {
	
	public Animator sprite;
	public Transform kirby;
	public power_type power;
	public float speed = 30f;
	private float distance;
	public int score = 400; //points earned for destroying
	
	//Pacing//
	//	public Vector3 start;
	//	public Vector3 pointA;
	//	public Vector3 pointB;
	
	
	private float usage;
	private float delay;
	private Vector3 vel;
	private PE_Obj my_obj;
	private RaycastHit hit;
	private RaycastHit attack_ray;
	public enemy_state state;
	private float fieldOfViewRange = 68.0f;
	private Vector3 rayDir = Vector3.zero;
	private int index = 0;
	public Direction cur_dir = Direction.left;
	public bool set_delay = false;
	public bool attack = false;
	
	//powers
	public GameObject beam_string;
	public GameObject fireball;
	public GameObject spark;
	
	// Use this for initialization
	void Start () {
		my_obj = GetComponent<PE_Obj> ();
		delay = 0.5f;
		distance = Mathf.Abs (kirby.position.x - transform.position.x);
		state = enemy_state.wander;
		gameObject.renderer.material.color = Color.clear;
		vel = my_obj.vel;
		vel.x = -speed;
		my_obj.vel = vel;
	}
	
	// Update is called once per frame
	void Update () {
		//Basic movement to the right
		vel = my_obj.vel; 
		//		-------
		//if they bump something, change directions or jump
		rayDir = kirby.position - transform.position;
		
		//		if (((Vector3.Angle (rayDir, Vector3.left)) < fieldOfViewRange) || 
		//		    (Vector3.Angle(rayDir, Vector3.right) < fieldOfViewRange)) {
		//			if(Physics.Raycast (transform.position, rayDir, out hit, 200f)){
		//				if(hit.transform.tag == "Player"){
		//					if(hit.distance < 1f){
		////						print ("distance " + hit.distance);
		//						print ("attack state");
		//						state = enemy_state.attack;
		//					}
		//					state = enemy_state.chase;
		//				}
		//			}
		//		}
		
		switch (state) {
		case enemy_state.wander:
			state_wander();
			break;
		case enemy_state.chase:
			state_chase();
			break;
		case enemy_state.attack:
			Attack();
			break;
		}
		
		if (vel.x < 0) {
			sprite.SetInteger("Action", 0);	
			cur_dir = Direction.left;
		} else {
			sprite.SetInteger("Action", 1);	
			cur_dir = Direction.right;
		}
		
		my_obj.vel = vel;
	}
	
	
	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Ground") {
			// GameObject thePE_Obj = GameObject.Find ("PE_Obj");
			PE_Obj my_obj = gameObject.GetComponent<PE_Obj> ();
			my_obj.acc = Vector3.zero; 
			my_obj.vel = Vector3.zero; 
			//			my_obj.reached_ground = true; 
		}
		if (col.gameObject.tag == "Enemy") {
			Physics.IgnoreCollision(gameObject.collider, col);
		}
		//		if (col.gameObject.tag == "Player") {
		//			Kirby kirb = kirby.gameObject.GetComponent<Kirby>();
		//			kirb.Got_Attacked(); 
		//		}
	}
	
	void Attack(){
		if (!set_delay) {
			usage = Time.time + delay;		
			set_delay = true;
			attack = false;
		}
		print ("power " + power);
		if (Time.time < usage && attack == false) {
			my_obj.acc = Vector3.zero;
			my_obj.vel = Vector3.zero;
			switch (power) {
			case power_type.none: 
				print ("ENEMY DOES NOTHING");
				break;
			case power_type.beam:
				print ("ENEMY ATTACKS WITH BEAM");
				Beam (); //every 3 seconds 
				break;
			case power_type.fire:
				print ("ENEMY ATTACKS WITH FIRE");
				flames ();
				break;
			case power_type.spark:
				print ("ENEMY ATTACKS WITH SPARK");
				Spark();
				break;
			}
			attack = true;
		}
		//set timer
		if (Time.time > usage) {
			state = enemy_state.wander;
			set_delay = false;
		}
	}
	
	void state_wander(){
		print ("wandering enemy");
		if (((Vector3.Angle (rayDir, Vector3.left)) < fieldOfViewRange) || 
		    (Vector3.Angle(rayDir, Vector3.right) < fieldOfViewRange)) {
			if(Physics.Raycast (transform.position, rayDir, out hit, 200f)){
				if(hit.transform.tag == "Player"){
					state = enemy_state.chase;
				}
			}
		}
		
		if (Physics.Raycast (transform.position, Vector3.left, out hit, 10f)) {
			if(hit.collider.tag == "Wall" || hit.collider.tag == "Ground"){
				print ("hit something on left" + gameObject.name);
				vel.x = speed;
			}
		}if (Physics.Raycast (transform.position, Vector3.right, out hit, 10f)) {
			if(hit.collider.tag == "Wall" || hit.collider.tag == "Ground"){
				print ("hit something on right" + gameObject.name);
				vel.x = -1 * speed;
			}
		}
	}
	
	void state_chase(){
		print ("chasing kirby");
		Vector3 temp = rayDir;
		temp.Normalize ();
		vel.x = temp.x * speed;
		
		if (((Vector3.Angle (rayDir, Vector3.left)) < fieldOfViewRange) || 
		    (Vector3.Angle(rayDir, Vector3.right) < fieldOfViewRange)) {
			if(Physics.Raycast (transform.position, rayDir, out hit, 200f)){
				if(hit.transform.tag == "Player"){
					if(hit.distance < 1f){
						//						print ("distance " + hit.distance);
						print ("attack state");
						state = enemy_state.attack;
					}
				}
			}
		}
	}
	
	void Beam(){
		GameObject beam = Instantiate (beam_string) as GameObject;
		beam.transform.position = gameObject.transform.position;
		Attack beam_power = beam.GetComponent<Attack> ();
		beam_power.kirby = false;
		beam_power.go_right = (cur_dir == Direction.right) ? true : false;
		beam_power.poof = true;
	}
	
	void flames(){
		GameObject fire = Instantiate (fireball) as GameObject;
		fire.transform.position = transform.position;
		Attack fire_power = fire.GetComponent<Attack> ();
		fire_power.kirby = false;
		fire_power.go_right = (cur_dir == Direction.right) ? true : false;		
		fire_power.poof = true;
	}
	
	void Spark(){
		GameObject sparks = Instantiate (spark) as GameObject;
		sparks.transform.position = transform.position;
		Attack spark_power = sparks.GetComponent<Attack> ();
		spark_power.kirby = false;
		spark_power.go_right = (cur_dir == Direction.right) ? true : false;		
		spark_power.poof = true;
	}
	
	void OnBecameInvisible(){
		print ("invisible enemy " + gameObject.name);
		gameObject.SetActive (false);
	}
	
	
	
}
