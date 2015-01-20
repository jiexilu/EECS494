using UnityEngine;
using System.Collections;

public class Puff_Ball : MonoBehaviour {

	private float distance;
	public Kirby kirby;

	// Use this for initialization
	void Start () {
		distance = transform.position.x + 10f;
	}
	
	// Update is called once per frame
	void Update () {
//		transform.Translate (transform.position.x + 10, transform.position.y, transform.position.z);
		if (transform.position.x < distance) {
			Destroy (this.gameObject);		
			//Get a ref to the appliePicker component of the main camera
//			ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();
//			//Call the public AppleDestroyed() method of apscript
//			apScript.AppleDestroyed();
		}
	
	}
}
