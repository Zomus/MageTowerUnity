using UnityEngine;
using System.Collections;

public class TrapController : MonoBehaviour {
	Animator anim;
	TrapController trapTile;

	void Start () {
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void setTileReference(TrapController tile){
		trapTile = tile;
	}
}
