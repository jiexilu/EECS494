using UnityEngine;
using System.Collections;

public class Hub_follow : MonoBehaviour {

	public Transform camera;
	private float distance;
	private float vertical;

	// Use this for initialization
	void Start () {
		distance = camera.position.x - transform.position.x;
		vertical = camera.position.y - transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tp = transform.position;
		tp.x = camera.position.x - distance;
		tp.y = camera.position.y - vertical;
		transform.position = tp;
	}
}
