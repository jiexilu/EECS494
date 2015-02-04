using UnityEngine;
using System.Collections;

public class god_menu : MonoBehaviour {

	public Vector3[] selections2;
	public GameObject star;
	private int index = 0;
	public int lvl;

	// Use this for initialization
	void Start () {
		star.transform.position = selections2 [0];	
	}
	
	// Update is called once per frame
	void Update () {
		level (lvl);
	}

	public void level(int level){
		print ("level " + level);
		if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.DownArrow)) {
			index = (index + 1) % 2;
			print ("index " + index);
			star.transform.position = selections2[index];
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.UpArrow)) {
			index = Mathf.Abs((index - 1) % 2);
			print ("index " + index);
			star.transform.position = selections2[index];
		}
		
		GameObject go = GameObject.Find("godMode");
		godMode god = go.GetComponent<godMode>();
		if (star.transform.position == selections2 [index]) {
			print ("god mode on");
			god.god_mode = true;
			if (Input.GetKeyDown (KeyCode.Return)){
				print ("return");
				Application.LoadLevel (level);
			}
		}
		else if(star.transform.position == selections2[index]){
			print ("god mode off");
			god.god_mode = false;
			if(Input.GetKeyDown(KeyCode.Return)){
				print ("return");
				Application.LoadLevel (level);
			}
		}
	}
}
