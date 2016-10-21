using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    public static int wizardHp = 12;
	//HP of wizard

    public static int finance = 0;
	//Money collected

    public static int timer = 0;
	//time (in seconds) since the level has started

    public static int stage = 1;
	//level

    public static int enemyLeft = 0;
	//remaining enemies to be spawned

    public static int tick = 0;
	//counts up in seconds, and when it hits 4, will spawn a new enemy

    public static bool stageStart = false;
	//has the stage started yet?

    public static bool lv1pawn = false; //UNUSED
	public static bool lv2pawn = false; //UNUSED
	public static bool lv3pawn = false; //UNUSED

    bool shifter = false;
	//if shiter == true, then enemies spawn on left side of screen, otherwise, spawn on right

    public GameObject enemyPrefab;
	//defines the prefab (model) for enemmies

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        // Creates Enemy based on Tick.
        if (tick > 4 && enemyLeft > 0) //when tick counts to 4 and there are remaining enemies to be spawned
        {
            enemyCreator();
			//spawn an enemy
            tick = 0;
			//set tick back to 0 so it can start counting up again
        }

		if (timer == 0 && stageStart == true) //if the timer counts down to 0 (portal is open) and the stage has begun
        {
            winFunction();
			//win
        }
	}

    // This function creates enemy, switching between leftside and rightside.
    void enemyCreator()
    {
        if (shifter == false)
        {
            Instantiate(enemyPrefab, new Vector3(10,0.1f,0), Quaternion.identity);
			//create a new instance of the enemy prefab
            shifter = true;
			//flip the shifter value so it spawns on the other side
        } else if (shifter == true)
        {
            Instantiate(enemyPrefab, new Vector3(-10,0.1f,0), Quaternion.identity);
			//create a new instance of the enemy prefab
            shifter = false;
			//flip the shifter value so it spawns on the other side
        }
    }

    // This function handles what happens after portal time reaches 0.
    public void winFunction()
    {
        stageStart = false;
		//pause the stage
        stage++;
		//go to next level
        string stageName = "sceneStage" + stage;

        Debug.Log("Next Level Ready");
        //Play some fancy animation.
        new WaitForSeconds(10);
        //Discard current stage and move to next stage.
        //Application.Loadlevel(stageName);
    }
}
