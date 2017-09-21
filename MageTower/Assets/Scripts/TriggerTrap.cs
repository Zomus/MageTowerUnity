using UnityEngine;
using System.Collections;

public class TriggerTrap : MonoBehaviour {

	TileController tileRef;

	void Awake(){
		tileRef = transform.parent.GetComponent<TileController>();
	}

	// Use this for initialization
	void OnTriggerEnter (Collider other) {
		if(other.tag == "Enemy"){
			
			if(tileRef.trapType == TileController.SPRING_TRAP){
				
				if(other.GetComponent<EnemyController>() != null){
					EnemyController ec = other.GetComponent<EnemyController>();
					if(tileRef.ready){
						ec.lifted();
						//let the enemy know that it is in the air

						other.GetComponent<Rigidbody>().velocity = new Vector3(0f, 10f, 0f);
						//fling it up with a force

						tileRef.ready = false;
						Debug.Log(tileRef.trapRef.GetComponent<TrapController>());
						tileRef.trapRef.GetComponent<TrapController>().triggerTrapAnimation();
					}/*else if(ec.levitated){
						other.GetComponent<Rigidbody>().velocity = new Vector3(0f, 10f, 0f);
					}*/

				}else{
					Debug.Log("Error: Cannot find component EnemyController on other.");
				}
			}

			if(tileRef.trapType == TileController.SAW_TRAP && tileRef.ready){
				if(other.GetComponent<EnemyController>() != null){
					other.GetComponent<EnemyController> ().death ();
					//kills the enemy immediately
					tileRef.ready = false;
					tileRef.trapRef.GetComponent<TrapController>().triggerTrapAnimation();
				}
			}

		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
