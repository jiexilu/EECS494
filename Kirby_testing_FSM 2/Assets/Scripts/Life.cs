using UnityEngine;
using System.Collections;

public class Life : MonoBehaviour {

	public Kirby kirby;
	public Animator life;

	// Use this for initialization
	void Start () {
		life.SetInteger ("Numbers", 4);
	}
	
	// Update is called once per frame
	void Update () {
//		if (kirby.life == 3) {
//			life.SetInteger("Numbers", 3);		
//		}
	}
}
