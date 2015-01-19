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
	
	public float speed = 6f;
	public float jump_speed = 10f; 
	public float max_jump_height = 5f; 

	public bool has_enemy = false;
	public power_type power = power_type.none;

	private bool is_floating = false; 
	private Buttons prev_button = Buttons.none; 
	private bool increase_jump = true; 
	private power_type enemy_power = power_type.none;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		PE_Obj my_obj = gameObject.GetComponent<PE_Obj> ();

		// left input
		if (Input.GetKey(KeyCode.LeftArrow))
		{ 
			transform.position += Vector3.left * speed * Time.deltaTime;
			prev_button = Buttons.left;
		}
		// right input
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.position += Vector3.right * speed * Time.deltaTime;
			prev_button = Buttons.right; 
		}
		// up input
		else if (Input.GetKey(KeyCode.UpArrow))
		{
			if (is_floating) {
				increaseFloating(my_obj); 
			} else {
				is_floating = true;
				my_obj.grav = PE_GravType.floating; 
				// put sucking in air to float animation in here
				print ("is_floating is true");
			}
			prev_button = Buttons.up; 
		}
		// down input
		else if (Input.GetKey(KeyCode.DownArrow) && !is_floating)
		{
			//if he has power take the power and get unfat
			if(has_enemy){
				print ("has power" + enemy_power);
				switch(enemy_power){
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
			}
			else{
				// duck!
				print ("duck!");
			}
			prev_button = Buttons.down; 
		}
		// a input
		else if (Input.GetKey(KeyCode.A)) 
		{
			if (is_floating) {
				increaseFloating(my_obj);
			} else {
				//jump
				if (my_obj.reached_ground && prev_button != Buttons.a) {
					increase_jump = true; 
				}
				else if (my_obj.reached_ground == false && prev_button != Buttons.a) {
					increase_jump = false; 
				}
				else if (increase_jump) {
					transform.position += Vector3.up * jump_speed * Time.deltaTime;
					my_obj.reached_ground = false; 
					if (transform.position.y >= max_jump_height) {
						increase_jump = false; 
					}
				}
				print ("jump!");
			}
			prev_button = Buttons.a; 
		}
		// b input 
		else if (Input.GetKey(KeyCode.B)) 
		{
			if (is_floating) {
				// release air
				is_floating = false;
				my_obj.grav = PE_GravType.constant; 
				print ("release air. is_floating is false");
			} else {
				// suck in air
				print ("suck in air");
			}
			prev_button = Buttons.b; 
		}
		// TODO: insert combination input
		else {
			prev_button = Buttons.none;
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
		my_obj.reached_ground = false; 
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
			my_obj.reached_ground = true; 
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
		}
	}

	void OnTriggerExit(Collider col){
		print ("Kirby exiting collider");
		PE_Obj my_obj = gameObject.GetComponent<PE_Obj> ();
		if (col.gameObject.tag == "Ground") {
			my_obj.reached_ground = false;  
		}
	}
}
