using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {
	
	private float pos1;
	// Use this for initialization
	void Start () {
		renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		 
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.name == "Kirby") {
			pos1 = col.gameObject.transform.position.y;	
		}
	}
	
	void OnTriggerStay(Collider col){
		if (col.gameObject.name == "Kirby") {
			Vector3 player = col.gameObject.transform.position;
			player.y = pos1;
			col.gameObject.transform.position = player;
		}
	}
}
