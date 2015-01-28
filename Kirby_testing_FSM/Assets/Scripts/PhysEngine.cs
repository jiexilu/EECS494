using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PE_GravType {
	none, 
	constant
}

public enum PE_Dir { // The direction in which the PE_Obj is moving
	still,
	up,
	down,
	upRight,
	downRight,
	downLeft,
	upLeft
}

public class PhysEngine : MonoBehaviour {
	static public List<PE_Obj>	objs;
	
	static public float			closeEnough = 0.1f;
	
	public Vector3		gravity = new Vector3(0,-5f,0);
	
	// Use this for initialization
	void Awake() {
		objs = new List<PE_Obj>();
	}
	
	
	void FixedUpdate () {
		// Handle the timestep for each object
		float dt = Time.fixedDeltaTime;
		foreach (PE_Obj po in objs) {
			TimeStep(po, dt);
		}
		
		// Resolve collisions
		
		
		// Finalize positions
		foreach (PE_Obj po in objs) {
			po.cur_pos = po.transform.position = po.next_pos;
		}
		
	}
	
	
	public void TimeStep(PE_Obj po, float dt) {
		if (po.still) {
			po.next_pos = po.cur_pos = po.transform.position;
			return;
		}
		
		// Velocity
		
		Vector3 tAcc = po.acc;
		if (po.grav == PE_GravType.constant) {
			tAcc += gravity;
			po.vel += tAcc * dt;
		}
		
		//		print ("acc position" + po.acc);
		//		print ("vel position" + po.vel);
		
		// Position
		po.next_pos = po.transform.position;
		po.next_pos += po.vel * dt;
		//		print ("y velocity " + po.vel.y);
		//		print ("Acc " + tAcc.y);
		
	}
	
	// Static equality functions to deal with floating point math errors
	static public bool EQ(float f0, float f1) {
		if ( Mathf.Abs(f1-f0) <= closeEnough ) {
			return( true );
		}
		return( false );
	}
	
	static public bool LEQ(float f0, float f1) {
		if ( f0 < f1 || Mathf.Abs(f1-f0) <= closeEnough ) {
			return( true );
		}
		return( false );
	}
	
	static public bool GEQ(float f0, float f1) {
		if ( f0 > f1 || Mathf.Abs(f1-f0) <= closeEnough ) {
			return( true );
		}
		return( false );
	}
}
