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
	public float speed = 1f;
	private float distance;
	public int score = 400; //points earned for destroying
	
	private float usage;
	private float delay;
	private float delay_until_attack = 3f; 
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
	public bool attack = true;
	public float dist_until_chase = 4f;
	
	//powers
	public GameObject beam_string;
	public GameObject fireball;
	public GameObject spark;

	public bool hit_wall = false;
	public bool hit_cube = false; 
	public bool hit_water = false;

	public Transform BL, BR;


	void Awake () {
		BL = transform.Find ("BL");
		BR = transform.Find ("BR");
	}

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
		if (name == "BrontoBurt") {
			my_obj.grav = PE_GravType.none;
			vel.x = -speed*2;
			vel.y = speed*2;
			my_obj.vel = vel; 
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Basic movement to the right
		vel = my_obj.vel; 
		//		-------
		//if they bump something, change directions or jump
		//rayDir = kirby.position - transform.position;

		if (hit_wall || hit_water) {
			gameObject.SetActive(false); 
		}

		if (name == "BrontoBurt") {
			BrontoBurt_mvmt();
			my_obj.vel = vel;
			return;
		}
		
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
		if (col.gameObject.tag == "Enemy") {
			Physics.IgnoreCollision(gameObject.collider, col);
		}
	}
	
	void Attack(){
		vel = Vector3.zero;
		if (!set_delay) {
			usage = Time.time + delay;		
			set_delay = true;
		}
		if (Time.time < usage && attack) {
			print ("stop moving");
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
			attack = false;
		}
		//set timer
		if (Time.time > usage) {
			state = enemy_state.wander;
			set_delay = false;
		}
	}

	void BrontoBurt_mvmt() {
		Camera_follow cam = Camera.main.GetComponent<Camera_follow> ();
		float max = cam.transform.position.y + 2.5f; 
		float min = cam.transform.position.y;
		if (transform.position.y > (max - 0.1f)) {
			vel.y = -speed*2;
		} else if (transform.position.y < (min + 0.1f)) {
			vel.y = speed*2; 
		}
	}
	
	void state_wander(){
		//print ("wandering enemy");
		if (!set_delay && !attack) {
			usage = Time.time + delay_until_attack;		
			set_delay = true;
		} 
		if (Time.time > usage) {
			attack = true; 
			set_delay = false; 
		}

		if (vel == Vector3.zero) {
			vel.x = -speed;
		}

		if (hit_cube) {
			vel.x = -vel.x;	
			hit_cube = false; 
		}

		if (Mathf.Abs(Vector3.Distance(transform.position, kirby.position)) < dist_until_chase 
		    && attack) {
			state = enemy_state.chase;
		}

	}

	void state_chase(){
		//print ("chasing kirby");

		float dist_to_kirby = Vector3.Distance (transform.position, kirby.position);

		if (Mathf.Abs(dist_to_kirby) < dist_until_chase && transform.position.x > kirby.position.x) {
			//print ("Kirby to left");
			vel.x = -speed;
		} else if (Mathf.Abs(dist_to_kirby) < dist_until_chase && transform.position.x < kirby.position.x) {
			//print ("Kirby to right");
			vel.x = speed;
		} else {
			//print ("back to wander");
			state = enemy_state.wander;
		}

		if (Mathf.Abs(dist_to_kirby) < 2f) {
			state = enemy_state.attack;
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
