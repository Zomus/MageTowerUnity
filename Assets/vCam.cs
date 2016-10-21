using UnityEngine;
using System.Collections;


//Class code for the camera
public class vCam : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		followObject (GameObject.Find ("Tower"), 10);
		//test the vCam by uncommenting the above line
	}

	//followObject makes the camera track objects (perhaps mouse later)
	void followObject (GameObject obj, int followFactor = 1){
		/*PARAMETERS:
			obj 			= object that this vCam will follow
			followFactor	= how tightly the camera will follow the object (this number must be 1 or larger; 1 follows tight, infinity follows infinitely loosely)
		*/

		Vector2 objPos = new Vector2 (0, 0);
		//instantiate a new 2D vector with values [0, 0];

		objPos.x += (obj.transform.position.x - this.gameObject.transform.position.x) / followFactor;
		//calculates the camera's horizontal position (x) with respect to the object's horizontal position
		objPos.y += (obj.transform.position.y - this.gameObject.transform.position.y) / followFactor;
		//calculates the camera's vertical position (y) with respect to the object's vertical position

		this.gameObject.transform.Translate (objPos);
		//move the camera depending on its position with respect to the object
	}
}
