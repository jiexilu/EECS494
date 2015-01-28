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
	stand_enemy, 
	stand_power,
	use_power
}


public class Kirby : MonoBehaviour {

	public GameObject puffBall_prefab;
	public GameObject star;
	public GameObject beam;
	public Animator sprite_kirby;

	public Vector3 vel;

	public float speed = 3f;
	public float air_speed = 4f; 
	public float float_speed = 3f; 
	public float max_jump_speed = 8f; 
	public bool reached_ground = false; 

	public bool has_enemy = false;
	public power_type power = power_type.none;
	//public bool is_floating = false; 
	public bool near_enemy = false;
	public Direction prev_dir = Direction.right;
	public float life = 4f;
	public int health = 6;

	public State prev_state = State.stand;
	public State cur_state = State.stand; 
	public State next_state = State.stand;
	public State cur_stand = State.stand;

	private PE_Obj my_obj;
	private bool increase_jump = true; 
	private power_type enemy_power = power_type.none;

	// Use this for initialization
	void Start () {
		my_obj = GetComponent<PE_Obj> ();
	}
	
	// Update is called once per frame
	void Update () {

		vel = my_obj.vel;
		
		//print (vel);
		reached_ground = (my_obj.ground != null);
		next_state = cur_state; 

		// you can only increase jump when you've reached ground AND you've released the up key 
		//Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Period)
		if (reached_ground) {
			increase_jump = true; 
		}

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
		}

