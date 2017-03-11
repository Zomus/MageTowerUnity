using System;
using System.Collections;
using System.Collections.Generic;

public class TrapController{
	public TrapController (){
		
	}
	void Update(){
		if (Input.GetMouseButtonDown(0)){ // on click
			int layerMask = 1 << 8;
			layerMask = ~layerMask;

			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// casts a ray from camera to the point where your mouse is hovering over
			//if (Physics.Raycast(transform.position, transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, layerMask)) {
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){ //return true if hits and return the hit object as a RaycastHit
				if (hit.collider.tag == "Trappable"){
					//PLACE TRAP
				}
			}
	}
}