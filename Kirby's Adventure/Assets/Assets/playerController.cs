using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	private Animator animator;
	public int before = 0;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		var horizontal = Input.GetAxis ("Horizontal");

		if (Input.GetKey (KeyCode.RightArrow)) {
			animator.SetInteger("Direction", 2);
			before = 2;
		}
		else if(Input.GetKey (KeyCode.RightArrow)) {
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
		else if(Input.GetKey(KeyCode.A)){
			if(before == 2){
				animator.SetInteger("Direction", 6);
			}
			else if(before == 3){
				animator.SetInteger ("Direction", 5);
			}
		}
	}
}
