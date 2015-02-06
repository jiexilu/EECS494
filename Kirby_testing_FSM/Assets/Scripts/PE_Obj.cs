using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public Col_dir col_dir = Col_dir.none;
	
	public bool is_under_water = false;
	BoxCollider box;
	private float big_difference = 0.5f;
	
	
	void Start() {
		PhysEngine.objs.Add (this);
	}
	
	void OnTriggerEnter(Collider other) {
		if (still) return;
		
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return;
		
		ResolveCollisionWith (otherPEO);
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
		if (otherPEO.CompareTag("Pool") && is_under_water) {
			is_under_water = false;
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
		box = GetComponent<BoxCollider> ();
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
		
		if (col.name == "Cube" && CompareTag("Player")) {
			print ("I'm colliding with cube");
			print ("velocity " + vel);
		}
		
		// collision up of kirby
		if (vel.y > 0 && PhysEngine.GEQ(y_up, col_y_down)) {
			//get new position 
			dif_y = Mathf.Abs (y_up - col_y_down);
			//set new position 
			if (dif_y < big_difference) {
				next_pos.y -= dif_y;
				//set velocity to zero
				vel.y = 0f;
			}
		} else if (vel.y <= 0) { // collision on the bottom of kirby
			//			if (CompareTag("Player")) {
			//				print("stop");
			//			}
			if (col.CompareTag("Slope")) {
				Slope_resolution(col);
			} else if (col.CompareTag("Pool")) {
				if (gameObject.CompareTag("Enemy")) {
					Enemy_1 enemy = gameObject.GetComponent<Enemy_1> ();
					enemy.hit_water = true; 
					return;
				}
				Water_resolution(col);
			} else if (PhysEngine.LEQ(y_down, col_y_up))  {
				//get new position 
				dif_y = Mathf.Abs (y_down - col_y_up);
				//set new position 
				if (dif_y < big_difference) {
					next_pos.y += dif_y;
					//set velocity to zero
					vel.y = 0f;
					ground = col;
				}
			}
		}
		
		// collision to the right of kirby
		if (vel.x > 0) {  
			if (col.CompareTag("Slope")) {
				col_dir = Col_dir.right;
				Slope_resolution(col);
			} else if (col != ground && !within_x_bound(x_left, x_right, col_x_left, col_x_right)) {
				if (gameObject.CompareTag("Enemy")) {
					Enemy_1 enemy = gameObject.GetComponent<Enemy_1> ();
					enemy.hit_cube = (col.gameObject.CompareTag("Ground")) ? true : false; 
					enemy.hit_wall = (col.gameObject.CompareTag("Wall")) ? true : false; 
				}
				col_dir = Col_dir.right;
				//get new position 
				dif_x = Mathf.Abs (x_right - col_x_left);
				if (dif_x < big_difference) {
					//set new position 
					next_pos.x -= dif_x;
					//set velocity to zero
					vel.x = 0f;
				}
			}
		} else if (vel.x < 0) { 
			// collision to the left of kirby
			if (col.CompareTag("Slope")) {
				col_dir = Col_dir.left;
				Slope_resolution(col);
			} else if (col != ground && !within_x_bound(x_left, x_right, col_x_left, col_x_right)) {
				if (gameObject.CompareTag("Enemy")) {
					Enemy_1 enemy = gameObject.GetComponent<Enemy_1> ();
					enemy.hit_cube = (col.gameObject.CompareTag("Ground")) ? true : false; 
					enemy.hit_wall = (col.gameObject.CompareTag("Wall")) ? true : false; 
				}
				col_dir = Col_dir.left;
				//get new position 
				dif_x = Mathf.Abs (x_left - col_x_right);
				if (dif_x < big_difference) {
					//set new position 
					next_pos.x += dif_x;
					//set velocity to zero
					vel.x = 0f;
				}
			}
		} else if (col_x_left == x_right || col_x_right == x_left) {
			vel.x = 0f; 
		}
		
		//		float temp = Mathf.Abs(Vector3.Distance(next_pos, cur_pos));
		//		if (temp >= 0.5f) {
		//			print ("Big difference!!!!");
		//			return;
		//		}
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
		// How would I make this universal so it applies to the enemies and kirby
		Direction cur_dir;
		Vector3 BR;
		Vector3 BL;
		
		if (CompareTag("Player")) {
			Kirby kirby = gameObject.GetComponent<Kirby> ();
			cur_dir = kirby.cur_dir;
			BR = kirby.BR.position;
			BL = kirby.BL.position;
		} else {
			if (!CompareTag("Enemy")) return; // error checking
			Enemy_1 enemy = gameObject.GetComponent<Enemy_1> ();
			if (enemy == null) return; 
			cur_dir = enemy.cur_dir;
			BR = enemy.BR.position;
			BL = enemy.BL.position;
		}
		Slope_pts col_slope = col.gameObject.GetComponent<Slope_pts> ();
		float slope_angle = col.transform.eulerAngles.z; 
		//		print ("euler Angle " + slope_angle);
		Vector3 col_p0; // bottom point on slope
		Vector3 col_p1; // top point on slope
		Vector3 p2; // object intersect position
		
		if (slope_angle < 90f) {
			col_p0 = col_slope.TL.position;
			col_p1 = col_slope.TR.position;
			p2 = BR;
		} else { //  if (slope_angle > 90f) 
			//			print ("slope angle is greater than 90f");
			col_p0 = col_slope.BL.position;
			col_p1 = col_slope.BR.position;
			p2 = BL;
		}
		
		Vector3 v1 = col_p1 - col_p0;
		Vector3 v2 = p2 - col_p0;
		Debug.DrawLine (col_p0, col_p1, Color.red, 10f);
		Debug.DrawLine (p2, col_p0, Color.black, 10f);
		v1.Normalize();
		float dot_result = Vector3.Dot(v1, v2);
		Vector3 projection = (v1 * dot_result) + col_p0;
		Debug.DrawLine (col_p0, projection, Color.green, 10f);
		
		// Use interpolation to find pQx
		// Find the percentage of distance along [p1…p0]
		float u = (p2.y - col_p0.y) / (col_p1.y - col_p0.y);
		Vector3 x_movement = (1-u)*col_p0 + u*col_p1;
		
		// Use interpolation to find pQy
		u = (p2.x - col_p0.x) / (col_p1.x - col_p0.x);
		Vector3 y_movement = (1-u)*col_p0 + u*col_p1;
		
		float y_dist = Vector3.Distance (y_movement, p2);
		float x_dist = Vector3.Distance (x_movement, p2);
		
		Debug.DrawLine (p2, x_movement, Color.yellow, 10f);
		Debug.DrawLine (p2, y_movement, Color.blue, 10f);
		
		//		print ("X dist " + x_dist);
		//		print ("Y dist " + y_dist);
		//		print ("current pos " + next_pos);
		if (slope_angle < 90f && cur_dir == Direction.right) {
			next_pos.y += y_dist;
		} else if (slope_angle < 90f && cur_dir == Direction.left) {
			next_pos.x -= x_dist;
		} else if (slope_angle > 90f && cur_dir == Direction.right) {
			next_pos.x += x_dist;
		} else { // slope_angle > 90f && cur_dir == Direction.left
			next_pos.y += y_dist;
		}
		
		//		print ("new pos " + next_pos);
		//set velocity to zero
		vel.y = 0f;
		ground = col; 
	}
	
	void Water_resolution(PE_Obj col) {
		// if it's an enemy, make sure it's underwater tag is set to true 
		// so it can be deleted once it hits the bottom
		if (!CompareTag("Player")) return;
		Kirby kirby = gameObject.GetComponent<Kirby> ();
		if (kirby.is_floating) {
			// handle this just as if it was ground
			float col_y_lossy = col.transform.lossyScale.y / 2f; 
			float col_y_up = col.transform.position.y + col_y_lossy;
			
			// get object boundaries
			Vector3 box_size = box.size;
			float y_lossy = (transform.lossyScale.y / 2f) * box_size.y; 
			float y_down = transform.position.y - y_lossy;
			
			//set new position 
			next_pos.y += Mathf.Abs (y_down - col_y_up);
			//set velocity to zero
			vel.y = 0f;
		} else {
			is_under_water = true; 
		}
	}
	
}
