using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public class Block : MonoBehaviour {
	public Material defaultMat;

	//TRAP TYPES
	public const int NO_TRAP = 0;
	public const int SPRING_TRAP = 1;

	public int trapType;
	//type of trap placed on this block

	// Use this for initialization
	void Start () {
		trapType = NO_TRAP;
		//start by having no trap
		tag = "Trappable";
		//allow the block to be trappable
		defaultMat = getChild("Top").GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
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

		Transform selectedChild;
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