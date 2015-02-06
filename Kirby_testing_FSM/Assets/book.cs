using UnityEngine;
using System.Collections;

public class book : MonoBehaviour {

	private PE_Obj my_obj;
	private Vector3 disappear;

	// Use this for initialization
	void Start () {
//		my_obj = GetComponent<PE_Obj> ();
		disappear = gameObject.transform.position;
		disappear.y = -6;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.down * 3f * Time.deltaTime, Space.World);
	}

	void OnTriggerEnter(Collider Col){
		if (Col.tag != "boss") {
				Destroy (this.gameObject);
		}
	}
}
