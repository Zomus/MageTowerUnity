using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public class TileController : MonoBehaviour {
	private Material defaultMat;

	//TRAP TYPES
	public const int NO_TRAP = 0;
	public const int SPRING_TRAP = 1;

	public int trapType;
	//type of trap placed on this block

	public GameObject trapRef;
	//reference to the trap that has been placed on this tile

	public bool ready = false;
	//whether trap has been triggered
	//true = ready to kill another enemy
	//false = not ready to kill (must be reset by the hand)

	// Use this for initialization
	void Start () {
		trapType = NO_TRAP;
		//start by having no trap
		tag = "Trappable";
		//allow the block to be trappable
		defaultMat = getChild("Top").GetComponent<MeshRenderer>().material;
		//set default material to the material of the top side (to turn back later)
	}

	public void addTrapReference(GameObject trap){
		trapRef = trap;
	}

	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Enemy"){ //block hits an enemy
			if(other.GetComponent<Rigidbody>().velocity.y < 0 && other.GetComponent<EnemyController>() != null){
				other.GetComponent<EnemyController>().landed((int)transform.position.y);
				//let the enemy know that it has landed
			}
		}

	}

	/*public void assignNewMaterial(string name){
		Material newMat = Resources.Load("Materials/"+name, typeof(Material)) as Material;
		gameObject.GetComponent<MeshRenderer>().material = newMat;
	}*/
	public void assignNewMaterial(string name){
		if(name != "Default"){ //if parameter 'name' is not "Default"
			Material newMat = Resources.Load("Materials/"+name, typeof(Material)) as Material;
			//Look up newly assigned material in resources folder
			getChild("Top").GetComponent<MeshRenderer>().material = newMat;
			//assign new material
		}else{
			getChild("Top").GetComponent<MeshRenderer>().material = defaultMat;
			//assign default material if parameter 'name' is "Default"
		}
	}

	public Transform getChild(string name){
		//getChild obtains the Transform of a child in this GameObject with the name 'name'

		//Transform selectedChild;
		//Transform of the selected child

		foreach (Transform child in transform){ //for loop searches through the parent GameObject's Transform
			if (child.name == name){ //if names match
				return child;
				//return that the child's reference
			}
		}

		Debug.Log("Error: Child cannot be found.");
		return null;
	}
}