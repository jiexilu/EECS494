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
		if (tag == "Wall") {
			if (col.gameObject.tag != "Ground" && col.gameObject.tag != "Ceiling") {
					pos1 = col.gameObject.transform.position.x;		
			}
		}
		else if (tag == "Ceiling" && col.gameObject.tag != "Wall") {
			pos1 = col.gameObject.transform.position.y;	
			print ("ceiling collid");
		}
	}

	void OnTriggerStay(Collider col){
		Vector3 player = col.gameObject.transform.position;
		if (tag == "Wall" && col.gameObject.tag != "Ground") {
			print ("Wall collide");
			player.x = pos1;
			col.gameObject.transform.position = player;
		}
		else if (tag == "Ceiling" && col.gameObject.tag != "Wall") {
			player.y = pos1;
			col.gameObject.transform.position = player;
		}
	}
}
