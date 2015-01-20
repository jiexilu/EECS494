using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public Camera main_camera;

	// Use this for initialization
	void Start () {
		renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider col){
		if (col.gameObject.tag == "Player") {
			print ("Kirby at door");
			if(Input.GetKey(KeyCode.UpArrow)){
				Vector3 start = new Vector3(-8.72f,-10.33f, 0f);
				col.gameObject.transform.position = start;

				Vector3 cam_location = start;
				cam_location.y = -10.33f;
				cam_location.z = main_camera.transform.position.z;
				main_camera.transform.position = cam_location;
			}
		}
	}
}
