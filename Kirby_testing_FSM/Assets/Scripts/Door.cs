using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	
	public Camera main_camera;
	public Transform doorPort;
	public int next_level = 2;
	
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
			Kirby kirby = col.gameObject.GetComponent<Kirby> ();
			if((Input.GetKey(KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) 
			   && (kirby.cur_state == State.stand || kirby.cur_state == State.stand_power)){
				//Vector3 start = new Vector3(-8.72f,-10.33f, 0f);
				Vector3 new_pos = doorPort.position; 
				new_pos.z = 0;
				col.gameObject.transform.position = new_pos;
				
				Vector3 cam_location = doorPort.position;
				//				cam_location.y = -10.33f;
				cam_location.z = main_camera.transform.position.z;
				cam_location.x = -5.067625f;
				main_camera.transform.position = cam_location;
				Camera_follow cam = main_camera.GetComponent<Camera_follow>();
				cam.cur_level = next_level;
				if (cam.cur_level == 4) {
					Application.LoadLevel(2);
				}
			}
		}
	}
}
