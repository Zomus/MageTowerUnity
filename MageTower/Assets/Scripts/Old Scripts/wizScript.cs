using UnityEngine;
using System.Collections;

public class wizScript : MonoBehaviour {
	float xAccel = 1;
	float xSpeed = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//gameObject.transform.Rotate(new Vector3(0, 30, 0)*Time.deltaTime);

		gameObject.transform.Translate(new Vector3(xSpeed, 0, 0)*Time.deltaTime);

		if(gameObject.transform.position.x > 0.5){
			xSpeed += xAccel * Time.deltaTime;
		}
		if(gameObject.transform.position.x < -0.5){
			xSpeed -= xAccel * Time.deltaTime;
		}

		if(Main.timer <= 0){
			
		}
	}
}
