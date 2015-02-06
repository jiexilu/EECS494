using UnityEngine;
using System.Collections;

public class gradeF : MonoBehaviour {

	private float usage;

	// Use this for initialization
	void Start () {
		usage = Time.time + 3f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > usage) {
			Destroy(this.gameObject);		
		}
	}
}
