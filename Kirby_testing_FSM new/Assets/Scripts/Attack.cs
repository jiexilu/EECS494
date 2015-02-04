using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {
	
	public Animator sprite;
	private float distance;
	public bool go_right;
	public bool poof = false;
	public power_type power;
	private float usage;
	private float delay;
	public bool kirby;
	
	// Use this for initialization
	void Start () {
		delay = 0.5f;
		usage = Time.time + delay;
		distance = go_right ? transform.position.x + 2f : transform.position.x - 2f;
	}
	
	// Update is called once per frame
	void Update () {
		if (poof) {
			shoot ();		
		}
	}
	
	void shoot(){
		if (power == power_type.none) {
			if (go_right) {
				transform.Translate (Time.deltaTime * 6, 0, 0);
				sprite.SetInteger("Direction", 0);
				if (transform.position.x > distance) {
					Destroy (this.gameObject);	
				}
			} else {
				sprite.SetInteger("Direction", 1);
				transform.Translate (Time.deltaTime * -6, 0, 0);
				if (transform.position.x < distance) {
					Destroy (this.gameObject);	
				}
			}
		} else if (power == power_type.beam) {
			if (go_right) {
				gameObject.transform.Rotate (0, 0, -180 * Time.deltaTime * 2);
			} else {
				gameObject.transform.Rotate (0, 0, 180 * Time.deltaTime * 2);
			}
			if (Time.time > usage) {
				Destroy (this.gameObject);
			}
		} else if (power == power_type.sing) {
			gameObject.transform.Translate (0, Time.deltaTime * 3, 0);
			if (transform.position.y > 4f) {
				Destroy (this.gameObject);
			}
		} else if (power == power_type.spark) {
			if (Time.time > usage) {
				Destroy (this.gameObject);
			}
		} else if (power == power_type.fire) {
			if (go_right) {
				transform.Translate (Time.deltaTime * 2, 0, 0);
				sprite.SetInteger("Direction", 0);
			} else {
				transform.Translate (Time.deltaTime * -2, 0, 0);
				sprite.SetInteger("Direction", 1);
			}
			if (Time.time > usage) {
				Destroy (this.gameObject);	
			}
		}
	}
	
	void OnTriggerEnter(Collider col){
		if (kirby) {
			if (col.tag == "Enemy" && power != power_type.sing) {
				print ("attack");
				col.gameObject.SetActive (false);
				if (power == power_type.none) {
					Destroy (this.gameObject);
				}
			}
		} else {
			if(col.tag == "Player"){
				print ("attack");
				Kirby kirbs = col.GetComponent<Kirby>();
				kirbs.Got_Attacked();
			}
		}
	}
}
