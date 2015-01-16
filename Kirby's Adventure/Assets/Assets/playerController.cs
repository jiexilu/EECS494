using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	private Animator animator;
	public int before;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();
		before = 0;
	}
	
	// Update is called once per frame
	void Update () {

		var horizontal = Input.GetAxis ("Horizontal");

		if (horizontal > 0) {
			animator.SetInteger("Direction", 2);
			before = 2;
		}
		else if(horizontal < 0) {
			animator.SetInteger("Direction", 3);
			before = 3;
		}
		else if(horizontal == 0){
			if(before == 2)
			{
				animator.SetInteger("Direction", 0);
			}
			else{
				animator.SetInteger("Direction", 1);
			}
		}
	}
}
