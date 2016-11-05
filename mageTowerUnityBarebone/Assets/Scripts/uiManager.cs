using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class uiManager : MonoBehaviour {

    public Text hitPoint;
    public Text timer;
    public Text finance;

	public Transform canvas;

    // Use this for initialization
    void Start()
    {
		canvas = GameObject.Find("UI").GetComponent<Transform>();
		Debug.Log(canvas.position.x);
		float scale = 0.008f;
		canvas.localScale = new Vector3(scale, scale, scale);
		canvas.transform.Translate(canvas.position.x + 4.0f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
		//UPDATES ALL HUDs
		string moneyCount = Main.finance + " Golds";
        string healthCount = Main.wizardHp + " / 12";
        string timeCount = "Click Portal to Start";

        if (Main.stageStart == false)
        {
            timeCount = "Click Portal to Start";
        } else if (Main.stageStart == true)
        {
            timeCount = Main.timer + "sec";
        }

        hitPoint.text = healthCount;
        timer.text = timeCount;
        finance.text = moneyCount;
    }
}
