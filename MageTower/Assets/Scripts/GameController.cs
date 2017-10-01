using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	/* ROLE:
	 * Manages game stats (main character stats, spawning) and the various operations in the game
	 * Attached to Empty GameObject
	 * Only one operational GameController active at any time
	 */ 

	//CLASS VARIABLES

	//STATIC CLASS VARIABLES
	public static GameController main;
	//gives static reference to the controller so that the single operational GameController can be accessed from anywhere by referencing this class (without obtaining its instance reference)

	//PUBLIC CLASS VARIABLES

	//IMPORTANT GAME_OBJECTS
	public GameObject pauseMenu;
	//pause menu
	public GameObject tileContainer;
	//parent GameObject that will contain all spawned tiles
	public GameObject enemyContainer;
	//parent GameObject that will contain all spawned enemies
	public GameObject trapContainer;
	//parent GameObject that contains all spawned traps
	public GameObject particleContainer;
	//parent GameObject that contains all spawned particle effects
	public GameObject ladderContainer;
	//parent GameObject that contains all ladders

	//PLAYER STATS
	public int wizardHpMax;
	//wizard's (player's) max HP
	public int wizardHp;
	//wizard's (player's) current HP
	public int finance = 0;
	//coins the player has collected


	//STAGE TIMER
	public bool stageTimerRunning;
	//whether the stage timer is counting
	public float stageTimer;
	//counts time when stage begins
	public int stageTime = 100;
	//total seconds until portal opens and player escapes


	//ENEMY SPAWNING
	public GameObject enemyPrefab;
	//Prefab (blueprint) of spawned enemies
	public int enemiesLeft;
	//enemies left to be spawned in this level
	private float lastSpawnTime;
	//time at which the last enemy spawn occured since stage start
	public float spawnRate;
	//time before another enemy spawns

	public GameObject spawnCapsule;
	//capsule in game that specifies where enemies are spawned

	public GameObject goalCapsule;
	//capsule in game that specifies where enemies move towards



	//SPAWNING TILES AND TRAPS
	public GameObject tilePrefab;
	//Prefab (blueprint) of spawned tiles

	public int trapType;
	/*Type of trap that is being placed
	 * 1 = Spring Trap
	 * 2 = Saw Trap
	*/

	public GameObject[] trapPrefabs;
	//0th element is left undefined as 0 means no trap

	public GameObject highlightedTile;
	//Currently selected tile

	//RAYCASTING - for mouse over tile selection
	Ray ray;
	RaycastHit hit;

	void Start () {
		GameController.main = this;
		//attach static reference to main so that any object can reference main

		//SPAWN ALL BLOCKS
		int height = 0;
		//represents height of blocks being created by subsequent instructions

		/* Axes represented by each variable
		 * i		= z axis (left/right)
		 * j or k	= x axis (front/back)
		 * height	= y axis (up/down)
		 */

		for(int i = -5; i < 0; i++){
			for(int j = -14; j < 14; j++){
				spawnTile(j, height, i);
			}
		}
		//^^spawn blocks at (-14 ≤ x < 14, y = 0, -5 ≤ z < 0)

		{//brackets to make sure j can be used in other places (restricts the variable j to a local variable)
			for(int j = -2; j < 0; j++){
				spawnTile(j, height, 0);
			}
			for(int k = 2; k < 4; k++){
				spawnTile(k, height, 0);
				//must use k because j was used in same brackets
			}
		}
		//^^spawn blocks at (-2 ≤ x < 0 and 2 ≤ x < 4, y = 0, z = 0)

		for(int i = 1; i < 3; i++){
			for(int j = -2; j < 4; j++){
				spawnTile(j, height, i);
			}
		}
		//^^spawn blocks at (-2 ≤ x < 4, y = 0, 1 ≤ z < 3)

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
		//^^spawn blocks at (-2 ≤ x < 4, y = 5, 1 ≤ z < 3), except at (2, 5, 2) where the ladder exists

		//***Improvement point: condense into a function to spawn adjacent groups of blocks***
	}

	void spawnTile(float xPos, float yPos, float zPos){
		/* PARAMETERS:
		 * xPos = x position where the tile will be spawned
		 * yPos = y position where the tile will be spawned
		 * zPos = z position where the tile will be spawned
		 * DOES:
		 * Creates a single tile at (xPos, yPos, zPos) and load it into the game
		 */

		Instantiate(tilePrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity, tileContainer.transform);
		//instantiates a new tile at location specified inside the tileContainer
	}

	void spawnEnemy(float xPos, float yPos, float zPos){
		/* PARAMETERS:
		 * xPos = x position where the tile will be spawned
		 * yPos = y position where the tile will be spawned
		 * zPos = z position where the tile will be spawned
		 * DOES:
		 * Creates a single enemy at (xPos, yPos, zPos) and load it into the game
		 */

		GameObject tempEnemy = Instantiate(enemyPrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity, enemyContainer.transform) as GameObject;
		//instantiates a new enemy at location specified inside the enemyContainer

		tempEnemy.GetComponent<EnemyController>().Initialize((int)goalCapsule.transform.position.y);
		//send some variables to the controller attached to the newly created enemy
	}

	void Update () {
		//KEYBOARD INPUT - allows selection of trap type
		if(Input.GetKeyDown(KeyCode.Alpha0)){
			//if 0 key is pressed
			trapType = 0;
			//no traps will be selected
		}
		else if(Input.GetKeyDown(KeyCode.Alpha1)){
			//if 1 key is pressed
			trapType = 1;
			//spring trap will be selected
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2)){
			//if 2 key is pressed
			trapType = 2;
			//saw trap will be selected
		}

		//TILE DISELECTION
		if(highlightedTile != null){
			//if a tile was previously highlighted
			highlightedTile.GetComponent<TileController>().assignNewMaterial("Default");
			//reassign default material to reset texture of selected tile every frame
			highlightedTile = null;
			//diselect the previously highlighted
		}

		//LAYERING - allows selection of tile to place traps on
		int mask = ~(1 << 8);
		//Creates a layermask that allows the ray to pass through layer 8 (layer that contains the mage hand as to not select it)

		//CASTING THE RAY - to obtain the object that the player is pointing at
		GameObject castObject = castRay(mask);
		//cast a ray based on mouse location to obtain a GameObject (through the mask)

		//TILE SELECTION
		if(castObject != null){
			//run the following only if the ray is casted on an object
			if (castObject.tag == "Trappable" && trapType != 0){
				//if the object hit is marked as "Trappable" by a tag AND trap is selected
				highlightedTile = castObject;
				//select highlighted tile
				highlightedTile.GetComponent<TileController>().assignNewMaterial("GrassTop");
				//assign a new material to the tile to show that it is highlighted

				//PLACING TRAP
				if (Input.GetMouseButtonUp(0)){
					//run this block upon clicking

					TileController selectedTile = castObject.GetComponent<TileController>();
					//obtain the TileController component of the object

					if(selectedTile.trapType == 0){
						//no trap has been placed on the tile
						Debug.Log("Trap placed");
						//print out that a trap has been placed

						//PLACE TRAP
						selectedTile.trapType = trapType;
						//mark the trap type of the block to the type of the trap
						selectedTile.ready = true;
						//note that the trap is ready to trap an enemy
						Vector3 dropLocation = castObject.transform.position;
						//define where the trap is to be placed
						GameObject tempTrap = Instantiate(trapPrefabs[trapType], dropLocation, Quaternion.identity, trapContainer.transform) as GameObject;
						//spawn the trap at the dropLocation inside the trapContainer
						selectedTile.addTrapReference(tempTrap.GetComponent<TrapController>());
						//give the tile a reference to the TrapController component of the trap that has been set on it
					}else{
						//trap has already been placed on the tile
						Debug.Log("Trap resetted");
						//print out that a trap has been resetted

						//RESET TRAP
						selectedTile.ready = true;
						//note that the trap is ready to trap an enemy again
						selectedTile.trapRef.resetTrapAnimation();
						//reset animation for the trap
					}
				}
			}
		}

		//STAGE TIMER
		if(stageTimerRunning){
			//if the timer is counting
			stageTimer += Time.deltaTime;
			//count up the timer
		}

		//ENEMY SPAWNING
		if(stageTimer - lastSpawnTime > spawnRate){
			//time elapsed is enough to spawn another enemy

			Vector3 randomOffset = new Vector3(Random.Range(-14f, 14f), 2f, -4f);
			//random offset to the spawn capsule to allow for random location spawning
			spawnEnemy(spawnCapsule.transform.position.x + randomOffset.x, spawnCapsule.transform.position.y + randomOffset.y, spawnCapsule.transform.position.z + randomOffset.z);
			//spawn the enemy
			lastSpawnTime = stageTimer;
			//set the last spawn time to now
		}

		//PAUSE MENU
		if(Input.GetKeyDown(KeyCode.P)){
			//if the 'p' key is pressed

			Time.timeScale = pauseMenu.activeInHierarchy ? 1 : 0;
			//depending on the pause menu's state, start/stop the flow of time

			pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
			//toggle state of the pauseMenu
		}
	}

	GameObject castRay(int layerMask){
		/* PARAMETERS:
		 * layerMask = bit mask that filters out which layers should be ignored by the RayCast
		 * DOES:
		 * Casts a ray from the main camera to infinity at the mouse location
		 * RETURN VALUE:
		 * Returns the first object that the Ray hits; if no object is hit, returns null
		 */

		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//casts a ray from camera to the point where the mouse is hovering over

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){
			//Physics.Raycast returns true if hits and return the hit object as a RaycastHit --> something is hit
			return hit.collider.gameObject;
			//return the object that is hit
		}
		else{
			//if nothing was hit
			return null;
			//return null
		}
	}
}


