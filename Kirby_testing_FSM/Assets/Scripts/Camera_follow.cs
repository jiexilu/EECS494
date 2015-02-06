﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Camera_follow : MonoBehaviour {

	public Transform target;
	public RaycastHit rHit;
	public int cur_level = 1;
	public bool music_power = false;

	public  GameObject prof;

	void Awake(){

	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
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
		switch (cur_level) {
			case 1:
				if (target.position.y != 2.03f) {
					tp.y = 2.03f;
				}
				break;
			case 2:
				if(target.position.y != -9.23f){
					tp.y = -9.23f;
				}
				break;
			case 3:
				if (target.position.y != -20.46f) {
				tp.y = -20.46f;
				}
				break;
			case 4:
				if (target.position.y != -5.46f) {
					tp.y = -5.46f;
				}
				if (target.position.x < -4.03f) {
					tp.x = -4.03f;
				} else if (target.position.x > 15.54f) {
					tp.x = 15.54f;	
				}
				break;
			case 5:
				if (target.position.y != -16.5f) {
					tp.y = -16.5f;
				}
				if (target.position.x < -4.03f) {
					tp.x = -4.03f;
				} else if (target.position.x > 15.54f) {
					tp.x = 15.54f;	
				}
				bossProf boss = prof.GetComponent<bossProf>();
				boss.ready = true;
				//spawn boss
				break;
			case 6:
				if(target.position.y < -28.23 ){
					tp.y = target.transform.position.y;
				}
				if (target.position.x < -4.03f) {
					tp.x = -4.03f;
				} else if (target.position.x > 15.54f) {
					tp.x = 15.54f;	
				}
				break;
		}

		transform.position = tp;

	}

}
