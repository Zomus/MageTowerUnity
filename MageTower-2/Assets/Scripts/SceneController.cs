using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneController : MonoBehaviour {

	public static SceneController main;

	public Transform pauseMenu;
	public Transform player;

	//PLAYER STATS
	public static int wizardHpMax = 10;
	//player max HP
	public static int wizardHp = wizardHpMax;
	//player current HP

	public static int finance = 0;
	//coins the player has collected
	public static int timer = 0;
	//timer until portal opens and player escapes
	public static int stage = 1;
	//stage number for spawn arrays
	public static int enemyLeft = 0;
	//enemies left to be spawned in this level
	public static int tick = 0;

	public static float levelTime;
	//Time at which the level began


	//MOUSE CONTROLS
	//Raycasting for mouse selection
	Ray ray;
	RaycastHit hit;

	//TILES AND TRAPS
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

	//ENEMIES
	public GameObject enemyContainer;
	//Empty Object that contains all spawned tiles
	public GameObject enemyPrefab;
	//Prefab (blueprint) of spawned enemies

	public List<Enemy> enemyList;
	//List of enemies that are currently on the screen (in play)
	public float lastSpawnTime;
	//Time at which the last enemy spawn occured*/




	// Use this for initialization
	void Start () {

		SceneController.main = this;
		//attach static reference to main so that any object can reference main

		pauseMenu.gameObject.SetActive(false);
		//disable pausemenu

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
		//FUNCTION spawnTile: Creates a single game tile at (xPos, yPos, zPos) and load it onto the screen

		GameObject tempTile = Instantiate(tilePrefab, new Vector3(xPos + 0.5f, yPos - 0.5f, zPos + 0.5f), Quaternion.identity, tileContainer.transform) as GameObject;
		tempTile.transform.localScale = new Vector3(1f, 1f, 1f);
		tempTile.AddComponent<Block>();
		//Add the Block script to let it function as a block
		tempTile.AddComponent<BoxCollider>();
		//Add a collider to the tile to allow raycast collision
	}

	void spawnEnemy(float xPos, float yPos, float zPos){
		//FUNCTION spawnTile: Creates a single game tile at (xPos, yPos, zPos) and load it onto the screen

		GameObject tempEnemy = Instantiate(enemyPrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity, enemyContainer.transform) as GameObject;
		tempEnemy.transform.localScale = new Vector3(1f, 1f, 1f);
		tempEnemy.AddComponent<Enemy>();
		//Add the Block script to let it function as a block
		tempEnemy.AddComponent<NavMeshAgent>();
		//add a navmesh agent component to allow it to navigate around obstacles
		//tempEnemy.AddComponent<BoxCollider>();
		//Add a collider to the tile to allow 
	}

	// Update is called once per frame
	void Update () {
		//MOUSE SELECTION
		if(highlightedTile != null){
			highlightedTile.assignNewMaterial("Default");
			highlightedTile = null;
		}//reassign default material to reset texture of selected tile every frame
		castRay();


		//PAUSE MENU
		if(Input.GetKeyDown(KeyCode.P)){ //if the p key is pressed
			if(!pauseMenu.gameObject.activeInHierarchy){ //not active (paused)
				pauseMenu.gameObject.SetActive(true);
				//activate pause menu

				//player.GetComponent<MoveTo>().enabled = false;
				//disable player movement

				Time.timeScale = 0;
				//stop the flow of time
			}else{
				pauseMenu.gameObject.SetActive(false);
				//deactivate pause menu

				//player.GetComponent<MoveTo>().enabled = true;
				//enable player movement
				Time.timeScale = 1;
				//allow time to flow normally
			}
		}
	}

	void castRay(){
		//FUNCTION castRay: Casts a ray to determine if any tile is selected and perform other actions accordingly
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

	void spawnEnemy()
	{
		//FUNCTION spawnEnemy: spawns a single enemy


		/*if (shifter == false)
		{
			enemyList.Add(Instantiate(enemyPrefab, new Vector3(10,0.1f,0), Quaternion.identity) as GameObject);
			shifter = true;
		} else if (shifter == true)
		{
			enemyList.Add(Instantiate(enemyPrefab, new Vector3(-10,0.1f,0), Quaternion.identity) as GameObject);
			shifter = false;
		}*/

	}

	public void win()
	{
		//FUNCTION win: runs once when player successfully completes a level
		/*
		stageStart = false;
		stage++;
		preSet = false;
		//string stageName = "sceneStage" + stage;
		Debug.Log("Next Level Ready");
		//Play some fancy animation.
		new WaitForSeconds(10);
		//Discard current stage and move to next stage.
		//Application.Loadlevel(stageName);
		*/
	}
}


