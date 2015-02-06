using UnityEngine;
using System.Collections;

public class LoadLevelDoor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnTriggerStay(Collider col){
		if (col.tag == "Player") {
			if (Input.GetKeyUp (KeyCode.UpArrow)) {
				Application.LoadLevel(2);		
				return;
			}		
		}
	}
}
