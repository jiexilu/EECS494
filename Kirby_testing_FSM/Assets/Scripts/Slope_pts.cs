using UnityEngine;
using System.Collections;

public class Slope_pts : MonoBehaviour {
	public Transform TL, TR, BL, BR;

	// Use this for initialization
	void Awake () {
		TL = transform.Find ("TL");
		TR = transform.Find ("TR");
		BL = transform.Find ("BL");
		BR = transform.Find ("BR");
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log ("TL: " + TL.position.ToString());
//		Debug.Log ("TR: " + TR.position.ToString());
//		Debug.Log ("BL: " + BL.position.ToString());
//		Debug.Log ("BR: " + BR.position.ToString());
	}
}
