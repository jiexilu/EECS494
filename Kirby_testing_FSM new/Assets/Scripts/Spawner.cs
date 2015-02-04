using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
	public GameObject my_enemy;
	private Vector3 position = new Vector3 ();
	public bool should_spawn = true;
	
	// Use this for initialization
	void Start () {
		position = gameObject.transform.position;
		position.z = 7.16f;
		gameObject.transform.position = position;
		//		my_enemy.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	//initial view
	void OnBecameInvisible(){
		should_spawn = true;
	}
	
	void OnBecameVisible(){
		//		can only test this when away from the scene view in unity
		if (should_spawn) {
			print ("spawn enemy");
			my_enemy.SetActive(true);
			should_spawn = false;
			position.z = 0;
			my_enemy.transform.position = position;
		}
	}
	
}

