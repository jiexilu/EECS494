using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	private float pos1;
	// Use this for initialization

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag != "Ground") {
			pos1 = col.gameObject.transform.position.x;		
		}
	}

	void OnTriggerStay(Collider col){
		if (col.gameObject.tag != "Ground") {
			Vector3 player = col.gameObject.transform.position;
			player.x = pos1;
			col.gameObject.transform.position = player;
		}
	}
}
