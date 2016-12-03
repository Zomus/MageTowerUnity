using UnityEngine;
using System.Collections;

public class enemyFunction : MonoBehaviour {

    float mouseHoldSeconds = 0;
	//counts amount of seconds holding down given object - unused
    float mouseDelaySeconds = 0.5f;
	//amount of seconds for the mouse slowdown
    public Rigidbody enemy;
	//collision area of this enemy
    public Collider enemyCollider;
	//collider function
    float walkspeed = 0;
	//speed at which the enemy walks
    int posA = 0;
	//index of element in a "targetPoint" list
    bool pickUp = false;
	//whether this enemy is picked up by the magehand
    bool attack = false;
	//whether this enemy is currently attacking the wizard

	float attackCD = 2;
	//attack cooldown (2 seconds before it attacks you again
	float currentCD = 0;
	//actual counter that counts down


	//STATS
<<<<<<< HEAD
	/*
	float maxHp = 100;
	float hp = 100;
=======
	float maxHp = 10;
	float hp = 5;
>>>>>>> 5ef3b943ade2cacd53e83f424b8db15813fc8784

	public GameObject hpCanvas;
	public float hpBarLength;
	public float fullHpBarLength;

	GUIStyle currentStyle;
*/
    Ray ray;
    RaycastHit hit;

    private Vector3 screenPoint;
    private Vector3 offset;

    // Use this for initialization
    void Start () {
        walkspeed = 2.0f * Time.deltaTime;
		//set walkspeed to 1 (times deltaTime (time elapsed per frame))

        enemy = GetComponent<Rigidbody>();
		//set the collider

        if (transform.position.x > 0) //if it is right of the tower
        {
            posA = 1;
			//get odd index for location
        }

<<<<<<< HEAD
		//fullHpBarLength = 50;
=======
		hpCanvas = Main.enemyHpBarList[Main.enemyList.Find(this)]

		//fullHpBarLength = 20;
>>>>>>> 5ef3b943ade2cacd53e83f424b8db15813fc8784

		/*hpBar = new Slider();
		hpBar.minValue = 0;
		hpBar.maxValue = maxHp;
		hpBar.handleRect = null;
		hpBar.interactable = false;
		hpBar.transform.Translate(gameObject.transform.position - hpBar.transform.position);*/

	}
	
	// Update is called once per frame
	void Update () {
        if (pickUp == false && transform.position != Main.targetPoint[posA] && attack == false)
			//if this enemy is not picked up and not reached its target and not attacking wizard
        {
			transform.position = Vector3.MoveTowards(transform.position, Main.targetPoint[posA], walkspeed);
			//change current position by walkspeed from current position to target position
            enemyCollider.attachedRigidbody.useGravity = false;
			//don't apply gravity
        }
        else if (pickUp == true)
			//if this enemy is picked up
        {
			if (transform.position.x > 0)
				//if it is right of the tower
			{
				posA = Main.stageDefault + 1;
				//set its new target position to slightly left of the tower
			} else { //otherwise
				posA = Main.stageDefault;
				//set its new target position to slightly right of the tower
			}
			//set array target to default (usually 0)
            enemyCollider.attachedRigidbody.useGravity = true;
			//apply gravity to enemy
            attack = false;
			//enemy unable to attack
        }
        else if (pickUp == false && transform.position == Main.targetPoint[posA] && posA < Main.stageLimit)
			//not picked up and position is reached, and position index has not been changed past the limit
        {
            posA += 2;
			//if the point is reached, then it climbs the tower
        }
        else if (pickUp == false && posA >= Main.stageLimit && transform.position == Main.targetPoint[posA])
			//not picked up and position is reached, and position index has been changed past the limit
        {
            enemyCollider.attachedRigidbody.useGravity = true;
			//apply gravity so enemy stands on top of tower
            attack = true;
			//attack the wizard
        }
		if(attack){
			if(currentCD <= 0){
				Main.wizardHp -= 1;
				//deal damage to wizard
				currentCD = attackCD;
				//reset cooldown of attack
<<<<<<< HEAD
				//hp--;
=======
>>>>>>> 5ef3b943ade2cacd53e83f424b8db15813fc8784
			}
			else{
				currentCD -= Time.deltaTime;
			}
		}
<<<<<<< HEAD
		//displayCurrentHealth();
=======
>>>>>>> 5ef3b943ade2cacd53e83f424b8db15813fc8784
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
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
        pickUp = true;
    }

    // If dropped enemy fell, check the speed to kill enemy, or let it live.
    void OnCollisionEnter(Collision collision)
    {
        // Checks if enemy would die from falling, if it does, give money and kill it.
        if (collision.relativeVelocity.magnitude > 8)
        {
            Debug.Log("Killed");
            Main.finance += 10;
            Destroy(gameObject);
        }
        else
        {
            new WaitForSeconds(1);
            pickUp = false;
        }
    }

	void OnGUI() {
<<<<<<< HEAD
		/*
		currentStyle = new GUIStyle(GUI.skin.box);
		currentStyle.normal.background = MakeTex(600, 1, new Color(0f, 1f, 0f, 0.5f));

=======
		
		hpBarLength = fullHpBarLength * (hp /(float)maxHp);
>>>>>>> 5ef3b943ade2cacd53e83f424b8db15813fc8784
		if(hp > 0){
			//Debug.Log(hpBarLength);
			currentStyle = new GUIStyle(GUI.skin.box);
			currentStyle.normal.background = MakeTex((int)hpBarLength, 4, new Color(0f, 1f, 0f, 0.5f));
			GUI.Box(new Rect(Camera.main.WorldToScreenPoint(transform.position).x - fullHpBarLength/2, Camera.main.WorldToScreenPoint(-transform.position).y + 120, fullHpBarLength, 4), "");
			GUI.Box(new Rect(Camera.main.WorldToScreenPoint(transform.position).x - fullHpBarLength/2, Camera.main.WorldToScreenPoint(-transform.position).y + 120, hpBarLength, 4), "", currentStyle);
		}
		//BUG: OnGUI doesn't update often so when hp is negative the bar will go into the negatives*/
	}

<<<<<<< HEAD
	/*public void displayCurrentHealth() {
		hpBarLength = fullHpBarLength * (hp /(float)maxHp);
	}*/

=======
>>>>>>> 5ef3b943ade2cacd53e83f424b8db15813fc8784
	Texture2D MakeTex(int width, int height, Color col) {
		var pix = new Color[width * height];

		for (int i = 0; i < pix.Length; i++) {
			pix[i] = col;
		}

		var result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();
		return result;
	}
}
