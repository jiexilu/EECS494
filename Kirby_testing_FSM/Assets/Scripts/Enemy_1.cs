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
	public float 	leftAndRightEdge = 20f;
	public float 	speed = 7f;
	private float distance;
	public int score = 400; //points earned for destroying
	public Vector3[] points;

	//Pacing//
//	public Vector3 start;
//	public Vector3 pointA;
//	public Vector3 pointB;


	private float usage;
	private float delay;
	private Vector3 vel;
	private PE_Obj my_obj;
	private RaycastHit hit;
	private RaycastHit attack_ray;
	public enemy_state state;
	private float fieldOfViewRange = 68.0f;
	private Vector3 rayDir = Vector3.zero;
	private int index = 0;

	//powers
	public GameObject beam_string;

	// Use this for initialization
	void Start () {
		my_obj = GetComponent<PE_Obj> ();
		delay = 0.5f;
		usage = Time.time + delay;
		renderer.enabled = false;
		distance = Mathf.Abs (kirby.position.x - transform.position.x);
//		start = transform.position;
		state = enemy_state.wander;
//		while (true) {
//			yield return StartCoroutine(state_wander
//				}

	}
	
	// Update is called once per frame
	void Update () {
		//Basic movement to the right
		vel = my_obj.vel; 
//		-------
		//if they bump something, change directions or jump
		rayDir = kirby.position - transform.position;
		
		if (((Vector3.Angle (rayDir, Vector3.left)) < fieldOfViewRange) || 
		    (Vector3.Angle(rayDir, Vector3.right) < fieldOfViewRange)) {
			if(Physics.Raycast (transform.position, rayDir, out hit, 200f)){
				if(hit.transform.tag == "Player"){
					if(hit.distance < 10f){
						state = enemy_state.attack;
					}
					state = enemy_state.chase;
				}
				else{
					state = enemy_state.wander;
				}
			}
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
		} else {
			sprite.SetInteger("Action", 1);	
		}
		my_obj.vel = vel;
	}

//	IEnumerator MoveObject(Transform thisTransform, Vector3 start, Vector3 endPos, float Time){
//			
//			}

	void FixedUpdate(){

	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Ground") {
			// GameObject thePE_Obj = GameObject.Find ("PE_Obj");
			PE_Obj my_obj = gameObject.GetComponent<PE_Obj> ();
			my_obj.acc = Vector3.zero; 
			my_obj.vel = Vector3.zero; 
//			my_obj.reached_ground = true; 
		}
		if (col.gameObject.tag == "Enemy") {
			Physics.IgnoreCollision(gameObject.collider, col);
		}
//		if (col.gameObject.tag == "Player") {
//			Kirby kirb = kirby.gameObject.GetComponent<Kirby>();
//			kirb.Got_Attacked(); 
//		}
	}

	void Attack(){
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
				break;
			case power_type.spark:
				print ("ENEMY ATTACKS WITH SPARK");
				break;
			}
		//TODO:
		//call kirby's got_attacked() if he got hit
		//maybe put it on the projectile
	}

	void state_wander(){
		print ("wandering enemy");

//		if (Physics.Raycast (transform.position, rayDir, out hit, 10f)) {
//
//		}
//		int len = points.Length;
//		if (index >= len) {
//			index = 0;		
//		}
//		var dir = points[index] - gameObject.transform.position;
//			dir.Normalize();
//			if(gameObject.transform.position != points[0]){
//				vel.x = dir.x * speed; 
//			}
	//what to do when done pacing?
//		gameObject.SetActive (false);
	}
	
	void state_chase(){
		print ("chasing kirby");
		Vector3 temp = rayDir;
		temp.Normalize ();
		vel.x = temp.x * speed;
	}

	void Beam(){
		GameObject beam = Instantiate (beam_string) as GameObject;
		beam.transform.position = gameObject.transform.position;
		beam.transform.Rotate(0, 0, 180 * Time.deltaTime*2);
		if(Time.time > usage){
			Destroy (this.gameObject);
		}
	}


	
}
