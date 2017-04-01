using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TrapController : MonoBehaviour{

	Ray ray;
	RaycastHit hit;

	public GameObject tileContainer;
	public GameObject tilePrefab;

	public GameObject trapContainer;
	public GameObject springTrapPrefab;

	/*public TrapController (){
		
	}*/

	void Start(){
		
	}

	void Update(){
		if (Input.GetMouseButtonUp(0)){ // on click
			int layerMask = 1 << 8;
			layerMask = ~layerMask;

			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// casts a ray from camera to the point where your mouse is hovering over
			//if (Physics.Raycast(transform.position, transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, layerMask)) {
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){ //return true if hits and return the hit object as a RaycastHit
				if (hit.collider.tag == "Trappable"){
					//PLACE TRAP
					GameObject tempTrap = Instantiate(springTrapPrefab, hit.point, Quaternion.identity, trapContainer.transform) as GameObject;
					tempTrap.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
					
				}
			}
		}
	}
}

