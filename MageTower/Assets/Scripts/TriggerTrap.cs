﻿using UnityEngine;
using System.Collections;

public class TriggerTrap : MonoBehaviour {

	TileController tileRef;

	void Awake(){
		tileRef = transform.parent.GetComponent<TileController>();
	}

	// Use this for initialization
	void OnTriggerEnter (Collider other) {
		if(other.tag == "Enemy"){
			
			if(tileRef.trapType == TileController.SPRING_TRAP && tileRef.ready){
				if(other.GetComponent<EnemyController>() != null){
					other.GetComponent<EnemyController>().lifted();
					//let the enemy know that it is in the air

					other.GetComponent<Rigidbody>().velocity = new Vector3(0f, 10f, 0f);
					//fling it up with a force

					tileRef.ready = false;
					Debug.Log(tileRef.trapRef.GetComponent<TrapController>());
					tileRef.trapRef.GetComponent<TrapController>().playTrapAnimation();
					//Debug.Log();
					//tileRef.trapRef.GetComponent<Animator>().SetBool("Triggered", true);
				}else{
					Debug.Log("Error: Cannot find component EnemyController on other.");
				}
			}

		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}