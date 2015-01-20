using UnityEngine;
using System.Collections;

public enum power_type{
	none, spark, beam, fire
}

public class Enemy_1 : MonoBehaviour {

	public Transform kirby;
	public power_type power;
	public float 	leftAndRightEdge = 10f;
	public float	chanceToChangeDirections = 0.02f;
	public float 	speed = 1f;

	// Use this for initialization
	void Start () {
		renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Basic Movement
		Vector3 pos = transform.position;
		pos.x += speed * Time.deltaTime;
		transform.position = pos;
		
		//Changing Directions
		if (pos.x < -leftAndRightEdge) {
			speed = Mathf.Abs (speed); //Move Right
		} else if (pos.x > leftAndRightEdge) {
			speed = -Mathf.Abs (speed); //Move left
		}
	}

	void FixedUpdate(){
		if(Random.value < chanceToChangeDirections){
			speed *= -1; //Change Direction
		}
	}

	void OnTriggerEnter(Collider col){
		print ("bumped the enemy");
		if (col.gameObject.name == "Ground") {
			// GameObject thePE_Obj = GameObject.Find ("PE_Obj");
			PE_Obj my_obj = gameObject.GetComponent<PE_Obj> ();
			my_obj.acc = Vector3.zero; 
			my_obj.vel = Vector3.zero; 
//			my_obj.reached_ground = true; 
		}
	}

	//making it walk
//	IEnumerator patrol(){
//		for(float i = 0f ; i < 10f; i++){
//			transform.position.x = i;
//			yield return new WaitForSeconds(.1f);
//		}
//	}
}
