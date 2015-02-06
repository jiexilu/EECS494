using UnityEngine;
using System.Collections;

public class bossProf : MonoBehaviour {

	public Transform kirby;
	public Animator sprite;
	public Direction cur_dir;
	private float usage;
	private float delay;
	private Vector3 vel;
	private PE_Obj my_obj;
	public float speed = 1f;
	
	public prof_state cur_state = prof_state.walk;
	public GameObject pointA;
	public GameObject pointB;
	public int life = 10;

	//attack stuff
	public bool set_delay = false;
	private float toss_delay = 2f;
	private float toss_usage;
	public GameObject[] books;
	public GameObject Fs;

	public bool ready = false;

	// Use this for initialization
	void Start () {
		gameObject.transform.position = pointA.transform.position;
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
			case prof_state.toss:
				state_toss ();
				break;
			case prof_state.shout:
				state_shout ();
				break;
			case prof_state.fall:
				break;
		}
		my_obj.vel = vel;
		
		if (Time.time > usage && ready) {
			cur_state = Random.value < .5 ? prof_state.toss : prof_state.shout;
		}

		if (cur_state == prof_state.walk) {
			if (vel.x < 0) {
					sprite.SetInteger ("Action", 0);	
					cur_dir = Direction.left;
			} else {
					sprite.SetInteger ("Action", 1);	
					cur_dir = Direction.right;
			}
		}
	}
	
	void state_walk(){
		print ("walk prof");
		if (vel == Vector3.zero) {
			print ("zilch");
			vel.x = Random.Range (speed, -speed);		
		}
		if (gameObject.transform.position.x < pointA.transform.position.x) {
			print ("A");
			vel.x = -vel.x;		
		}
		if (gameObject.transform.position.x > pointB.transform.position.x) {
			print ("B");
			vel.x = -vel.x;		
		}
	}
	
	void state_toss(){
		if (!set_delay) {
			toss_usage = Time.time + toss_delay;
			set_delay = true;
		}
		vel = Vector3.zero;
		vel.y = speed;
		usage = Time.time + toss_delay;
		if (Time.time < toss_usage) {
			print ("toss prof");
			sprite.SetInteger("Action", 2);	
			foreach(var lit in books){
				book_spawn paper = lit.GetComponent<book_spawn>();
				paper.trigger = true;
			}
		}
		if (Time.time > toss_usage) {
			usage = Time.time + delay;
			cur_state = prof_state.walk;
			set_delay = false;
			foreach(var lit in books){
				book_spawn paper = lit.GetComponent<book_spawn>();
				paper.trigger = false;
			}
		}
	}

	void state_shout(){
		print ("shout");
		if (!set_delay) {
			toss_usage = Time.time + .25f;
			set_delay = true;
			print ("shout prof");
			sprite.SetInteger("Action", 2);	
			GameObject projectile = Instantiate (Fs) as GameObject;
			projectile.transform.position = transform.position;
//			projectile.transform.LookAt(kirby);
			projectile.transform.Translate(kirby.position * Time.deltaTime * 5f);
		}
		if (Time.time < toss_usage) {
			GameObject projectile = Instantiate (Fs) as GameObject;
			projectile.transform.position = transform.position;
			//			projectile.transform.LookAt(kirby);
			projectile.transform.Translate(kirby.position * Time.deltaTime * 5f);
		}
		usage = Time.time + delay;
		vel = Vector3.zero;
		if (Time.time > toss_usage) {
			usage = Time.time + delay;
			cur_state = prof_state.walk;
			set_delay = false;
		}
	}
	
	void OnTriggerEnter(Collider col){
		print (col.gameObject.tag);
		if (col.gameObject.tag == "Attack") {
			life--;
		}				
	}
}
