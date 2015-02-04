using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Camera_follow : MonoBehaviour {

	public GameObject[] prefabEnemies;
	public Transform target;
	public RaycastHit rHit;
	public int cur_level = 1;
	public bool music_power = false;

	void Awake(){

	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
//		if (music_power) {
//			foreach (var enemy in prefabEnemies) {
//				Vector3 point = enemy.transform.position;
//				Ray inCamera = camera.ScreenPointToRay (point);
//				if (Physics.Raycast (inCamera, out rHit)) {
//						print ("go to sleep");
//						PE_Obj enemy_sleep = enemy.GetComponent<PE_Obj> ();
//						enemy_sleep.vel.x = 0;	
//				}	
//			}
//		} else {
//			foreach (var enemy in prefabEnemies) {
//				Vector3 point = enemy.transform.position;
//				PE_Obj enemy_sleep = enemy.GetComponent<PE_Obj> ();
//			}	
//		}


		Vector3 tp = transform.position;
		tp.x = target.position.x;
		if (target.position.x < -5.14f) {
			tp.x = -5.12f;
		} else if (target.position.x > 40.04f) {
			tp.x = 40.04f;	
		}
		else if (target.position.x > -5.14 && target.position.x < 40.04f) {
			tp.x = target.position.x;
		}
		//TODO: change it so it works for each level
		switch (cur_level) {
			case 1:
				if (target.position.y < 2.687393) {
					tp.y = target.position.y;
				}
				break;
			case 2:
				if(target.position.y < -8.53){
					tp.y = target.position.y;
				}
				break;
			case 3:
				if (target.position.y < -19.96) {
					tp.y = target.position.y;
				}
				break;
			case 4:
				if (target.position.y < -30.34) {
					tp.y = target.position.y;
				}
				break;
		}

		transform.position = tp;

		//if enemy is off screen is disabled, make them active
//		prefabEnemies [0].SetActive (true);
//		prefabEnemies [1].SetActive (true);
//		prefabEnemies [2].SetActive (true);
	}

}
