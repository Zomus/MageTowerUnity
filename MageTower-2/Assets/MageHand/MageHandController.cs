using UnityEngine;
using System.Collections;

public class MageHandController : MonoBehaviour {

	Animator anim;
	int grabHash = Animator.StringToHash("Grab");

	float cooldown = 0;
	//float timer = 0;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 3);
		//point of the mouse using screen coordinates

		Vector3 offset = new Vector3(2f, -4.5f, 4.5f);
		//offset between the coordinate offset of the (0, 0, 0) of the hand and its true center

		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
		//point of the mouse in game coordinates

		gameObject.transform.position = curPosition + offset;
		//change the gameObject's position to follow the mouse

		if(Input.GetKeyDown(KeyCode.Space)){
			anim.SetTrigger(grabHash);
			cooldown = 1;
		}
		if(cooldown <= 0){
			anim.ResetTrigger(grabHash);
		}
		else{
			cooldown -= Time.deltaTime;
		}

	}
	// Targets point when mouse left click is held.
}
