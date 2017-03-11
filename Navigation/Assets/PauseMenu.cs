using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public Transform canvas;
	public Transform player;

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.P)){
			if(!canvas.gameObject.activeInHierarchy){
				canvas.gameObject.SetActive(true);
				player.GetComponent<MoveTo>().enabled = false;
				Time.timeScale = 0;

			}else{
				canvas.gameObject.SetActive(false);
				player.GetComponent<MoveTo>().enabled = true;
				Time.timeScale = 1;
			}
		}
	}
}
