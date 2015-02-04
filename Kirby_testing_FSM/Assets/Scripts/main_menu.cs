using UnityEngine;
using System.Collections;

public class main_menu : MonoBehaviour {
		
	public GameObject subMenu;

	// Use this for initialization
	void Start () {
		subMenu.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) {
			subMenu.SetActive(true);
		}
	}
}
