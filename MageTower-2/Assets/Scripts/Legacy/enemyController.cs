using UnityEngine;
using System.Collections;

public class enemyController : MonoBehaviour {

    float mouseHoldSeconds = 0;
    float mouseDelaySeconds = 0.5f;
    float walkspeed = 1.5f;
    public Rigidbody enemy;
    public AudioSource enemyAudio;

    // Used for animation flag, no purpose yet.
    bool climb = false;
    bool walk = false;
    bool attack = false;
    bool step = false;
    bool pickUp = false;

    Ray ray;
    RaycastHit hit;

    // Use this for initialization
    void Start()
    {
        enemy = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyMove();
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

    }

    // Var. MarkGX.
    private Vector3 screenPoint;
    private Vector3 offset;

    // Used to drag the object.
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

    void OnCollisionEnter(Collision collision)
    {
        // Checks if enemy would die from falling, if it does, give money and kill it.
        if (collision.relativeVelocity.magnitude > 8)
        {
            enemyAudio.Play();
            Debug.Log("Killed");
            Main.finance += 10;
            Destroy(gameObject);
        } else
        {
            new WaitForSeconds(1);
            pickUp = false;
        }
    }

    // It makes enemy move toward certain direction under certain condition.
    void enemyMove()
    {
        // Checks enemy location and make them move in certain direction.
        if (transform.position.x >= 1 && walk == true && pickUp == false)
        {
            enemy.velocity = new Vector3(-walkspeed, 0, 0);
        }
        else if (transform.position.x <= -1 && walk == true && pickUp == false)
        {
            enemy.velocity = new Vector3(walkspeed, 0, 0);
        }
        
        if (climb == true && pickUp == false) {
			float climbSpeed = walkspeed / 2;
			enemy.velocity = new Vector3(0, climbSpeed, 0);
		}


        if (transform.position.x >= 0.6f && attack == true && pickUp == false)
        {
            enemy.velocity = new Vector3(-walkspeed, 0, 0);
        }
        else if (transform.position.x <= -0.6f && attack == true && pickUp == false)
        {
            enemy.velocity = new Vector3(walkspeed, 0, 0);
        } else if (attack == true && pickUp == false)
        {
            enemy.velocity = new Vector3(0, 0, 0);
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
