using UnityEngine;
using System.Collections;

public enum Direction {
	left, 
	right
}

public enum State {
	stand, 
	jump, 
	inhale, 
	floating, 
	duck, 
	slide, 
	shoot, 
	suck, 
	stand_water, 
	stand_enemy, 
	stand_power,
	use_power
}


public class Kirby : MonoBehaviour {
	
	public bool God_mode = false;
	public GameObject kirby_spawner;
	public GameObject musicNotes;
	public GameObject musicBubble;
	public GameObject puffBall_prefab;
	public GameObject star;
	public GameObject beam;
	public GameObject spark;
	public GameObject fireball;
	public Animator sprite_kirby;
	public Camera_follow cam;
	public BoxCollider box;  
	
	public Vector3 vel;
	
	public float speed = 3f;
	public float slide_speed = 10f; 
	public float air_speed = 4f; 
	public float float_speed = 3f; 
	public float max_jump_speed = 8f; 
	public bool reached_ground = false; 
	public float usage;
	public float attack_usage;
	public float delay;
	public float slide_delay = 0.5f;
	public float attack_delay = 5f;
	public bool set_delay = false; 
	public bool set_attack_delay = false;
	public bool set_paralyzed_delay = false;
	public float slide_x = 0.5f;
	public float water_speed = 2f;
	public float swim_up_speed = 2f;
	public bool under_water = false; 
	
	public bool has_enemy = false;
	public power_type power = power_type.none;
	public bool is_floating = false; 
	public bool near_enemy = false;
	public Direction cur_dir = Direction.right;
	public float life = 4f;
	public int health = 6;
	
	public State prev_state = State.stand;
	public State cur_state = State.stand; 
	public State next_state = State.stand;
	public State cur_stand = State.stand;
	
	private PE_Obj my_obj;
	private bool increase_jump = true; 
	private power_type enemy_power = power_type.none;
	private Vector3 puffed_box_size = new Vector3(1.75f, 1.25f, 1.75f);
	private Vector3 standing_box_size = new Vector3(1f, 1f, 1f);
	private Vector3 duck_box_size = new Vector3(1f, 0.5f, 1f);
	private Vector3 slide_box_size = new Vector3(2f, 1f, 1f);
	
	public Transform BL, BR;
	
	void Awake () {
		BL = transform.Find ("BL");
		BR = transform.Find ("BR");
		box = GetComponent<BoxCollider> ();
	}
	
	// Use this for initialization
	void Start () {
		my_obj = GetComponent<PE_Obj> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		vel = my_obj.vel;
		if (under_water && !my_obj.is_under_water) {
			print ("curstate inhale");
			cur_state = State.inhale;
		} else if (my_obj.is_under_water) {
			print ("curstate stand water");
			cur_state = State.stand_water;
		}
		under_water = my_obj.is_under_water; 
		
		//print (vel);
		reached_ground = (my_obj.ground != null);
		next_state = cur_state; 
		
		// you can only increase jump when you've reached ground AND you've released the up key 
		//Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Period)
		//		if (reached_ground) {
		//			increase_jump = true; 
		//		}
		
		// input + cur_state = next_state; 
		switch (cur_state) {
		case State.stand:
			state_stand();
			break; 
		case State.jump: 
			state_jump();
			break; 
		case State.inhale:
			state_inhale();
			break; 
		case State.floating:
			state_floating();
			break; 
		case State.duck: 
			state_duck();
			break; 
		case State.slide:
			state_slide();
			break; 
		case State.shoot:
			state_shoot();
			break; 
		case State.suck:
			state_suck();
			break; 
		case State.stand_enemy:
			state_stand_enemy();
			break; 
		case State.stand_power:
			state_stand_power();
			break; 
		case State.use_power:
			state_use_power();
			break;
		case State.stand_water:
			state_under_water();
			break;
		}
		
		prev_state = cur_state; 
		cur_state = next_state; 
		my_obj.vel = vel;
	}
	
