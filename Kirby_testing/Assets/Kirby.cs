using UnityEngine;
using System.Collections;

public class Kirby : MonoBehaviour {
	
	public float speed = 5f;
	private bool is_floating; 

	// Use this for initialization
	void Start () {
		// Destroy (rigidbody);
		is_floating = false;
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
				is_floating = true;
				// put sucking in air to float animation in here
				print ("is_floating is true");
			}
		}
		// down input
		if (Input.GetKey(KeyCode.DownArrow) && !is_floating)
		{
			// duck!
			print ("duck!");
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
				print ("release air. is_floating is false");
			} else {
				// suck in air
				print ("suck in air");
			}
		}
		// combination input

	}

	void OnTriggerEnter(Collider col) {
		// When Kirby collides with something
		print ("Kirby collided");
		if (col.gameObject.name == "Ground") {
			// GameObject thePE_Obj = GameObject.Find ("PE_Obj");
			PE_Obj my_obj = gameObject.GetComponent<PE_Obj> ();
			my_obj.acc = Vector3.zero; 
			my_obj.vel = Vector3.zero; 
			my_obj.reached_ground = true; 
			print ("Kirby hit ground");
		}
	}

}
