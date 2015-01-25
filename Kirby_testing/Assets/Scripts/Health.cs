using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public Animator health;
	public Kirby kirby;

	// Use this for initialization
	void Start () {
		health.SetBool ("Health", true);
	}
	
	// Update is called once per frame
	void Update () {
		switch (kirby.health) {
			case 6:
				health.SetBool("Health", true);
				break;
			case 5:
				if(health.name == "Kirby_health_5"){
					health.SetBool("Health", false);
				}
				break;
			case 4:
				if(health.name == "Kirby_health_4"){
					health.SetBool("Health", false);
				}
				break;
			case 3:
				if(health.name == "Kirby_health_3"){
					health.SetBool("Health", false);
				}
				break;
			case 2:
				if(health.name == "Kirby_health_2"){
					health.SetBool("Health", false);
				}
				break;
			case 1:
				if(health.name == "Kirby_health_1"){
					health.SetBool("Health", false);
				}
				break;
			case 0:
				if(health.name == "Kirby_health_0"){
					health.SetBool("Health", false);
				}
				break;
		}
	}
}
