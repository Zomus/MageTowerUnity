using UnityEngine;
using System.Collections;

public class Wizard : MonoBehaviour {

	public int maxHP;
	//maximum HP of wizard

	public int HP;
	//current HP of wizard

	// Use this for initialization
	void Start () {
		HP = maxHP;
		//fill HP to full when spawning
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
