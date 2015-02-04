using UnityEngine;
using System.Collections;

public class subMenu : MonoBehaviour {

	public Vector3[] selections1;
	public GameObject star;
	public GameObject LevelMenu;
	public GameObject godMenu;
	public Vector3 startPos;
	public Vector3 endPos;
	private int index = 0;
	private int Level;

	// Use this for initialization
	void Start () {
		godMenu.SetActive (false);
		startPos = new Vector3 (0.11999f, -6.36f, -1.19f);
		endPos = new Vector3 (0.11999f, 0.94999f, -1.19f);
		gameObject.transform.position = startPos;
		star.transform.position = selections1 [1];

//		selections1 [0] = new Vector3 (2.48f, 0.22f, 1.07f);
//		selections1 [1] = new Vector3 (2.48f, -1.14f, 1.07f);
//		selections2 [2] = new Vector3 (0.81f, -1.87f, -1.17f);
//		selections2 [3] = new Vector3 (1.92f, -1.87f, -1.17f);
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.position = endPos;

		if (LevelMenu.activeSelf == true) {
			star.transform.position = selections1 [index];
						if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.DownArrow)) {
								index = (index + 1) % 2;
								star.transform.position = selections1 [index];
						}
						if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.UpArrow)) {
								index = Mathf.Abs ((index - 1) % 2);
								star.transform.position = selections1 [index];
						}
						if (Input.GetKeyDown (KeyCode.Return)) {
								if (star.transform.position == selections1 [1]) {
										print ("classic level");
										Level = 1;
										call_god(Level);
								} else if (star.transform.position == selections1 [0]) {
										print ("custom level");
										Level = 2;
										call_god (Level);
								}
						}
				}

	}

	void call_god(int level){
		if (LevelMenu.activeSelf == true) {
			godMenu.SetActive (true);
			LevelMenu.SetActive (false);
			god_menu menu = godMenu.GetComponent<god_menu>();
			menu.lvl = Level;
		}	
	}

}
