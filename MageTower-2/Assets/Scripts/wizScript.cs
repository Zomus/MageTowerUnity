using UnityEngine;
using System.Collections;

public class wizScript : MonoBehaviour {
	float sideAccel = 1;
	float sideSpeed = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//gameObject.transform.Rotate(new Vector3(0, 30, 0)*Time.deltaTime);

		gameObject.transform.Translate(new Vector3(sideSpeed, 0, 0)*Time.deltaTime);

		if(gameObject.transform.position.x > 0.5){
			sideSpeed += sideAccel * Time.deltaTime;
		}
		if(gameObject.transform.position.x < -0.5){
			sideSpeed -= sideAccel * Time.deltaTime;
		}
	}
}
