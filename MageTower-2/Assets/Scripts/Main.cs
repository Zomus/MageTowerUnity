using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

    
    public static int wizardHpMax = 10;
	public static int wizardHp = wizardHpMax;

    public static int finance = 0;
    public static int timer = 0;
    public static int stage = 1;
    public static int enemyLeft = 0;
    public static int tick = 0;

    public static bool stageStart = false;
    public static bool preSet = false;
    public static bool lv1pawn = false;
    public static bool lv2pawn = false;
    public static bool lv3pawn = false;

    bool shifter = false;

    public static List<Vector3> targetPoint = new List<Vector3>();
    public static int stageDefault = 0;
    public static int stageLimit = 0;

	public static List<GameObject> enemyList = new List<GameObject>();
	//list of references to enemies

    public GameObject enemyPrefab;

	public static List<GameObject> enemyHpBarList = new List<GameObject>();
	//parallel list of references to Hp bar of enemies

	public GameObject hpBarPrefab;

	// Use this for initialization
	void Start () {
		stagePrep();
	}
	
	// Update is called once per frame
	void Update () {

        // Creates Enemy based on Tick.
        if (tick > 4 && enemyLeft > 0)
        {
            enemyCreator();
            tick = 0;
        }

        if (timer == 0 && stageStart == true)
        {
            winFunction();
        }

		if(wizardHp < 0){
			wizardHp = 0;
			//GameObject.Find("Tower").transform.Rotate(new Vector3(45, 0, 0)*Time.deltaTime);
		}
	}

    // This function creates enemy, switching between leftside and rightside.
    void enemyCreator()
    {
        if (shifter == false)
        {
			enemyList.Add(Instantiate(enemyPrefab, new Vector3(10,0.1f,0), Quaternion.identity) as GameObject);
            shifter = true;
        } else if (shifter == true)
        {
			enemyList.Add(Instantiate(enemyPrefab, new Vector3(-10,0.1f,0), Quaternion.identity) as GameObject);
            shifter = false;
        }
		enemyHpBarList.Add(Instantiate(hpBarPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject);
    }

    // This function handles what happens after portal time reaches 0.
    public void winFunction()
    {
        stageStart = false;
        stage++;
        preSet = false;
        string stageName = "sceneStage" + stage;
        Debug.Log("Next Level Ready");
        //Play some fancy animation.
        new WaitForSeconds(10);
        //Discard current stage and move to next stage.
        //Application.Loadlevel(stageName);
    }

    void stagePrep()
    {
        if (Main.stage == 1 && Main.preSet == false)
        {
            targetPoint.Add(new Vector3(-1.05f, 0.5f, 0f));
			//slightly left of the tower
            targetPoint.Add(new Vector3(1.05f, 0.5f, 0f));
			//slightly right of the tower
            targetPoint.Add(new Vector3(-1.05f, 4.2f, 0f));
			//slightly left of the tower, but at the top
            targetPoint.Add(new Vector3(1.05f, 4.2f, 0f));
			//slightly right of the tower, but at the top
            targetPoint.Add(new Vector3(-0.7f, 4.1f, 0f));
			//slightly left of portal
            targetPoint.Add(new Vector3(0.7f, 4.1f, 0f));
			//slightly right of portal
            stageDefault = 0;
			//set the default target to 0
            stageLimit = 4;
			//target position index cannot increase larger than this amount of indices
            Main.preSet = true;
			//include guard (only runs once)
        }
    }
}
