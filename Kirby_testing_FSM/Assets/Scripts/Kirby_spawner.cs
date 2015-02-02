using UnityEngine;
using System.Collections;

public class Kirby_spawner : MonoBehaviour {

	private Vector3 position = new Vector3 ();

	// Use this for initialization
	void Start () {
		position = gameObject.transform.position;
		position.z = 7.16f;
		gameObject.transform.position = position;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
