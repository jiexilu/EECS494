using UnityEngine;
using System.Collections;
//using UnityEditor;

public enum Col_dir {
	none,
	left, 
	right
}

public class PE_Obj : MonoBehaviour {
	public bool still = false; 
	public PE_GravType	grav = PE_GravType.constant;
	
	public Vector3 acc = Vector3.zero; 
	
	public Vector3 vel = Vector3.zero; 
	
	public Vector3 cur_pos = Vector3.zero; 
	public Vector3 next_pos = Vector3.zero; 
	
	public PE_Obj ground = null; 
	
	void Start() {
		PhysEngine.objs.Add (this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
	void OnTriggerEnter(Collider other) {
		if (still) return;
		
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return;
		
		ResolveCollisionWith(otherPEO);
	}
	
	void OnTriggerStay(Collider other) {
		OnTriggerEnter(other);
	}
	
	void OnTriggerExit(Collider other) {
		// Ignore collisions of still objects
		if (still) return;
		
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return;
		
		// This sets ground to null if we fall off of the current ground
		// Jumping will also set ground to null
		if (ground == otherPEO) {
			ground = null;
		}
	}
	
	void ResolveCollisionWith(PE_Obj col) {
		next_pos = cur_pos;
		
		Col_dir col_dir = Col_dir.none; 
		
		// get collision boundaries
		float col_x_lossy = col.transform.lossyScale.x / 2f; 
		float col_x_left = col.transform.position.x - col_x_lossy;
		float col_x_right = col.transform.position.x + col_x_lossy;
		float col_y_lossy = col.transform.lossyScale.y / 2f; 
		float col_y_down = col.transform.position.y - col_y_lossy;
		float col_y_up = col.transform.position.y + col_y_lossy;
		
		// get object boundaries
		float x_lossy = transform.lossyScale.x / 2f; 
		float y_lossy = transform.lossyScale.y / 2f; 
		float x_left = transform.position.x - x_lossy;
		float x_right = transform.position.x + x_lossy;
		float y_down = transform.position.y - y_lossy;
		float y_up = transform.position.y + y_lossy;
		
		// differences in position
		float dif_x = 0f;
		float dif_y = 0f;
		
		// collision up of kirby
		if (vel.y > 0 && col_y_down < y_up && PhysEngine.LEQ(y_up, col_y_down)) {
			//get new position 
			dif_y = Mathf.Abs (y_up - col_y_down);
			//set new position 
			next_pos.y -= dif_y;
			//set velocity to zero
			vel.y = 0f;
		} else if (vel.y < 0 && col_y_up > y_down && PhysEngine.GEQ(y_down, col_y_up)) { // collision on the bottom of kirby
			//print ("collision on the bottom");
			//get new position 
			dif_y = Mathf.Abs (y_down - col_y_up);
			//set new position 
			next_pos.y += dif_y;
			//set velocity to zero
			vel.y = 0f;
			if (ground == null) {
				ground = col; 
			}
		}
		
		// collision to the right of kirby
		if (vel.x > 0 && col != ground && !within_x_bound(x_left, x_right, col_x_left, col_x_right)) { 
			col_dir = Col_dir.right; 
			print ("collision on the right");
			//get new position 
			dif_x = Mathf.Abs (x_right - col_x_left);
			//set new position 
			next_pos.x -= dif_x;
			//set velocity to zero
			vel.x = 0f;
		} else if (vel.x < 0 && col != ground && !within_x_bound(x_left, x_right, col_x_left, col_x_right)) {
			col_dir = Col_dir.left; 
			// collision to the left of kirby
			print ("collision on the left");
			//get new position 
			dif_x = Mathf.Abs (x_left - col_x_right);
			//set new position 
			next_pos.x += dif_x;
			//set velocity to zero
			vel.x = 0f;
		} else if (col_x_left == x_right || col_x_right == x_left) {
			vel.x = 0f; 
		}
		
		float temp = cur_pos.x + 1;
		if (next_pos.x > temp) {
			print ("Big difference!!!!");
			if (col_dir == Col_dir.left) {
				print ("left");
			} else if (col_dir == Col_dir.right) {
				print ("right");
			}
		}
		
		//		print ("cur position" + cur_pos);
		//		print ("next position" + next_pos);
		cur_pos = next_pos;
		transform.position = cur_pos;
		
	}
	
	
	bool within_x_bound(float k_lx, float k_rx, float c_lx, float c_rx) {
		if (PhysEngine.GEQ(k_lx, c_lx) && PhysEngine.LEQ(k_rx, c_rx)) {
			return true;
		}
		return false;
	}
	
}
