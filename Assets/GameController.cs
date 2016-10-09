//IMPORTS
using UnityEngine;
using System.Collections;

using System.Collections.Generic;
//Additional import (allows use of "List", which are resizable arrays)


public class GameController : MonoBehaviour {

	public GameObject enemyPrefab;
	//stores the prefab "Enemy" applied to spawned enemies

	public List<GameObject> enemyList = new List<GameObject>();

	public double runTime;
	//total run time of the game, in seconds

	public double spawnRate;
	//time between each enemy spawn, in seconds

	public double nextSpawn;
	//time position of next enemy spawn, in seconds

	public double portalTime;
	//time at which the portal opens and the wizard is able to escape, in seconds

	// Use this for initialization
	void Start () {
		//spawnEnemy ();
		//uncomment above code to spawn a single enemy at the beginning (testing purposes)
	}
	
	// Update is called once per frame
	void Update () {
		runTime += Time.deltaTime;
		//increase the run time by deltaTime, the change in time recorded by the program between this frame and the last frame
		if (runTime >= nextSpawn) { //if the run time has reached the time position for the next spawn
			spawnEnemy ();
			//spawn an enemy
			nextSpawn += spawnRate;
			//increase the time position for the next spawn by the spawn rate
		}
		if(runTime >= portalTime){
			//insert win code here
		}
	}

	//spawnEnemy spawns one enemy onto the 
	void spawnEnemy(int enemyType = 0){
		/*PARAMETERS:
			enemyType	= type of enemy spawned
		*/
		GameObject tempEnemy = Instantiate (enemyPrefab, new Vector3 (-10f, 1f, 0f), Quaternion.identity) as GameObject;
		//spawns an instance of an enemy, at position x = -10, y = 1, z = 0 without rotation

		//SETTING INITIAL VALUES OF VARIABLES FOR THAT ENEMY INSTANCE
		tempEnemy.GetComponent<Enemy> ().moveSpeed = 10;
		tempEnemy.GetComponent<Enemy> ().state = 1;

		enemyList.Add(tempEnemy);
		//add the enemy instance to the list of enemies
	}

}
