using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {
	//Raycasting for mouse selection
	Ray ray;
	RaycastHit hit;

	public GameObject tileContainer;
	//Empty Object that contains all spawned tiles
	public GameObject tilePrefab;
	//Prefab (blueprint) of spawned tiles

	public GameObject trapContainer;
	//Empty Object that contains all spawned traps
	public GameObject springTrapPrefab;
	//Prefab (blueprint) of spawned springs

	public Block highlightedTile;
	//Currently selected tile

	// Use this for initialization
	void Start () {
		//SPAWN ALL BLOCKS
		int height = 0;
		//represents height of blocks initialized currently
		for(int i = -14; i < 14; i++){
			for(int j = -1; j < 1; j++){
				spawnTile(i, height, j);
			}
		}

		for(int i = -2; i < 4; i++){
			for(int j = 1; j < 3; j++){
				spawnTile(i, height, j);
			}
		}

		height = 5;
		//new spawn height

		for(int i = -2; i < 4; i++){
			for(int j = 1; j < 3; j++){
				if(i == 2 && j == 2){
					break;
				}//traps cannot be placed on location where the player climbs up
				spawnTile(i, height, j);
			}
		}
		//FINISH SPAWNING BLOCKS
	}

	void spawnTile(float xPos, float yPos, float zPos){
		//Creates a single game tile at (xPos, yPos, zPos) and load it onto the screen
		GameObject tempTile = Instantiate(tilePrefab, new Vector3(xPos + 0.5f, yPos - 0.5f, zPos + 0.5f), Quaternion.identity, tileContainer.transform) as GameObject;
		tempTile.transform.localScale = new Vector3(1f, 1f, 1f);
		tempTile.AddComponent<Block>();
		//Add the Block script to let it function as a block
		tempTile.AddComponent<BoxCollider>();
		//Add a collider to the tile to allow 
	}

	// Update is called once per frame
	void Update () {
		if(highlightedTile != null){
			highlightedTile.assignNewMaterial("Default");
			highlightedTile = null;
		}//reassign default material to reset texture of selected tile every frame
		castRay();
	}

	void castRay(){
		//Casts a ray to determine if any tile is selected and perform other actions accordingly
		int layerMask = 1 << 8;
		layerMask = ~layerMask;
		//Creates a layermask that allows the ray to pass through the layer that contains the mage hand (as to not select the hand)

		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//casts a ray from camera to the point where your mouse is hovering over

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){ //return true if hits and return the hit object as a RaycastHit
			if (hit.collider.tag == "Trappable"){ //If the object hit is marked as "Trappable" by a tag
				highlightedTile = hit.collider.gameObject.GetComponent<Block>();
				//select highlighted tile
				hit.collider.gameObject.GetComponent<Block>().assignNewMaterial("GrassTop");
				//highlight the tile
			}
		}

		//CLICK TO ACTUALLY PLACE TRAP
		if (Input.GetMouseButtonUp(0)){ // on click
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){ //return true if hits and return the hit object as a RaycastHit
				if (hit.collider.tag == "Trappable" && hit.collider.gameObject.GetComponent<Block>().trapType == 0){ //Object is trappable and no trap has been placed on it
					Debug.Log("Trap placed");

					//PLACE TRAP
					hit.collider.gameObject.GetComponent<Block>().trapType = 1;
					//Mark the trap type of the block to 1 to store that a trap has been placed
					Vector3 dropLocation = hit.collider.gameObject.transform.position + new Vector3 (0f, 0.3f, 0f);
					//Define where the trap is to be placed
					GameObject tempTrap = Instantiate(springTrapPrefab, dropLocation, Quaternion.identity, trapContainer.transform) as GameObject;
					//Spawn the trap
					tempTrap.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
					//Scale the trap down
				}
			}
		}
	}
}


