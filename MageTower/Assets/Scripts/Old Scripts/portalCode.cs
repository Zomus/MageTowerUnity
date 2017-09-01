using UnityEngine;
using System.Collections;

public class portalCode : MonoBehaviour {
    
    Ray ray;
    RaycastHit hit;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Portal")
                {
                    stageManager();
                }
            }
        }

    }

    void stageManager()
    {
        if (Main.stage == 1 && Main.stageStart == false)
        {
            Main.timer = 180;
            Main.lv1pawn = true;
            Main.enemyLeft = 50;
            Main.stageStart = true;
            StartCoroutine("timeEngine");
        }
    }

    // This function will handle global timer for this game.
    IEnumerator timeEngine()
    {
        while (Main.timer > 0 && Main.stageStart == true)
        {
            Main.timer = Main.timer - 1;
            Main.tick++;
            yield return new WaitForSeconds(1);
        }
    }

}
