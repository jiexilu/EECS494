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
	public Col_dir col_dir; 
	
	void Start() {
		PhysEngine.objs.Add (this);
		col_dir = Col_dir.none; 
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
		
		// get collision boundaries
		float col_x_lossy = col.transform.lossyScale.x / 2f; 
		float col_x_left = col.transform.position.x - col_x_lossy;
		float col_x_right = col.transform.position.x + col_x_lossy;
		float col_y_lossy = col.transform.lossyScale.y / 2f; 
		float col_y_down = col.transform.position.y - col_y_lossy;
		float col_y_up = col.transform.position.y + col_y_lossy;
		
		// get object boundaries
		BoxCollider box = GetComponent<BoxCollider> ();
		Vector3 box_size = box.size;
		float x_lossy = (transform.lossyScale.x / 2f) * box_size.x; 
		float y_lossy = (transform.lossyScale.y / 2f) * box_size.y; 
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
			if (col.CompareTag("Slope")) {
				print ("slope col down");
				Slope_resolution(col);
			} else {
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
		}
		
		// collision to the right of kirby
		if (vel.x > 0 && col != ground && !within_x_bound(x_left, x_right, col_x_left, col_x_right)) {  
			col_dir = Col_dir.right;
			if (col.CompareTag("Slope")) {
				print ("slope col right");
				Slope_resolution(col);
			} else {
				//get new position 
				dif_x = Mathf.Abs (x_right - col_x_left);
				//set new position 
				next_pos.x -= dif_x;
				//set velocity to zero
				vel.x = 0f;
			}
		} else if (vel.x < 0 && col != ground && !within_x_bound(x_left, x_right, col_x_left, col_x_right)) {
			col_dir = Col_dir.left; 
			// collision to the left of kirby
			if (col.CompareTag("Slope")) {
				print ("slope col left");
				//Slope_resolution(col);
			} else {
				//get new position 
				dif_x = Mathf.Abs (x_left - col_x_right);
				//set new position 
				next_pos.x += dif_x;
				//set velocity to zero
				vel.x = 0f;
			}
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
	
	void Slope_resolution(PE_Obj col) {
		grav = PE_GravType.none;
		Vector3 col_center = col.collider.bounds.center;
		Vector3 col_ext = col.collider.bounds.extents;
		float horiz_len = Mathf.Sqrt(Mathf.Pow(col_ext.x, 2f) + Mathf.Pow(col_ext.y, 2f));
		float rot_deg = col.transform.rotation.eulerAngles.z;
		float rot_rad = rot_deg * Mathf.Deg2Rad;
		float vert_len = horiz_len * Mathf.Sin(rot_rad);
		print ("col_ext y " + col_ext.y);
		print ("col_ext x " + col_ext.x);
		print ("horiz_len " + horiz_len);
		print ("vert_len " + vert_len);
		Vector3 col_p0 = new Vector3(col_center.x - horiz_len, col_center.y - vert_len, 0f);
		Vector3 col_p1 = new Vector3(col_center.x + horiz_len, col_center.y + vert_len, 0f);
		Vector3 p2 = new Vector3(collider.bounds.center.x + collider.bounds.extents.x, 
		                         collider.bounds.center.y - collider.bounds.extents.y, 0f);
		Vector3 v1 = col_p1 - col_p0;
		Vector3 v2 = p2 - col_p0;
		Debug.DrawLine (col_p0, col_p1, Color.yellow, 1000f);
		Debug.DrawLine (p2, col_p0, Color.white, 1000f);
		v1.Normalize();
		float dot_result = Vector3.Dot(v1, v2);
		Vector3 projection = (v1 * dot_result) + col_p0;
		print ("rot_deg " + rot_deg); 
		print ("point0 " + col_p0);
		print ("point1 " + col_p1);
		print ("point2 " + p2);
		print ("v1 " + v1);
		print ("v2 " + v2);
		print ("this is the projection " + projection);
		
		// Use interpolation to find pQx
		// Find the percentage of distance along [p1…p0]
		float u = (p2.y - col_p0.y) / (col_p1.y - col_p0.y);
		Vector3 x_movement = (1-u)*col_p0 + u*col_p1;
		
		// Use interpolation to find pQy
		u = (p2.x - col_p0.x) / (col_p1.x - col_p0.x);
		Vector3 y_movement = (1-u)*col_p0 + u*col_p1;
		
		print ("X movement " + x_movement);
		print ("Y movement " + y_movement);
		print ("current pos " + next_pos);
		//		next_pos.y += Mathf.Abs(projection.y - cur_pos.y);
		//		next_pos.x += Mathf.Abs(projection.x - cur_pos.x);
		next_pos.y += Mathf.Abs(y_movement.y - cur_pos.y);
		//next_pos.x += Mathf.Abs(x_movement.x - cur_pos.x);
		//print ("result " + result);
		print ("new pos " + next_pos);
		//set velocity to zero
		vel.y = 0f;
		ground = col; 
	}
	
}
