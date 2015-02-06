using UnityEngine;
using System.Collections;

public class book : MonoBehaviour {

	private PE_Obj my_obj;
	private Vector3 disappear;
	public bool pause_attacked = false;
	private bool prev_paused = false;
	private Vector3 prev_position;

	// Use this for initialization
	void Start () {
//		my_obj = GetComponent<PE_Obj> ();
		disappear = gameObject.transform.position;
		disappear.y = -6;
		prev_position = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		prev_position = gameObject.transform.position;
		if (prev_paused != pause_attacked) {
			if(pause_attacked){
				gameObject.transform.position = prev_position;
			}
			else{
				transform.Translate (Vector3.down * 3f * Time.deltaTime, Space.World);
			}
		}
		if(!pause_attacked){
			transform.Translate (Vector3.down * 3f * Time.deltaTime, Space.World);
		}
		prev_paused = pause_attacked;
	}

	void OnTriggerEnter(Collider Col){
		if (Col.tag != "boss") {
				Destroy (this.gameObject);
		}
		if (Col.tag == "Attack") {
			print ("kirby attack");		
		}
	}
}
