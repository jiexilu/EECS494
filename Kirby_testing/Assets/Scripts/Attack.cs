using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {
	
	private float distance;
	public Kirby kirby;
	public bool go_right;
	public bool poof = false;
	public power_type power;
	
	// Use this for initialization
	void Start () {
		distance = go_right ? transform.position.x + 2f : transform.position.x - 2f;
	}
	
	// Update is called once per frame
	void Update () {
		if (poof) {
			shoot ();		
		}
	}
	
	void shoot(){
		if (power != power_type.spark) {
			if (go_right) {
				transform.Translate (Time.deltaTime * 6, 0, 0);
				if (transform.position.x > distance) {
					Destroy (this.gameObject);	
				}
			} else {
				transform.Translate (Time.deltaTime * -6, 0, 0);
				if (transform.position.x < distance) {
					Destroy (this.gameObject);	
				}
			}
		}
	}
	
	void OnTriggerEnter(Collider col){
		if (col.tag == "Enemy") {
			print ("hit the poof ball");
			col.gameObject.SetActive (false);
			Destroy (this.gameObject);
		}
	}
}
