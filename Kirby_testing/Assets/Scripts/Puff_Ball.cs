using UnityEngine;
using System.Collections;

public class Puff_Ball : MonoBehaviour {

	private float distance;
	public Kirby kirby;

	// Use this for initialization
	void Start () {
		distance = transform.position.x + 2f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Time.deltaTime * 6, 0, 0);
		if (transform.position.x > distance) {
			Destroy (this.gameObject);	
		}
	
	}
}
