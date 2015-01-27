using UnityEngine;
using System.Collections;

public class Camera_follow : MonoBehaviour {

	public Transform target;

	// Use this for initialization
	void Start () {
		Camera cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tp = transform.position;
		if (target.position.x > -5.14 && target.position.x < 17.11) {
			tp.x = target.position.x;
		}
		if (target.position.y < 2.687393) {
			tp.y = target.position.y;
		}
		transform.position = tp;
	}
}
