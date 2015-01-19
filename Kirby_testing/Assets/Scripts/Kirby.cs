using UnityEngine;
using System.Collections;

public class Kirby : MonoBehaviour {
	
	public float speed = 5f;
	private bool is_floating; 
	public bool has_enemy;
	public bool inflated;
	public power_type power;
	private power_type enemy_power;

	// Use this for initialization
	void Start () {
		// Destroy (rigidbody);
		is_floating = false;
		has_enemy = false;
		inflated = false;
		power = power_type.none;
		enemy_power = power_type.none;
	}
	
	// Update is called once per frame
	void Update () {

		// float level = 0f; 

		// left input
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		// right input
		if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.position += Vector3.right * speed * Time.deltaTime;
		}
		// up input
		if (Input.GetKey(KeyCode.UpArrow))
		{
			if (is_floating) {
				transform.position += Vector3.up * speed * Time.deltaTime;
				print ("move up");
				PE_Obj my_obj = gameObject.GetComponent<PE_Obj> ();
				my_obj.reached_ground = false; 
			} else {
				inflated = true;
				is_floating = true;
				// put sucking in air to float animation in here
				print ("is_floating is true");
			}
		}
		// down input
		if (Input.GetKey(KeyCode.DownArrow) && !is_floating)
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
		}
		// a input
		if (Input.GetKey(KeyCode.A)) 
		{
			if (is_floating) {
				// move up
				print ("move up");
			} else {
				//jump
				print ("jump!");
			}
		}
		// b input 
		if (Input.GetKey(KeyCode.B)) 
		{
			if (is_floating) {
				// release air
				is_floating = false;
				has_enemy = false;
				inflated = false;
				print ("release air. is_floating is false");
			} else {
				// suck in air
				inflated = true;
//				print ("suck in air");
			}
		}
		// combination input

	}

	void OnTriggerEnter(Collider col) {
		// When Kirby collides with something
		if (col.gameObject.name != "Kirby_personal_space") {
			print ("Kirby collided");
		}
		if (col.gameObject.tag == "Ground") {
			PE_Obj my_obj = gameObject.GetComponent<PE_Obj> ();
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
		PE_Obj my_obj = gameObject.GetComponent<PE_Obj> ();
		my_obj.reached_ground = false;
	}
}
