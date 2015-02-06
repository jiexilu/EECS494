using UnityEngine;
using System.Collections;

public class powerup_text : MonoBehaviour {

	private float usage;
	private bool set = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (this.guiText.color == Color.black) {
			if(!set){
				usage = Time.time + 4f;	
				set = true;
			}
			if(Time.time > usage){
				this.guiText.color = Color.clear;
			}
		}
	}
}
