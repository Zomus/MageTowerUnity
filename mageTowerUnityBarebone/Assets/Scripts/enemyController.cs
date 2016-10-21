using UnityEngine;
using System.Collections;

public class enemyController : MonoBehaviour {

    float mouseHoldSeconds = 0;
    float mouseDelaySeconds = 0.5f;


    float walkspeed = 1.5f;
	//speed at which the enemy walks

    public Rigidbody enemy;
	//Collider???

    public AudioSource enemyAudio;

    // Used for animation flag, no purpose yet.
	//Question to Jon: The variables below all affect how your code runs? why the no purpose?

	//STATES
    bool climb = false;
    bool walk = false;
    bool attack = false;
    bool step = false;
    bool pickUp = false;

    Ray ray; //UNUSED
	RaycastHit hit; //UNUSED

    // Use this for initialization
    void Start()
    {
        enemy = GetComponent<Rigidbody>();
		//set enemy to its Rigidbody collider component???
    }

    // Update is called once per frame
    void Update()
    {
        enemyMove();
		//function call to function that makes enemy move (walks/climbs/attacks)

		//LOCATION OF ENEMY DETERMINES ITS ACTIONS
        if (transform.position.y <= 0.5f && climb == false && transform.position.x >= 1.1f || transform.position.x <= -1.1f)
        {
            walk = true;
        } else if (transform.position.x <= 1.1f && transform.position.x >= -1.1f && attack == false && transform.position.y <= 4.0f && pickUp == false)
        {
            walk = false;
            climb = true;
        } else if (transform.position.y > 4.0f && pickUp == false) {
            climb = false;
            attack = true;
        } else
        {
			// Did not wrote the code for mouse pointer delay.
	        walk = false;
			climb = false;
			attack = false;
            //Plays struggle animation.
        }

        /* Targets object to be dragged
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Enemy")
                {
                    OnMouseDrag();
                }
            }
        }*/
		//QUESTION to Jon: What is the difference between OnMouseDown and Input.GetMouseButtonDown?
    }

    // Var. MarkGX.
    private Vector3 screenPoint;
    private Vector3 offset;

    // Used to drag the object.
    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		//defines screenPoint as position on the screen rather than the world (the world has a different coordinate system) and defines it to be the enemy's position

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		//defines offset to be a vector from the mouse's position to the object's position as a position in the world
    }

    // Used to drag the object.
    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		//defines position of mouse as a position on the screen

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		//defines the current position of the mouse to the GameObject's position
		//NOTE to Jon: can't curPosition simply = gameObject.transform.position? because replacing offset with the value of offset gives a term that cancels itself
        transform.position = curPosition;
		//sets the enemy position ot the position of the mouse
        pickUp = true;
		//marks the enemy as currently being picked up
    }

    void OnCollisionEnter(Collision collision)
    {
        // Checks if enemy would die from falling, if it does, give money and kill it.
        if (collision.relativeVelocity.magnitude > 8)
        {
            enemyAudio.Play();
			//play the death sound effect of an enemy
            Debug.Log("Killed");
			//output message that an enemy has been killed
            Main.finance += 10;
			//increase money
            Destroy(gameObject);
			//completely remove the enemy
        } else
        {
            new WaitForSeconds(1);
			//pauses current coroutine for 1 second??? (not sure what this does exactly)
            pickUp = false;
			//marks the enemy as currently not being picked up
        }
    }

    void enemyMove()
    {
        // Checks enemy location and make them move in certain direction.
        if (transform.position.x >= 1 && walk == true && pickUp == false) //if the enemy is right of the tower and is walking and is not picked up
			//NOTE to Jon: in "walk == true" the "== true" is redundant because the value of walk is true already; similarly "pickUp == false" can be replaced by "!pickUp" its much faster
		{
            enemy.velocity = new Vector3(-walkspeed, 0, 0);
			//sets the enemy velocity to moving in the -x direction (left) at walkspeed
        }
        else if (transform.position.x <= -1 && walk == true && pickUp == false) //if the enemy is left of the twoer and is walking and is not picked up
        {
            enemy.velocity = new Vector3(walkspeed, 0, 0);
			//sets the enemy velocity to moving in the +x direction (right) at walkspeed
        }

		//CLIMBING
        if (climb == true && pickUp == false) { //if the enemy is climbing and is not picked up
			float climbSpeed = walkspeed / 2;
			//set climbSpeed to half of walkspeed
			enemy.velocity = new Vector3(0, climbSpeed, 0);
			//sets the enemy velocity to moving in the +y direction (up) at climbSpeed
		}

		//ATTACKING
        if (transform.position.x >= 0.6f && attack == true && pickUp == false) //if the enemy is right of the center of the tower and is attacking and is not picked up
        {
            enemy.velocity = new Vector3(-walkspeed, 0, 0);
			//sets the enemy velocity to moving in the -x direction (left) at walkspeed
        }
		else if (transform.position.x <= -0.6f && attack == true && pickUp == false) //if the enemy is left of the center of the tower and is attacking and is not picked up
        {
            enemy.velocity = new Vector3(walkspeed, 0, 0);
			//sets the enemy velocity to moving in the +x direction (right) at walkspeed
        } else if (attack == true && pickUp == false) //if the enemy is close enough to the tower
        {
            enemy.velocity = new Vector3(0, 0, 0);
			//sets the enemy velocity to 0
        }
    }

    /* Legacy Code
    void DragSys()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 setPoint;

        setPoint.x = point.x;
        setPoint.y = point.y - 3;
        setPoint.z = gameObject.transform.position.z;

        gameObject.transform.position = setPoint;
        pickUp = true;
    } */
}
