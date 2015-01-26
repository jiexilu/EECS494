using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PE_GravType {
	none, 
	constant
}

public enum PE_Collider {
	sphere,
	aabb
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
			po.transform.position = po.pos1;
		}
		
	}
	
	
	public void TimeStep(PE_Obj po, float dt) {
		if (po.still) {
			po.pos0 = po.pos1 = po.transform.position;
			return;
		}
		
		// Velocity
		po.velRel = (po.pos1 - po.pos0) / dt;
		
		po.vel0 = po.vel;
		Vector3 tAcc = po.acc;
		switch (po.grav) {
		case PE_GravType.constant:
			tAcc += gravity;
			po.vel += tAcc * dt;
			break;
		}
		
		if (po.vel.x==0) { // Special case when po.vel.x == 0
			if (po.vel.y > 0) {
				po.dir = PE_Dir.up;
				print ("up");
			} else {
				po.dir = PE_Dir.down;
				print ("down");
			}
		} else if (po.vel.x>0 && po.vel.y>0) {
			po.dir = PE_Dir.upRight;
			print ("up right");
		} else if (po.vel.x>0 && po.vel.y<=0) {
			po.dir = PE_Dir.downRight;
			print ("down right");
		} else if (po.vel.x<0 && po.vel.y<=0) {
			po.dir = PE_Dir.downLeft;
			print ("down left");
		} else if (po.vel.x<0 && po.vel.y>0) {
			po.dir = PE_Dir.upLeft;
			print ("up left");
		}
		
		// Position
		po.pos1 = po.pos0 = po.transform.position;
		po.pos1 += po.vel * dt;
		
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
