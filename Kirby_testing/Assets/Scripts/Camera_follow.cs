using UnityEngine;
using System.Collections;

public class Camera_follow : MonoBehaviour {

	public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tp = transform.position;
		tp.x = target.position.x;
		transform.position = tp;
	}
}
