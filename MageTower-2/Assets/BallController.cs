using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {
	private Vector3 vec3;
	// Use this for initialization
	void Start () {
		vec3 = new Vector3(0f, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey("left")){
			vec3.x = -2;
		}
		else if (Input.GetKey("right")){
			vec3.x = 2;
		}
		else{
			vec3.x = 0;
		}
		if(gameObject.transform.position.x < 2f){
			gameObject.transform.Translate(vec3 * Time.deltaTime);
		}
	}
	public Vector3 getVec3(){
		return vec3;
	}
}
