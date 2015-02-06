using UnityEngine;
using System.Collections;



public class ProfBoss : MonoBehaviour {

	public Animator sprite;
	public Direction cur_dir;
	private float usage;
	private float delay = 7f;
	private Vector3 vel;
	private PE_Obj my_obj;
	public float speed = 1f;

	public prof_state cur_state = prof_state.walk;
	public GameObject pointA;
	public GameObject pointB;
	public int life = 10;

	//shout stuff
	public bool set_delay = false;
	private float shout_delay = 3f;
	private float shout_usage;

	// Use this for initialization
	void Start () {
		my_obj = GetComponent<PE_Obj> ();
		delay = 2f;
		prof_state cur_state = prof_state.walk;
		usage = Time.time + delay;
	}
	
	// Update is called once per frame
	void Update () {
		vel = my_obj.vel;

		switch (cur_state) {
			case prof_state.walk:
			state_walk ();
				break;
			case prof_state.shout:
				state_shout ();
				break;
			case prof_state.toss:
				break;
			case prof_state.fall:
				break;
		}
		my_obj.vel = vel;

		if (Time.time > usage) {
			cur_state = prof_state.shout;
		}

		if (vel.x < 0) {
			sprite.SetInteger("Action", 0);	
			cur_dir = Direction.left;
		} else {
			sprite.SetInteger("Action", 1);	
			cur_dir = Direction.right;
		}
	}

	void state_walk(){
		print ("walk prof");
		if (vel == Vector3.zero) {
			print ("zilch");
			vel.x = -speed;		
		}
		//walks between two points
		if (gameObject.transform.position.x < pointA.transform.position.x || 
						gameObject.transform.position.x > pointB.transform.position.x) {
			print ("change directions");
			vel.x = -vel.x;

		}
	}

	void state_shout(){
		if (!set_delay) {
			shout_usage = Time.time + shout_delay;
			set_delay = true;
		}
		vel = Vector3.zero;
		if (Time.time < shout_usage) {
			print ("shout prof");
			sprite.SetInteger("Action", 2);	
		}
		if (Time.time > shout_usage) {
			usage = Time.time + delay;
			cur_state = prof_state.walk;
		}
	}

	void OnTriggerEnter(Collider col){
		print (col.gameObject.tag);
		if (col.gameObject.tag == "Attack") {
			life--;
		}				
	}

}
