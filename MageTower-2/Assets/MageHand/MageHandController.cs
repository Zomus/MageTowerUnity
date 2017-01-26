using UnityEngine;
using System.Collections;

public class MageHandController : MonoBehaviour {

	Animator anim;
	//int grabHash = Animator.StringToHash("Grab");

	float cooldown = 0;
	//float timer = 0;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -4);
		//point of the mouse using screen coordinates

		Vector3 offset = new Vector3(1.5f, -3.5f, 4.5f);
		//offset between the coordinate offset of the (0, 0, 0) of the hand and its true center

		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
		//point of the mouse in game coordinates

		gameObject.transform.position = curPosition + offset;
		//change the gameObject's position to follow the mouse

		if(gameObject.transform.position.y < 0f){
			gameObject.transform.Translate(new Vector3(0f, -gameObject.transform.position.y, 0f));
			anim.SetBool("Walking", true);
		}else{
			anim.SetBool("Walking", false);
		}


		if(Input.GetMouseButtonDown(0)){
			anim.SetBool("Grabbing", true);
			cooldown = 1;
		}
		if(cooldown < 0){
			anim.SetBool("Grabbing", false);
		}

		else{
			cooldown -= Time.deltaTime * 2;
		}
		//Debug.Log("Grabbing: "+anim.GetBool("Grabbing"));
	}
	// Targets point when mouse left click is held.
}

/*
if (Input.GetMouseButtonDown(0))
	Debug.Log("Pressed left click.");

if (Input.GetMouseButtonDown(1))
	Debug.Log("Pressed right click.");

if (Input.GetMouseButtonDown(2))
	Debug.Log("Pressed middle click.");
*/