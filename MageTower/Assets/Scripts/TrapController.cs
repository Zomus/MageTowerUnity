using UnityEngine;
using System.Collections;

public class TrapController : MonoBehaviour {
	Animator anim;
	TrapController trapTile;

	void Start () {
		anim = transform.Find("Model").GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void setTileReference(TrapController tile){
		trapTile = tile;
	}

	public void triggerTrapAnimation(){
		anim.SetBool("Triggered", true);
	}
	public void resetTrapAnimation(){
		anim.SetBool("Triggered", false);
	}
}
