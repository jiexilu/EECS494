using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Camera_follow : MonoBehaviour {

	public Transform target;
	public RaycastHit rHit;
	public int cur_level = 1;
	public bool pause_power = false;

	public  GameObject prof;
	public GameObject[] enemies;
	public GameObject[] bossPowers;
	private bool prev_pause = false;

	void Awake(){

	}

	// Use this for initialization
	void Start () {
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
	}
	
	// Update is called once per frame
	void Update () {
		bossPowers = GameObject.FindGameObjectsWithTag ("bossPower");

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
		if (pause_power) {
			print ("pause is ture");		
		}
		if(prev_pause != pause_power){
			print ("pause power changed");
			foreach(var x in enemies){
				Enemy_1 bully = x.GetComponent<Enemy_1>();
				if(bully == null){
					print ("wtf " + x.name);
				}
				bully.pause_attacked = pause_power;
			}

			if(bossPowers.Length > 0){
				foreach(var y in bossPowers){
				 //freeze the power in midair
					book booky = y.GetComponent<book>();
					if(booky == null){
						print ("huh? " + y.name);
					}
					else{
						booky.pause_attacked = pause_power;
					}
				}
			}
			bossProf bossy = prof.GetComponent<bossProf>();
			bossy.pause_attacked = true;
		}

		prev_pause = pause_power;
	}

}
