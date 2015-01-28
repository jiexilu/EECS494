using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Camera_follow : MonoBehaviour {

	static public Camera_follow S;
	public GameObject[] prefabEnemies;
	public Transform target;

	void Awake(){
		S = this;
	}

	// Use this for initialization
	void Start () {
		Camera cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tp = transform.position;
		if (target.position.x > -5.14 && target.position.x < 17.11) {
			tp.x = target.position.x;
		}
		//TODO: change it so it works for each level
		if (target.position.y < 2.687393) {
			tp.y = target.position.y;
		}
		transform.position = tp;

		//if enemy is off screen is disabled, make them active
//		prefabEnemies [0].SetActive (true);
//		prefabEnemies [1].SetActive (true);
//		prefabEnemies [2].SetActive (true);
	}
}
