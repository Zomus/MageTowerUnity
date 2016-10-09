using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int maxHP;
	//maximum HP of enemy

	public int HP;
	//current HP of enemy

	public double attack;
	//attack stat of enemy

	public float moveSpeed;
	//walking speed of enemy

	public float climbSpeed;
	//climbing speed of enemy

	public int state;
	/*state of the enemy
	0 = not moving
	1 = walking
	2 = climbing
	*/

	// Use this for initialization
	void Start () {
		HP = maxHP;
		//fill HP to full when spawning
	}
	
	// Update is called once per frame
	void Update () {
		if (state == 1) { //if walking
			this.gameObject.transform.Translate (new Vector2 (moveSpeed, 0) * Time.deltaTime);
		}
		if (state == 2) { //if climbing
			this.gameObject.transform.Translate (new Vector2 (0, climbSpeed) * Time.deltaTime);
		}

		if (this.gameObject.transform.position.x > 8 || HP <= 0){ //when it goes out of bounds or dies
			die ();
			//remove the enemy
		}
	}

	//die destroys this enemy instance completely and erases all traces of its existence
	void die(){
		GameController gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
		gameController.enemyList.Remove (this.gameObject);
		//removes this enemy from the list of enemies in the game controller
		
		Destroy (this.gameObject);
		//destroy the gameObject this script is attached to
		Destroy (this);
		//destroy this script
	}
}
