using UnityEngine;
using System.Collections;

public class TriggerTrap : MonoBehaviour {

	TileController tileRef;

	void Awake(){
		tileRef = transform.parent.GetComponent<TileController>();
	}

	// Use this for initialization
	void OnTriggerEnter (Collider other) {
		if(other.tag == "Enemy" && !other.GetComponent<EnemyController>().dead){
			
			if(tileRef.trapType == TileController.SPRING_TRAP){
				
				if(other.GetComponent<EnemyController>() != null){
					EnemyController ec = other.GetComponent<EnemyController>();
					if(tileRef.ready || ec.levitated){
						ec.lifted();
						//let the enemy know that it is in the air

						//other.GetComponent<Rigidbody>().velocity = new Vector3(1f/*Random.Range(-1f, 1f)*/, 10f, 0f/*Random.Range(-1f, 1f)*/);
						//fling it up with a force

						tileRef.ready = false;
						Debug.Log(tileRef.trapRef.GetComponent<TrapController>());
					}

				}else{
					Debug.Log("Error: Cannot find component EnemyController on other.");
				}
			}

			if(tileRef.trapType == TileController.SAW_TRAP && tileRef.ready){
				if(other.GetComponent<EnemyController>() != null){
					other.GetComponent<EnemyController> ().death ();
					//kills the enemy immediately
					tileRef.ready = false;
				}
			}

		}

	}
}
