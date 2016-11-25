using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class uiManager : MonoBehaviour {

    public Text hitPoint;
    public Text timer;
    public Text finance;
	public Slider hitPointSlider;

    // Use this for initialization
    void Start()
    {
		hitPointSlider.maxValue = Main.wizardHpMax;
    }

    // Update is called once per frame
    void Update()
    {
        string moneyCount = Main.finance + " Golds";
        string healthCount = Main.wizardHp + " / " + Main.wizardHpMax;
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
		hitPointSlider.value = Main.wizardHp;
    }

}
