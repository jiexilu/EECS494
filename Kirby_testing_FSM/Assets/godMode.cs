using UnityEngine;
using System.Collections;

public class godMode : MonoBehaviour {

	public bool god_mode;
	public GameObject go;

	// Use this for initialization
	void Start () {
		god_mode = false;
		DontDestroyOnLoad (go);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
