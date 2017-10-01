using UnityEngine;
using System.Collections;

public class TrapController : MonoBehaviour {
	Animator anim;
	TileController tileRef;

	void Awake () {
		anim = transform.Find("Model").GetComponent<Animator>();
	}

	public void setCrossReference(TileController tile){
		tileRef = tile;
	}

	public void resetTrapAnimation(){
		anim.SetBool("Triggered", false);
	}

	// Use this for initialization
	void OnTriggerEnter (Collider other) {
		EnemyController ec = other.GetComponent<EnemyController>();

		if(other.tag == "Enemy" && !ec.dead && tileRef.trapType != 0){
			//hitting an enemy AND enemy is not dead AND a trap is set

			anim.SetBool("Triggered", true);
			//triger trap animation

			if(tileRef.trapType == TileController.SPRING_TRAP){

				if(other.GetComponent<EnemyController>() != null){
					if(tileRef.ready || ec.levitated){
						ec.lifted();
						//let the enemy know that it is in the air

						other.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-1f, 1f), 10f, Random.Range(-1f, 1f));
						//fling it up with a force

						tileRef.ready = false;
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
