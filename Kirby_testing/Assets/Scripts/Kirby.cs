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
	public Animator sprite_kirby;

	public float speed = 6f;
	public float jump_speed = 10f; 
	public float max_jump_height = 5f; 
	public bool reached_ground = false; 

	public bool has_enemy = false;
	public power_type power = power_type.none;
	public bool is_floating = false; 
	public bool near_enemy = false;
	public Buttons previous_direction = 0;

	private PE_Obj my_obj;
	private Buttons prev_button = Buttons.none; 
	private bool increase_jump = true; 
	private power_type enemy_power = power_type.none;

	// Use this for initialization
	void Start () {
		my_obj = GetComponent<PE_Obj> ();
	}
	
	// Update is called once per frame
	void Update () {

		reached_ground = (my_obj.ground != null);

		// left input
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) { 
				sprite_kirby.SetInteger ("Action", 1);
				sprite_kirby.SetInteger ("Action", 3);
				transform.position += Vector3.left * speed * Time.deltaTime;
				prev_button = Buttons.left;
				previous_direction = Buttons.left;
		}
		// right input
		else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
				transform.position += Vector3.right * speed * Time.deltaTime;
				prev_button = Buttons.right; 
				if (is_floating) {
						sprite_kirby.SetInteger ("Action", 12);
				} else if (has_enemy) {
						sprite_kirby.SetInteger ("Action", 16);
				} else {
						sprite_kirby.SetInteger ("Action", 0);
						sprite_kirby.SetInteger ("Action", 2);
				}
				previous_direction = Buttons.right;
		}
		// up input
		else if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {
				if (is_floating) {
						increaseFloating (my_obj); 
//				if(previous_direction == Buttons.right){
						sprite_kirby.SetInteger ("Action", 12);
//				}
				} else {
						is_floating = true;
						my_obj.grav = PE_GravType.floating; 
						// put sucking in air to float animation in here
						print ("is_floating is true");
						if (previous_direction == Buttons.right) {
								sprite_kirby.SetInteger ("Action", 4);
						}
				}
				prev_button = Buttons.up; 
		}
		// down input
		else if ((Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) && !is_floating) {
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
				} else {
						// duck!
						print ("duck!");
						if (previous_direction == Buttons.right) {
								sprite_kirby.SetInteger ("Action", 6);
						}
				}
				prev_button = Buttons.down; 
		}
		// a input
		else if (Input.GetKey (KeyCode.X) || Input.GetKey (KeyCode.Period)) {
				if (is_floating) {
						increaseFloating (my_obj);
						sprite_kirby.SetInteger ("Action", 12);
				} else {
						//jump
						if (reached_ground && prev_button != Buttons.a) {
								increase_jump = true; 
						} else if (reached_ground == false && prev_button != Buttons.a) {
								increase_jump = false; 
						} else if (increase_jump) {
								transform.position += Vector3.up * jump_speed * Time.deltaTime;
								my_obj.ground = null; 
								if (transform.position.y >= max_jump_height) {
										increase_jump = false; 
								}
						}
						print ("jump!");
						if (previous_direction == Buttons.right) {
								sprite_kirby.SetInteger ("Action", 10);
						}

				}
				prev_button = Buttons.a; 
		}
		// b input 
		else if (Input.GetKey (KeyCode.Z) || Input.GetKey (KeyCode.Comma)) {
				if (is_floating) {
						// release air
						is_floating = false;
						my_obj.grav = PE_GravType.constant; 
						print ("release air. is_floating is false");
						sprite_kirby.SetInteger ("Action", 0);
						ReleaseAir();
						
				} else if (has_enemy) {
						sprite_kirby.SetInteger ("Action", 16);
				} else {
						if (near_enemy == false) {
								if (power == power_type.none) {
										// suck in air
										print ("suck in air");
										is_floating = true;
										if (previous_direction == Buttons.right) {
												sprite_kirby.SetInteger ("Action", 14);
										}
								} else {
										has_enemy = false;
										print ("shoot power");
								}
						}
				}
				prev_button = Buttons.b; 
		} 
		//select 
		else if (Input.GetKey (KeyCode.Tab)) {
			if(has_enemy){
				has_enemy = false;
				power = power_type.none;
				print ("release power");
			}
		}
		// TODO: insert combination input
		else {
			prev_button = Buttons.none;
			if(previous_direction == Buttons.right){
				if(!is_floating && !has_enemy){
					sprite_kirby.SetInteger("Action", 0);
				}
			}
			else if(previous_direction == Buttons.left){
				sprite_kirby.SetInteger("Action", 1);
			}
		}

	}

	void increaseFloating(PE_Obj my_obj) {
		if (my_obj.vel.y < 0f) {
			print ("I'm falling and trying to go up");
			//my_obj.acc = Vector3.zero;
			my_obj.vel.y = 0f; 
		}
		transform.position += Vector3.up * speed * Time.deltaTime;
		print ("move up");
		my_obj.ground = null; 
	}

	void OnTriggerEnter(Collider col) {
		// When Kirby collides with something

		print ("Kirby collided");

		PE_Obj my_obj = gameObject.GetComponent<PE_Obj> ();

		if (col.gameObject.name != "Kirby_personal_space") {
			print ("Kirby collided");
		}
		if (col.gameObject.tag == "Ground") {
			my_obj.acc = Vector3.zero; 
			my_obj.vel = Vector3.zero; 
			my_obj.ground = null; 
			print ("Kirby hit ground");
		}
		if (col.gameObject.tag == "Enemy") {
			Enemy_1 enemy = col.gameObject.GetComponent<Enemy_1>();
			if (enemy == null){
				print ("darn");
			}
			if(enemy.power != power_type.none && Input.GetKey(KeyCode.B)){
				print("enemy sucked in");
				enemy_power = enemy.power;
				has_enemy = true;
			}
			print ("ENEMY GONE!");
			col.gameObject.SetActive(false);
			col.gameObject.renderer.enabled = false;
			near_enemy = false;
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
		GameObject puffBall = Instantiate (puffBall_prefab) as GameObject;
		puffBall.transform.position = transform.position;
		//TODO: make the puffball move then disapear
		if (previous_direction == Buttons.right) {

			print ("puffball goes right");
		} 
		else {
			print ("puffbal goes left");
		}
	}
}
