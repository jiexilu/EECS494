using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {
	
	private float distance;
	public Kirby kirby;
	public bool go_right;
	public bool poof = false;
	public power_type power;
	private float usage;
	private float delay;
	
	// Use this for initialization
	void Start () {
		delay = 2;
		kirby = GetComponent<Kirby> ();
//		transform.position = kirby.transform.position;
		distance = go_right ? transform.position.x + 2f : transform.position.x - 2f;
	}
	
	// Update is called once per frame
	void Update () {
		if (poof) {
			usage = Time.time + delay;
			shoot ();		
		}
	}
	
	void shoot(){
		if (power == power_type.none || power == power_type.fire) {
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
		if (power == power_type.beam) {
			print ("use beam");
			gameObject.transform.Rotate(0, 0, -180 * Time.deltaTime);
			if(Time.time > usage){
				Destroy (this.gameObject);
			}
		}
	}
	
	void OnTriggerEnter(Collider col){
		if (col.tag == "Enemy") {
			col.gameObject.SetActive (false);
			Destroy (this.gameObject);
			kirby.near_enemy = false;
		}
	}
}
