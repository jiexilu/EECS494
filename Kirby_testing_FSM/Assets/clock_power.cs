using UnityEngine;
using System.Collections;

public class clock_power : MonoBehaviour {

	public GUIText txt;

	// Use this for initialization
	void Start () {
		txt.color = Color.clear;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col){
		if (col.tag == "Player") {
			txt.color = Color.black;
			GameObject kirby = GameObject.FindGameObjectWithTag("Player");
			Kirby kirbs = kirby.GetComponent<Kirby>();
			kirbs.has_pause = true;
			print ("picked up power");
			//trigger a message that announces the new power
			Destroy(this.gameObject);	
		}
	}
}
