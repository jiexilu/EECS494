using UnityEngine;
using System.Collections;

public class Power : MonoBehaviour {

	public Kirby kirby;
	public Animator power;

	// Use this for initialization
	void Start () {
		power.SetInteger ("Power", 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (kirby.power == power_type.none) {
			power.SetInteger ("Power", 0);	
		} else if (kirby.power == power_type.spark) {
			power.SetInteger ("Power", 1);	
		}
		else if (kirby.power == power_type.beam) {
			power.SetInteger ("Power", 2);	
		}
	}
}
