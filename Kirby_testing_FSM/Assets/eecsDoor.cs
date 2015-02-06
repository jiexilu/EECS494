using UnityEngine;
using System.Collections;

public class eecsDoor : MonoBehaviour {

	public GUIText txt;
	public Kirby kirby;
	private float usage;
	public bool set = false;

	// Use this for initialization
	void Start () {
		txt.color = Color.clear;
	}
	
	// Update is called once per frame
	void Update () {	
		if (kirby.transform.position.x > 16.501 && !kirby.has_pause) {
				txt.color = Color.black;
		} else {
			txt.color = Color.clear;		
		}
	}
}
