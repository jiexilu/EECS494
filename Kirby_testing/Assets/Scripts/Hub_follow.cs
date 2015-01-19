using UnityEngine;
using System.Collections;

public class Hub_follow : MonoBehaviour {

	public Transform camera;
	private float distance;

	// Use this for initialization
	void Start () {
		distance = camera.position.x - transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tp = transform.position;
		tp.x = camera.position.x - distance;
		transform.position = tp;
	}
}
