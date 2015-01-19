using UnityEngine;
using System.Collections;

public class Enemy_1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col){
		print ("bumped the enemy");
		if (col.gameObject.name == "Ground") {
			// GameObject thePE_Obj = GameObject.Find ("PE_Obj");
			PE_Obj my_obj = gameObject.GetComponent<PE_Obj> ();
			my_obj.acc = Vector3.zero; 
			my_obj.vel = Vector3.zero; 
			my_obj.reached_ground = true; 
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
