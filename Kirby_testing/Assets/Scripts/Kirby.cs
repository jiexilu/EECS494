using UnityEngine;
using System.Collections;

public enum Buttons {
	none,
	a, 
	b, 
	left, 
	right, 
	up, 
	down
}

public class Kirby : MonoBehaviour {

	public GameObject puffBall_prefab;
	public GameObject star;
	public Animator sprite_kirby;

	public Vector3 vel;

	public float speed = 3f;
	public float air_speed = 4f; 
	public float float_speed = 3f; 
	public float max_jump_speed = 8f; 
	public bool reached_ground = false; 

	public bool has_enemy = false;
	public power_type power = power_type.none;
	public bool is_floating = false; 
	public bool near_enemy = false;
	public Buttons previous_direction = 0;
	public float life = 4f;
	public int health = 6;

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

		// you can only increase jump when you've reached ground AND you've released the up key 
		//Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Period)
		if (reached_ground) {
			increase_jump = true; 
		}
		
		// Horizontal movement
		float vX = Input.GetAxis("Horizontal"); // Returns a number [-1..1]
		if (vel.y != 0) {
			vel.x = vX * air_speed;
		} else {
			vel.x = vX * speed;
		}

		// left input
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) { 
			sprite_kirby.SetInteger ("Action", 3);
			previous_direction = Buttons.left;
			if (is_floating) {
				sprite_kirby.SetInteger ("Action", 13);
			}
			else if(has_enemy){
				sprite_kirby.SetInteger ("Action", 21);
			}
		}
		// right input
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			if (is_floating) {
				sprite_kirby.SetInteger ("Action", 12);
			} else if (has_enemy) {
				sprite_kirby.SetInteger ("Action", 16);
			} else {
				sprite_kirby.SetInteger ("Action", 2);
			}
			previous_direction = Buttons.right;
		}
		// up input
		// Float with up
		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W) 
		    || ((Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.Period)) && is_floating)) {
			if (is_floating) {
				increaseFloating (my_obj);
				if (previous_direction != Buttons.left) {
					sprite_kirby.SetInteger ("Action", 12);
					previous_direction = Buttons.right;	
				} else {
					sprite_kirby.SetInteger ("Action", 13);
					previous_direction = Buttons.left;	
				}
			} else if (!has_enemy) {
				is_floating = true;
				my_obj.grav = PE_GravType.none; 
				// put sucking in air to float animation in here
				print ("is_floating is true");
				if (previous_direction != Buttons.left) {
					sprite_kirby.SetInteger ("Action", 4);
					previous_direction = Buttons.right;	
				} else {
					sprite_kirby.SetInteger ("Action", 4);
					previous_direction = Buttons.left;	
				}
			}
		} else {
			decrease_floating();
		}

		// down input
		if ((Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S)) && !is_floating) {
			//if he has power take the power and get unfat
			if (has_enemy) {
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
			} else {
				// duck!
				print ("duck!");
				if (previous_direction == Buttons.right) {
						sprite_kirby.SetInteger ("Action", 6);
				}
			}
		}
		// Jumping with A (which is x or .)
		if ((Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.Period)) && !is_floating) {
			if (transform.position.y < 3.5f && increase_jump) { // Jump if you're grounded
				print ("jumping");
				vel.y = max_jump_speed;
				my_obj.grav = PE_GravType.none;
				my_obj.ground = null; // Jumping will set ground = null
				if (transform.position.y >= 3.4f) {
					increase_jump = false; 
				}
				if (previous_direction == Buttons.right) {
					sprite_kirby.SetInteger ("Action", 10);
				}
			} else {
				decrease_jump();
			}
		} else if (!is_floating) { // only decrease if you've already jumped 
			decrease_jump();
		}

		// b input 
		if (Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.Comma)) {
			if (is_floating) {
				// release air
				is_floating = false;
				my_obj.grav = PE_GravType.constant; 
				print ("release air. is_floating is false");
				sprite_kirby.SetInteger ("Action", 18);
				ReleaseAir ();
			} else if (has_enemy) {
					//release the enemy with a star
				sprite_kirby.SetInteger ("Action", 0);
				ReleaseAir ();
				has_enemy = false;
			} else if (power != power_type.none) {
				Attack();
			} 
		} 
		if (Input.GetKey (KeyCode.Z) || Input.GetKey (KeyCode.Comma)) {
			if (power == power_type.none) {
				if (previous_direction != Buttons.left) {
						sprite_kirby.SetInteger ("Action", 14);
				} 
			}
			else if(has_enemy){
				sprite_kirby.SetInteger ("Action", 20);
			}
		}
		//select	
		if (Input.GetKey (KeyCode.Tab)) {
			if(power != power_type.none){
				has_enemy = false;
				power = power_type.none;
				print ("release power");
			}
		}
		// TODO: insert combination input
		if (vel == Vector3.zero) {
			if(previous_direction == Buttons.right){
				if(!is_floating && !has_enemy){
					sprite_kirby.SetInteger("Action", 0);
				}
				if(has_enemy){
					sprite_kirby.SetInteger("Action", 20);
				}
			}
			else if(previous_direction == Buttons.left){
				if(!is_floating && !has_enemy){
					sprite_kirby.SetInteger("Action", 1);
				}
				if(has_enemy){
					sprite_kirby.SetInteger("Action", 20);
				}
			}
		}

		my_obj.vel = vel;
	}

	void increaseFloating(PE_Obj my_obj) {
		vel.y = float_speed;
		print ("move up");
		my_obj.ground = null; 
	}
	
	void decrease_floating() {
		if (is_floating) {
			vel.y = -1.5f; 
		}
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
			if(enemy.power != power_type.none && (Input.GetKey (KeyCode.Z) || Input.GetKey (KeyCode.Comma))){
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

	void ReleaseAir(){
		GameObject projectile;
		if (has_enemy) {
			projectile = Instantiate (star) as GameObject;
		} 
		else {
			projectile = Instantiate (puffBall_prefab) as GameObject;
		}
		projectile.transform.position = transform.position;

		//TODO: make the puffball move then disapear
		Attack puff = projectile.GetComponent<Attack>();
		if (previous_direction == Buttons.right) {
			print ("puffball goes right");
			puff.go_right = true;
		} 
		else {
			print ("puffball goes left");
			puff.go_right = false;
		}
		puff.poof= true;
	}

	void Attack(){
		print ("Kirby attacks with power!");
		switch (power) {
			case power_type.none: 
				print ("NOTHING");
				break;
			case power_type.beam:
				print ("BEAM");
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
