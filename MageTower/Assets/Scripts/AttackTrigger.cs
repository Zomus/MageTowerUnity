using UnityEngine;
using System.Collections;

public class AttackTrigger : MonoBehaviour {
	
	void OnTriggerEnter (Collider other) {
		if(other.tag == "Enemy"){
			//if the enemy enters this area, it will begin attacking

			other.GetComponent<EnemyController>().attackWizard();

			//Replace destroy with attack function later
		}

	}
	

}
