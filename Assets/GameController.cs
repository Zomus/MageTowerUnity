//IMPORTS
using UnityEngine;
using System.Collections;

using System.Collections.Generic;
//Additional import (allows use of "List", which are resizable arrays)


public class GameController : MonoBehaviour {

	public GameObject enemyPrefab;
	//stores the prefab "Enemy" applied to spawned enemies

	public List<GameObject> enemyList = new List<GameObject>();

	public double runTime = 0;
	//total run time of the game, in seconds

	public double spawnRate = 1;
	//time between each enemy spawn, in seconds

	public double nextSpawn = 0;
	//time position of next enemy spawn, in seconds

	public double portalTime = 40;
	//time at which the portal opens and the wizard is able to escape, in seconds

	// Use this for initialization
	void Start () {
		//spawnEnemy ();
		//uncomment above code to spawn a single enemy at the beginning (testing purposes)
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (runTime+" "+nextSpawn+" "+spawnRate);
		runTime += Time.deltaTime;
		if (runTime >= nextSpawn) {
			spawnEnemy ();
			//Debug.Log (runTime);
			nextSpawn += spawnRate;
		}
		if(runTime >= portalTime){
			//insert win code here
		}
	}
	void spawnEnemy(int enemyType = 0){
		/*PARAMETERS:
			enemyType	= type of enemy spawned
		*/
		GameObject tempEnemy = Instantiate (enemyPrefab, new Vector3 (-10f, 1f, 0f), Quaternion.identity) as GameObject;
		//spawns an instance of an enemy

		//SETTING VARIABLES OF THE ENEMY
		tempEnemy.GetComponent<Enemy> ().moveSpeed = 10;
		tempEnemy.GetComponent<Enemy> ().state = 1;

		enemyList.Add(tempEnemy);
	}

}
