using UnityEngine;
using System.Collections;

public class book_spawn : MonoBehaviour {

	public GameObject bookPrefab;
	public bool trigger = false;
	public bool set_delay = false;
	private float toss_delay = .75f;
	private float toss_usage;
	public int count = 0;

	// Use this for initialization
	void Start () {
//		gameObject.renderer.material.color = Color.clear;
		renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (trigger) {
//		StartCoroutine (BookDrop ());
			if (!set_delay) {
				toss_usage = Time.time + toss_delay;
				set_delay = true;
				GameObject books = Instantiate (bookPrefab) as GameObject;
				bookPrefab.gameObject.transform.position = gameObject.transform.position;
			}
			if (Time.time > toss_usage) {
					set_delay = false;
			}
		}
	}

	IEnumerator BookDrop() {
		yield return new WaitForSeconds(Random.Range(1, 5));
		print ("wait for 5 seconds");
		GameObject books = Instantiate (bookPrefab) as GameObject;
		bookPrefab.gameObject.transform.position = gameObject.transform.position;
	}
}
