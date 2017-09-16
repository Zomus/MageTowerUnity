using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public static GameController main;

	public Transform pauseMenu;
	public Transform player;

	//PLAYER STATS
	public int wizardHpMax = 10;
	//player max HP
	public int wizardHp = 10;
	//player current HP

	public int finance = 0;
	//coins the player has collected
	public int stageTime = 100;
	//seconds until portal opens and player escapes
	public int stage = 1;
	//stage number for spawn arrays
	public int enemyLeft = 0;
	//enemies left to be spawned in this level

	public bool stageTimerRunning = false;
	//whether the stage timer is counting
	public float stageTimer = 0;
	//counts time when stage begins


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

	public int trapType;
	/*Type of trap that is being placed
	 * 1 = Spring Trap
	 * 2 = 
	*/

	public GameObject[] trapPrefabs;
	//0th element is left undefined as 0 means no trap

	public GameObject highlightedTile;
	//Currently selected tile

	//ENEMIES
	public GameObject enemyContainer;
	//Empty Object that contains all spawned tiles
	public GameObject enemyPrefab;
	//Prefab (blueprint) of spawned enemies

	public List<GameObject> enemyList = new List<GameObject>();
	//List of enemies that are currently on the screen (in play)
	public float lastSpawnTime;
	//time at which the last enemy spawn occured since stage start
	public float spawnRate;
	//time before another enemy spawns

	public GameObject spawnCapsule;
	//capsule in game that specifies where enemies are spawned

	public GameObject goalCapsule;
	//capsule in game that specifies where enemies move towards
	private int goalFloor;
	//specifies elevation of the goal capsule

	public GameObject ladderContainer;
	//parent GameObject that contains all ladders

	public GameObject particleContainer;
	//parent GameObject that contains all particle effects

	// Use this for initialization
	void Start () {

		GameController.main = this;
		//attach static reference to main so that any object can reference main

		pauseMenu.gameObject.SetActive(false);
		//disable pausemenu

		goalFloor = (int)goalCapsule.transform.position.y;
		//set goal floor

		stageTimerRunning = true;

		//SPAWN ALL BLOCKS
		int height = 0;
		//represents height of blocks initialized currently

		// i = z axis (left/right)
		// j = x axis (front/back)
		// height = y axis (up/down)

		//Will condense into a function to spawn adjacent groups of blocks

		for(int i = -2; i < 0; i++){
			for(int j = -14; j < 14; j++){
				spawnTile(j, height, i);
			}
		}

		{//brackets to make sure j can be used in other places (restricts the variable j to a local variable)
			for(int j = -2; j < 0; j++){
				spawnTile(j, height, 0);
			}
		}

		for(int i = 1; i < 3; i++){
			for(int j = -2; j < 4; j++){
				spawnTile(j, height, i);
			}
		}

		height = 5;
		//new spawn height

		for(int i = 1; i < 3; i++){
			for(int j = -2; j < 4; j++){
				if(!(i == 2 && j == 2)){
					//traps cannot be placed on location where the player climbs up
					//(i.e do not place block on (2, 2)
					spawnTile(j, height, i);
				}
			}
		}
		//FINISH SPAWNING BLOCKS
	}

	void spawnTile(float xPos, float yPos, float zPos){
		//FUNCTION spawnTile: Creates a single game tile at (xPos, yPos, zPos) and load it onto the screen

		Instantiate(tilePrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity, tileContainer.transform);
		/*tempTile.transform.localScale = new Vector3(1f, 1f, 1f);
		tempTile.AddComponent<TileController>();
		//Add the Block script to let it function as a block
		tempTile.AddComponent<BoxCollider>();
		//Add a collider to the tile to allow raycast collision*/
	}

	void spawnEnemy(float xPos, float yPos, float zPos){
		//FUNCTION spawnTile: Creates a single game tile at (xPos, yPos, zPos) and load it onto the screen

		GameObject tempEnemy = Instantiate(enemyPrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity, enemyContainer.transform) as GameObject;

		tempEnemy.GetComponent<EnemyController>().Initialize(goalFloor);
		//send some variables to the controller

		enemyList.Add(tempEnemy);
		//add enemy to enemyList so it can be accessed later
	}

	// Update is called once per frame
	void Update () {
		//MOUSE SELECTION
		if(highlightedTile != null){
			highlightedTile.GetComponent<TileController>().assignNewMaterial("Default");
			highlightedTile = null;
		}//reassign default material to reset texture of selected tile every frame

		castRay();


		//ENEMY SPAWNING
		if(stageTimerRunning){ //if the timer is counting
			stageTimer += Time.deltaTime;
			//count up the timer
		}
			
		if(stageTimer - lastSpawnTime > spawnRate){ //time elapsed is enough to spawn another enemy

			spawnEnemy(spawnCapsule.transform.position.x, spawnCapsule.transform.position.y, spawnCapsule.transform.position.z);
			//spawn the enemy
			lastSpawnTime = stageTimer;
			//set the last spawn time to now
		}

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
				highlightedTile = hit.collider.gameObject;
				//select highlighted tile
				hit.collider.gameObject.GetComponent<TileController>().assignNewMaterial("GrassTop");
				//highlight the tile
			}
		}

		//CLICK TO ACTUALLY PLACE TRAP
		if (Input.GetMouseButtonUp(0)){ // on click
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){ //return true if hits and return the hit object as a RaycastHit
				if (hit.collider.tag == "Trappable"){ //Object is trappable
					TileController selectedTile = hit.collider.gameObject.GetComponent<TileController>();
					//selected block/tile

					if(hit.collider.gameObject.GetComponent<TileController>().trapType == 0){ //no trap has been placed on it; set the trap
						Debug.Log("Trap placed");

						//PLACE TRAP
						selectedTile.trapType = trapType;
						//Mark the trap type of the block to the type of the trap to store that a trap has been placed
						selectedTile.ready = true;

						Vector3 dropLocation = hit.collider.gameObject.transform.position;
						//Define where the trap is to be placed
						GameObject tempTrap = Instantiate(trapPrefabs[trapType], dropLocation, Quaternion.identity, trapContainer.transform) as GameObject;
						//Spawn the trap
						selectedTile.GetComponent<TileController>().addTrapReference(tempTrap);
						//give the tile a reference to the trap that has been set on it
						//tempTrap.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
						//Scale the trap down
					}else{ //otherwise reset the trap so it can be ready again
						Debug.Log("Trap resetted");
						selectedTile.ready = true;
						selectedTile.trapRef.GetComponent<TrapController>().resetTrapAnimation();
					}


				}
			}
		}
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


