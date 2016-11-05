using UnityEngine;
using System.Collections;

public class MageHandController : MonoBehaviour {

	private Vector3 screenPoint();
	private Vector3 offset();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	// Targets point when mouse left click is held.
	void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	// Used to drag the object.
	void OnMouseDrag()
	{
		//Debug.Log("hi");
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
		//point of the mouse using screen coordinates

		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
		//point of the mouse in game coordinates

		transform.position = curPosition;
	}
}
