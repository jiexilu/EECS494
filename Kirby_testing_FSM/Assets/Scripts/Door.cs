using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public Camera main_camera;
	public int cur_level = 1; 
	public Transform doorPort;

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
				//Vector3 start = new Vector3(-8.72f,-10.33f, 0f);
				col.gameObject.transform.position = doorPort.position;

				Vector3 cam_location = doorPort.position;
//				cam_location.y = -10.33f;
				cam_location.z = main_camera.transform.position.z;
//				cam_location.x = -5.067625f;
				main_camera.transform.position = cam_location;
			}
		}
	}
}