	void state_stand() {
		horiz_movement();
		cur_stand = State.stand;
		if (cur_dir == Direction.right) {
			sprite_kirby.SetInteger("Action", 0);
		} else {
			sprite_kirby.SetInteger("Action", 1);
		}
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			// left input
			cur_dir = Direction.left;
			sprite_kirby.SetInteger ("Action", 3);
		} 
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			// right input
			sprite_kirby.SetInteger ("Action", 2);
			cur_dir = Direction.right;
		} 
		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {
			// up input
			next_state = State.inhale; 
		} 
		if (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S)) {
			// down input
			box.size = duck_box_size;
			next_state = State.duck;
		} 
		if ((Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.Period)) && increase_jump) {
			// a input
			next_state = State.jump; 
		} 
		if (Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.Comma)) {
			// b input
			next_state = State.suck;
		}
	}
	
	void state_jump() {
		horiz_movement();
		if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.Period)) {
			if (transform.position.y < 3.5f && increase_jump) {
				vel.y = max_jump_speed;
				my_obj.grav = PE_GravType.none;
				my_obj.ground = null; // Jumping will set ground = null
				if (transform.position.y >= 3.4f) {
					increase_jump = false; 
				}
				if (cur_dir == Direction.right) {
					sprite_kirby.SetInteger ("Action", 10);
				}
				else{
					sprite_kirby.SetInteger("Action", 11);
				}
			} else {
				decrease_jump();
			}
		} else {
			decrease_jump();
		}
		if (reached_ground) {
			increase_jump = true; 
			next_state = cur_stand;
		}
	}
	
	void state_inhale() {
		// Do a little hop
		my_obj.grav = PE_GravType.none; 
		if (cur_dir != Direction.left) {
			sprite_kirby.SetInteger ("Action", 4);
			cur_dir = Direction.right;	
		} else {
			sprite_kirby.SetInteger ("Action", 4);
			cur_dir = Direction.left;
		}
		change_height (0.75f);
		box.size = puffed_box_size;
		next_state = State.floating; 
	}
	
	void state_floating() {
		is_floating = true; 
		horiz_movement();
		// left input
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			cur_dir = Direction.left;
			sprite_kirby.SetInteger ("Action", 13);
		} 
		// right input
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			sprite_kirby.SetInteger ("Action", 12);
			cur_dir = Direction.right;
		}
		// up and a input
		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W) 
		    || Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.Period)) {
			
			increase_floating(my_obj);
			
			if (cur_dir != Direction.left) {
				sprite_kirby.SetInteger ("Action", 12);
				cur_dir = Direction.right;	
			} else {
				sprite_kirby.SetInteger ("Action", 13);
				cur_dir = Direction.left;	
			}
		} else {
			decrease_floating();
		}
		// b input deflate
		if (Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.Comma)) {
			my_obj.grav = PE_GravType.constant; 
			print ("release air. is_floating is false");
			is_floating = false; 
			next_state = State.shoot;
		}
	}
	
	void state_duck() {
		// duck!
		//TODO: Decrease Scale
		print ("duck!");
		if (cur_dir == Direction.right) {
			sprite_kirby.SetInteger ("Action", 6);
		} else if(cur_dir == Direction.left) {
			sprite_kirby.SetInteger ("Action", 7);
		}
		// b input
		if (Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.Comma)) {
			// slide
			if(cur_dir == Direction.right){
				sprite_kirby.SetInteger("Action", 22);
			}
			else{
				sprite_kirby.SetInteger("Action", 23);
			}
			next_state = State.slide; 
		} else if ((Input.GetKeyUp (KeyCode.DownArrow) || Input.GetKeyUp (KeyCode.S))) {
			// go back to previous standing
			change_height (0.25f);
			box.size = standing_box_size;
			next_state = cur_stand; 
		}
	}
	
	void state_slide() {
		if (!set_delay) {
			change_height (0.25f);
			box.size = slide_box_size;
			usage = Time.time + slide_delay;
			set_delay = true; 
		}
		if (Time.time < usage) {
			// increase velocity in the direction you are facing 
			// keep increasing until you've reached a certain time
			float vX = (cur_dir == Direction.right) ? slide_x : -slide_x;
			vel.x = vX * slide_speed;
		} else {
			set_delay = false;
			if (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S)) {
				next_state = State.duck;
			} else {
				// go back to previous standing
				change_height (0.25f);
				box.size = standing_box_size;
				next_state = cur_stand; 
			}
		}
	}
	
	void state_shoot() {
		if (prev_state == State.floating) {
			sprite_kirby.SetInteger ("Action", 18);
			GameObject projectile = Instantiate (puffBall_prefab) as GameObject;
			projectile.transform.position = transform.position;
			Attack puff = projectile.GetComponent<Attack>();
			puff.kirby = true;
			if (puff == null) {
				print ("Puff is null");
			}
			if (cur_dir == Direction.right) {
				print ("puffball goes right");
				puff.go_right = true;
			} 
			else {
				print ("puffball goes left");
				puff.go_right = false;
			}
			puff.poof= true;
			next_state = cur_stand;
			
		} else if (prev_state == State.stand_enemy) {
			GameObject projectile = Instantiate (star) as GameObject;
			has_enemy = false; 
			sprite_kirby.SetInteger ("Action", 18);
			projectile.transform.position = transform.position;
			Attack starfire = projectile.GetComponent<Attack>();
			starfire.kirby = true;
			if (starfire == null) {
				print ("Puff is null");
			}
			if (cur_dir == Direction.right) {
				print ("puffball goes right");
				starfire.go_right = true;
			} 
			else {
				print ("puffball goes left");
				starfire.go_right = false;
			}
			starfire.poof= true;
			next_state = State.stand;
		}
		box.size = standing_box_size;
	}
	
	void state_suck() {
		if (cur_dir == Direction.right) {
			sprite_kirby.SetInteger ("Action", 14);
		}
		else if (cur_dir == Direction.left) {
			sprite_kirby.SetInteger ("Action", 15);
		}
		// personal space stuff goes here
		if (has_enemy) { // If the sucking motion I just did got an enemy
			change_height (0.75f);
			box.size = puffed_box_size;
			next_state = State.stand_enemy;
		}
		else if (!near_enemy && (Input.GetKeyUp (KeyCode.Z) || Input.GetKeyUp (KeyCode.Comma))) {
			next_state = State.stand;		
		}
	}
	
	void state_stand_enemy() {
		cur_stand = State.stand_enemy;
		horiz_movement();
		if (cur_dir == Direction.right) {
			sprite_kirby.SetInteger ("Action", 20);
		}
		else if (cur_dir == Direction.left) {
			sprite_kirby.SetInteger ("Action", 21);
		}
		// left input
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			cur_dir = Direction.left;
			sprite_kirby.SetInteger ("Action", 17);
		} 
		// right input
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			cur_dir = Direction.right;
			sprite_kirby.SetInteger ("Action", 16);
		} 
		// down input
		if (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S)) {
			print ("has power" + enemy_power);
			switch (enemy_power) {
			case power_type.none: 
				print ("NOTHING");
				power = power_type.none;
				break;
			case power_type.beam:
				print ("BEAM");
				power = power_type.beam;
				break;
			case power_type.fire:
				power = power_type.fire;
				print ("FIRE");
				break;
			case power_type.spark:
				power = power_type.spark;
				print ("SPARK");
				break;
			default:
				print ("crap");
				break;
			}
			enemy_power = power_type.none;
			has_enemy = false;
			sprite_kirby.SetInteger ("Action", 0);
			box.size = standing_box_size;
			if(power != power_type.none){
				next_state = State.stand_power; 
			}
			else{
				next_state = State.stand;
			}
		} 
		if (Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.Comma)) {
			next_state = State.shoot;
		}
		//TODO: kirby can jump when he has an enemy
		if (Input.GetKey (KeyCode.X) || Input.GetKey (KeyCode.Period)) {
			next_state = State.jump;		
		}
		//cur_stand = State.stand_power;
	}
	
	void state_stand_power() {
		horiz_movement();
		cur_stand = State.stand_power;
		if (cur_dir == Direction.right) {
			sprite_kirby.SetInteger("Action", 0);
		} else {
			sprite_kirby.SetInteger("Action", 1);
		}
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			// left input
			cur_dir = Direction.left;
			sprite_kirby.SetInteger ("Action", 3);
		} 
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			// right input
			sprite_kirby.SetInteger ("Action", 2);
			cur_dir = Direction.right;
		} 
		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {
			// up input
			next_state = State.inhale; 
		} 
		if (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S)) {
			// down input
			next_state = State.duck;
		} 
		if ((Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.Period)) && increase_jump) {
			// a input
			next_state = State.jump; 
		} 
		if (Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.Comma)) {
			// b input
			next_state = State.use_power;
		}
		// select
		if (Input.GetKeyDown (KeyCode.Tab) && power != power_type.none) {
			power = power_type.none;
			next_state = State.stand;
			print ("release power");
		}
	}
	
	void state_use_power() {
		if (!set_attack_delay) {
			vel.x = 0;
			attack_usage = Time.time + attack_delay;		
			print ("Kirby attacks with power!");
			set_attack_delay = true;
			switch (power) {
			case power_type.none: 
				print ("NOTHING");
				break;
			case power_type.beam:
				print ("BEAM");
				GameObject projectile = Instantiate (beam) as GameObject;
				projectile.transform.position = transform.position;
				Attack beam_power = projectile.GetComponent<Attack> ();
				beam_power.kirby = true;
				beam_power.go_right = (cur_dir == Direction.right) ? true : false;
				beam_power.poof = true;
				break;
			case power_type.fire:
				print ("FIRE");
				GameObject fire = Instantiate (fireball) as GameObject;
				fire.transform.position = transform.position;
				Attack fire_power = fire.GetComponent<Attack> ();
				fire_power.kirby = true;
				fire_power.go_right = (cur_dir == Direction.right) ? true : false;		
				fire_power.poof = true;
				break;
			case power_type.spark:
				sprite_kirby.SetInteger("Action", 24);	
				GameObject sparks = Instantiate (spark) as GameObject;
				sparks.transform.position = transform.position;
				Attack spark_power = sparks.GetComponent<Attack> ();
				spark_power.kirby = true;
				spark_power.go_right = (cur_dir == Direction.right) ? true : false;		
				spark_power.poof = true;
				print ("SPARK");
				break;
			case power_type.sing:
				print ("SING!");
				if (cur_dir == Direction.right) {
					sprite_kirby.SetInteger ("Action", 14);
				} else if (cur_dir == Direction.left) {
					sprite_kirby.SetInteger ("Action", 15);
				}
				GameObject notes = Instantiate (musicNotes) as GameObject;
				notes.transform.position = transform.position;
				Attack sing_power = notes.GetComponent<Attack> ();	
				sing_power.kirby = true;
				sing_power.go_right = (cur_dir == Direction.right) ? true : false;
				sing_power.poof = true;
				cam.music_power = true;
				GameObject bubble = Instantiate(musicBubble) as GameObject;
				bubble.transform.position = transform.position;
				break;
			}
		}
		//TODO: while attacking, don't change states
		if (Time.time > attack_usage) {
			cam.music_power = false;
			next_state = State.stand_power;
			set_attack_delay = false;
		}
	}
	
	void state_under_water() {
		my_obj.grav = PE_GravType.none;
		horiz_movement();
		if (cur_dir == Direction.right) {
			sprite_kirby.SetInteger("Action", 0);
		} else {
			sprite_kirby.SetInteger("Action", 1);
		}
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			// left input
			cur_dir = Direction.left;
			sprite_kirby.SetInteger ("Action", 3);
		} 
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			// right input
			sprite_kirby.SetInteger ("Action", 2);
			cur_dir = Direction.right;
		} 
		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)
		    || Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.Period)) {
			// swim up
			vel.y = swim_up_speed;
		} else {
			decrease_floating();
		}
		if (Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.Comma)) {
			// spit out water
		}
	}
	
	// Horizontal movement
	void horiz_movement() {
		float vX = Input.GetAxis("Horizontal"); // Returns a number [-1..1]
		if (under_water) {
			vel.x = vX * water_speed; 
		} else if (vel.y != 0) {
			vel.x = vX * air_speed;
		} else {
			vel.x = vX * speed;
		}
	}
	
	void increase_floating(PE_Obj my_obj) {
		vel.y = float_speed;
		my_obj.ground = null; 
	}
	
	void decrease_floating() {
		vel.y = -1.5f; 
	}
	
	void decrease_jump() {
		increase_jump = false; 
		if (vel.y > 0) {
			vel.y = 0;
		}
		my_obj.grav = PE_GravType.constant;
		print ("falling with constant acc");
	}
	
	void change_height(float increase) {
		Vector3 temp = transform.position;
		temp.y += increase; 
		transform.position = temp;
	}
	
	void float_in_pool(Collider col) {
		float col_y_lossy = col.transform.lossyScale.y / 2f; 
		float col_y_up = col.transform.position.y + col_y_lossy;
		float y_lossy = (transform.lossyScale.y / 2f) * box.size.y; 
		float y_down = transform.position.y - y_lossy;
		float dif_y = 0f;
		//get new position 
		dif_y = Mathf.Abs (y_down - col_y_up);
		//set new position 
		change_height(dif_y);
		//set velocity to zero
		vel.y = 0f;
	}
	
	void OnTriggerEnter(Collider col) {
		// When Kirby collides with something
		if (col.gameObject.tag == "Enemy") {
			print ("Kirby collided");
			Enemy_1 enemy = col.gameObject.GetComponent<Enemy_1>();
			if (enemy == null){
				print ("darn");
			}
			if(cur_state == State.suck){
				print("enemy sucked in");
				enemy_power = enemy.power;
				has_enemy = true;
				print ("ENEMY GONE!");
			}
			else if (cur_state != State.slide) {
				if(!set_paralyzed_delay){
					usage = Time.time + 40f;
					set_paralyzed_delay = true;
				}
				//gets paralyze for a bit in this phase
				print ("kirby hurt");
				if(Time.time < usage){
					power = power_type.ouch;//but for a short time
				} //TODO: add else somehow
				power = power_type.none;
				Got_Attacked(); 
				//TODO: i want to call got_attaked in enemy_1 script
			}
			if(col.gameObject.tag == "boss"){
				Got_Attacked();
			}
			col.gameObject.SetActive(false);
			near_enemy = false;
		}
	}
	
	void OnTriggerExit(Collider col){
		print ("Kirby exiting collider");
		my_obj = gameObject.GetComponent<PE_Obj> ();
		if (col.gameObject.tag == "Ground") {
			my_obj.ground = null;  
		}
	}
	
	
	public void Got_Attacked(){
		GameObject go = GameObject.Find("godMode");
		godMode god = go.GetComponent<godMode>();
		if (!god.god_mode) {
			health--;
			if (health == 0 && life > 1) {
				life--;
				health = 6;
				Vector3 temp = kirby_spawner.transform.position;
				temp.z = 0;
				transform.position = temp;
				next_state = State.stand;
			}
			if (life == 0 && health == 0) {
				print ("Game over");
				gameObject.SetActive (false);
			}
		}
	}
	
	void FixedUpdate(){
		//just need this in place for raycasting
	}
}
