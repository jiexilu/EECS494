using UnityEngine;
using System.Collections;

public class music : MonoBehaviour {

	public Kirby kirby;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "Player") {
//			kirby.power = power_type.sing;
			kirby.cur_state = State.stand_power;
			gameObject.SetActive(false);
		}
	}
}
