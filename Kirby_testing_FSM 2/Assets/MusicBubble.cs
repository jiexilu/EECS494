using UnityEngine;
using System.Collections;

public class MusicBubble : MonoBehaviour {

	Transform kirby;
	private float growthRate = 3f;
	private float scale = 1.5f;
	private Vector3 freeze;
	private float usage;
	private float delay = 3f;

	// Use this for initialization
	void Start () {
		renderer.enabled = false;
		usage = Time.time + delay;
	}
	
	// Update is called once per frame
	void Update () {
		//grow in size
		transform.localScale = Vector3.one * scale;
		scale += growthRate * Time.deltaTime;
		if (Time.time > usage) {
			Destroy (gameObject);		
		}
	}

	void OnTriggerEnter(Collider col){
		freeze = col.transform.position;
		if (col.tag == "Enemy") {
			print ("frozen");
		}
//		col.gameObject.transform.position;
	}

	void OnTriggerStay(Collider col){
		if (col.tag == "Enemy") {
			col.transform.position = freeze;		
		}			
	}
}
