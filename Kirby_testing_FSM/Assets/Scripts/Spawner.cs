using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject my_enemy;
	private Vector3 position = new Vector3 ();

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
	void OnBecameVisible(){
//		my_enemy.gameObject.SetActive (true);
//		position.z = 0;
//		my_enemy.transform.position = position;
	}

	void OnBecameInvisible(){
//		can only test this when away from the scene view in unity
		if (my_enemy == null) return; 
		print ("spawner out of sight");
		if (my_enemy.activeSelf == false) {
			print ("enemy spawn");
			my_enemy.SetActive (true);
			position.z = 0;
			my_enemy.transform.position = position;
		}
		if(my_enemy.transform.position.y < -35.46){
			my_enemy.SetActive(false);
		}
	}

}
