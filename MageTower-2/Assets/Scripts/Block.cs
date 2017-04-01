using UnityEngine;
using System.Collections;




public class Block : MonoBehaviour {
	//TRAP TYPES
	public const int NO_TRAP = 0;
	public const int SPRING_TRAP = 1;

	public int trapType;
	//type of trap placed on this block

	// Use this for initialization
	void Start () {
		trapType = NO_TRAP;
		//start by having no trap

		assignNewMaterial("Dirt");
		tag = "Trappable";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void assignNewMaterial(string name){
		Material newMat = Resources.Load("Materials/"+name, typeof(Material)) as Material;
		gameObject.GetComponent<MeshRenderer>().material = newMat;
	}
}
