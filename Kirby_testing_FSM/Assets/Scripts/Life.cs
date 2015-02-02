using UnityEngine;
using System.Collections;

public class Life : MonoBehaviour {

	public Kirby kirby;
	public Animator life;

	// Use this for initialization
	void Start (){
		kirby.life = 4f;
		life.SetInteger ("Numbers", 4);
	}
	
	// Update is called once per frame
	void Update () {
		if (kirby.life == 3) {
			life.SetInteger ("Numbers", 3);		
		} else if (kirby.life == 2) {
			life.SetInteger ("Numbers", 2);		
		} else if (kirby.life == 1) {
			life.SetInteger ("Numbers", 1);
		}
	}
}
