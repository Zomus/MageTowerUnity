using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {

	Ray ray;
	RaycastHit hit;

	public GameObject tileContainer;
	public GameObject tilePrefab;

	public GameObject trapContainer;
	public GameObject springTrapPrefab;

	public Block highlightedTile;
	//Currently selected tile

	// Use this for initialization
	void Start () {
		int height = 0;
		//represents height of blocks initialized currently
		for(int i = -14; i < 14; i++){
			for(int j = -1; j < 1; j++){
				//Debug.Log(tileContainer+" "+i+" "+j);
				//Create game tile and load it onto the screen
				GameObject tempTile = Instantiate(tilePrefab, new Vector3(i + 0.5f, height, j + 0.5f), Quaternion.identity, tileContainer.transform) as GameObject;
				tempTile.transform.localScale = new Vector3(0.8f, 0.5f, 0.8f);
				tempTile.AddComponent<Block>();
				tempTile.AddComponent<BoxCollider>();
				//Add the Block script to let it function as a block
			}
		}

		for(int i = -2; i < 4; i++){
			for(int j = 1; j < 3; j++){
				//Debug.Log(tileContainer+" "+i+" "+j);
				//Create game tile and load it onto the screen
				GameObject tempTile = Instantiate(tilePrefab, new Vector3(i + 0.5f, height, j + 0.5f), Quaternion.identity, tileContainer.transform) as GameObject;
				tempTile.transform.localScale = new Vector3(0.8f, 0.5f, 0.8f);
				tempTile.AddComponent<Block>();
				tempTile.AddComponent<BoxCollider>();
				//Add the Block script to let it function as a block
			}
		}

		height = 5;

		for(int i = -2; i < 4; i++){
			for(int j = 1; j < 4; j++){
				//Debug.Log(tileContainer+" "+i+" "+j);
				//Create game tile and load it onto the screen
				if(i == 2 && j == 2){
					j++;
				}//traps cannot be placed on location where the player climbs up
				GameObject tempTile = Instantiate(tilePrefab, new Vector3(i + 0.5f, height, j + 0.5f), Quaternion.identity, tileContainer.transform) as GameObject;
				tempTile.transform.localScale = new Vector3(0.8f, 0.5f, 0.8f);
				tempTile.AddComponent<Block>();
				tempTile.AddComponent<BoxCollider>();
				//Add the Block script to let it function as a block
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if(highlightedTile != null){
			highlightedTile.assignNewMaterial("Dirt");
			highlightedTile = null;
		}
		placeTrap();
	}

	void assignNewMaterial(string name){
		Material newMat = Resources.Load("Materials/"+name, typeof(Material)) as Material;
		gameObject.GetComponent<MeshRenderer>().material = newMat;
	}

	void placeTrap(){
		//Selection
		int layerMask = 1 << 8;
		layerMask = ~layerMask;

		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		// casts a ray from camera to the point where your mouse is hovering over

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){ //return true if hits and return the hit object as a RaycastHit
			if (hit.collider.tag == "Trappable"){
				//Highlight block
				highlightedTile = hit.collider.gameObject.GetComponent<Block>();
				//select highlighted tile
				hit.collider.gameObject.GetComponent<Block>().assignNewMaterial("Grass");
			}
		}

		//CLICK TO ACTUALLY PLACE TRAP
		if (Input.GetMouseButtonUp(0)){ // on click
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){ //return true if hits and return the hit object as a RaycastHit
				if (hit.collider.tag == "Trappable" && hit.collider.gameObject.GetComponent<Block>().trapType == 0){ //Object is trappable and no trap has been placed on it
					Debug.Log("hi");
					//Place
					hit.collider.gameObject.GetComponent<Block>().trapType = 1;
					GameObject tempTrap = Instantiate(springTrapPrefab, hit.collider.gameObject.transform.position, Quaternion.identity, trapContainer.transform) as GameObject;
					tempTrap.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
				}
			}
		}
	}
}