		prev_state = cur_state; 
		cur_state = next_state; 
		my_obj.vel = vel;
	}

	void state_stand() {
		horiz_movement();
		cur_stand = State.stand;
		if (prev_dir == Direction.right) {
			sprite_kirby.SetInteger("Action", 0);
		} else {
			sprite_kirby.SetInteger("Action", 1);
		}
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			// left input
			prev_dir = Direction.left;
			sprite_kirby.SetInteger ("Action", 3);
		} 
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			// right input
			sprite_kirby.SetInteger ("Action", 2);
			prev_dir = Direction.right;
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
				if (prev_dir == Direction.right) {
					sprite_kirby.SetInteger ("Action", 10);
				}
			} else {
				decrease_jump();
			}
		} else {
			decrease_jump();
		}
		if (reached_ground) {
			increase_jump = true; 
			next_state = (cur_stand == State.stand) ? State.stand : State.stand_power;
		}
	}

	void state_inhale() {
		// Do a little hop
		my_obj.grav = PE_GravType.none; 
		print ("is_floating is true");
		if (prev_dir != Direction.left) {
			sprite_kirby.SetInteger ("Action", 4);
			prev_dir = Direction.right;	
		} else {
			sprite_kirby.SetInteger ("Action", 4);
			prev_dir = Direction.left;
		}
		next_state = State.floating; 
	}

	void state_floating() {
		horiz_movement();
		// left input
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			prev_dir = Direction.left;
			sprite_kirby.SetInteger ("Action", 13);
		} 
		// right input
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			sprite_kirby.SetInteger ("Action", 12);
			prev_dir = Direction.right;
		}
		// up and a input
		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W) 
			|| Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.Period)) {
			
			increase_floating(my_obj);
			
			if (prev_dir != Direction.left) {
				sprite_kirby.SetInteger ("Action", 12);
				prev_dir = Direction.right;	
			} else {
				sprite_kirby.SetInteger ("Action", 13);
				prev_dir = Direction.left;	
			}
		} else {
			decrease_floating();
		}
		// b input deflate
		if (Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.Comma)) {
			my_obj.grav = PE_GravType.constant; 
			print ("release air. is_floating is false");
			next_state = State.shoot;
		}
	}

	void state_duck() {
		// duck!
		//TODO: Decrease Scale
		print ("duck!");
		if (prev_dir == Direction.right) {
			sprite_kirby.SetInteger ("Action", 6);
		}
		// b input
		if (Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.Comma)) {
			// slide
			next_state = State.slide; 
		} else if ((Input.GetKeyUp (KeyCode.DownArrow) || Input.GetKeyUp (KeyCode.S))) {
			// go back to previous standing
			next_state = (cur_stand == State.stand ) ? State.stand : State.stand_power; 
		}
	}

	void state_slide() {
		if (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S)) {
			next_state = State.duck;
		} else {
			// go back to previous standing
			next_state = (cur_state == State.stand) ? State.stand : State.stand_power; 
		}
	}

	void state_shoot() {
		if (prev_state == State.floating) {
			sprite_kirby.SetInteger ("Action", 18);
			GameObject projectile = Instantiate (puffBall_prefab) as GameObject;
			projectile.transform.position = transform.position;
			Attack puff = projectile.GetComponent<Attack>();
			if (puff == null) {
				print ("Puff is null");
			}
			if (prev_dir == Direction.right) {
				print ("puffball goes right");
				puff.go_right = true;
			} 
			else {
				print ("puffball goes left");
				puff.go_right = false;
			}
			puff.poof= true;
			next_state = (cur_stand == State.stand) ? State.stand : State.stand_power;

		} else if (prev_state == State.stand_enemy) {
			GameObject projectile = Instantiate (star) as GameObject;
			has_enemy = false; 
			sprite_kirby.SetInteger ("Action", 18);
			projectile.transform.position = transform.position;
			Attack starfire = projectile.GetComponent<Attack>();
			if (starfire == null) {
				print ("Puff is null");
			}
			if (prev_dir == Direction.right) {
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
	}

	void state_suck() {
		if (prev_dir == Direction.right) {
			sprite_kirby.SetInteger ("Action", 14);
		}
		else if (prev_dir == Direction.left) {
			sprite_kirby.SetInteger ("Action", 15);
		}
		// personal space stuff goes here
		if (has_enemy) { // If the sucking motion I just did got an enemy
			next_state = State.stand_enemy;
		}
		else if (!near_enemy && (Input.GetKeyUp (KeyCode.Z) || Input.GetKeyUp (KeyCode.Comma))) {
			next_state = State.stand;		
		}
	}

	void state_stand_enemy() {
		horiz_movement();
		if (prev_dir == Direction.right) {
			sprite_kirby.SetInteger ("Action", 20);
		}
		else if (prev_dir == Direction.left) {
			sprite_kirby.SetInteger ("Action", 21);
		}
		// left input
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			prev_dir = Direction.left;
			sprite_kirby.SetInteger ("Action", 21);
		} 
		// right input
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			prev_dir = Direction.right;
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
			next_state = State.stand_power; 
		} 
		if (Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.Comma)) {
			next_state = State.shoot;
		}
		cur_stand = State.stand_power;
	}

	void state_stand_power() {
		horiz_movement();
		cur_stand = State.stand_power;
		if (prev_dir == Direction.right) {
			sprite_kirby.SetInteger("Action", 0);
		} else {
			sprite_kirby.SetInteger("Action", 1);
		}
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			// left input
			prev_dir = Direction.left;
			sprite_kirby.SetInteger ("Action", 3);
		} 
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			// right input
			sprite_kirby.SetInteger ("Action", 2);
			prev_dir = Direction.right;
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
		if (Input.GetKeyDown (KeyCode.Tab)) {
			power = power_type.none;
			next_state = State.stand;
			print ("release power");
		}
	}

	void state_use_power() {
		print ("Kirby attacks with power!");
		switch (power) {
			case power_type.none: 
				print ("NOTHING");
				break;
			case power_type.beam:
				print ("BEAM");
				GameObject projectile = Instantiate (beam) as GameObject;
				beam.transform.position = transform.position;
				Attack beam_power = beam.GetComponent<Attack>();
				beam_power.go_right = (prev_dir == Direction.right) ? true : false;
				beam_power.poof = true;
				break;
			case power_type.fire:
				print ("FIRE");
				break;
			case power_type.spark:
				print ("SPARK");
				break;
			default:
				print ("crap");
				break;
		}
		next_state = State.stand_power;
	}

	// Horizontal movement
	void horiz_movement() {
		float vX = Input.GetAxis("Horizontal"); // Returns a number [-1..1]
		if (vel.y != 0) {
			vel.x = vX * air_speed;
		} else {
			vel.x = vX * speed;
		}
	}

	void increase_floating(PE_Obj my_obj) {
		vel.y = float_speed;
		print ("move up");
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

	void OnTriggerEnter(Collider col) {
		// When Kirby collides with something

		print ("Kirby collided");

		PE_Obj my_obj = gameObject.GetComponent<PE_Obj> ();

		if (col.gameObject.name != "Kirby_personal_space") {
			print ("Kirby collided");
		}
		if (col.gameObject.tag == "Enemy") {
			Enemy_1 enemy = col.gameObject.GetComponent<Enemy_1>();
			if (enemy == null){
				print ("darn");
			}
			if(cur_state == State.suck){
				print("enemy sucked in");
				enemy_power = enemy.power;
				has_enemy = true;
				print ("ENEMY GONE!");
				col.gameObject.SetActive(false);
				near_enemy = false;
			}
			else{
				print ("kirby hurt");
				health--;
				col.gameObject.SetActive(false);
				near_enemy = false;
			}

		}
	}

	void OnTriggerExit(Collider col){
		print ("Kirby exiting collider");
		PE_Obj my_obj = gameObject.GetComponent<PE_Obj> ();
		if (col.gameObject.tag == "Ground") {
			my_obj.ground = null;  
		}
	}


	public void Got_Attacked(){
		health--;
		if (health == 0 && life > 1) {
			life--;
			health = 6;
		}
		if (life == 0 && health == 0) {
			print ("Game over");
			gameObject.SetActive(false);
		}
	}

}
