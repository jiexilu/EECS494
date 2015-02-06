using UnityEngine;
using System.Collections;

public class health_cube : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider col){
		if (col.tag == "Player") {
			Kirby kirby = col.gameObject.GetComponent<Kirby>();
			kirby.health = 6;
			Destroy(this.gameObject);
		}
	}
}
