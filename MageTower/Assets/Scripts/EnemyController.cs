using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
	public float halfEnemyHeight;
	//how tall half the enemy is (used to offset the height of enemy to determine whether it is finished climbing)

	private GameObject goal;
	//goal that this character wants to reach, in game space

	private GameObject particleContainer;
	//container that parents all particle effects

	public GameObject deathsplosion;
	//prefab of explosion

	Animator anim;
	//animator that controls the animation of the enemy

	Ray ray;
	RaycastHit hit;

	public UnityEngine.AI.NavMeshAgent agent;
	Rigidbody rb;
	CapsuleCollider collider;

	private int currentFloor;
	//floor that this character is currently on
	private int targetFloor;
	//floor that this character wants to reach

	public int climbSpeed;
	//speed at which this character climbs the ladder
	int climbing = 0;
	//is this character currently climbing the ladder
	/*
	 * -1 = climbing down
	 *  0 = not climbing
	 *  1 = climbing up
	 */
	float climbTargetElevation;
	// y value to which the character should be after climbing a ladder

	List <GameObject> destList = new List<GameObject>();
	//list of destinations that this character must traverse to reach its goal

	int nextDestIndex = 0;
	//the index of the character's next destination, stored in destList

	public bool levitated = true;

	//passing values to the enemy object upon instantiation
	public void Initialize(int tf){
		targetFloor = tf;
		//set values to local variables
	}

	// Use this for initialization
	void Start ()
	{
		currentFloor = (int)transform.position.y;
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		rb = GetComponent<Rigidbody>();
		collider = GetComponent<CapsuleCollider>();

		agent.enabled = false;
		//disable the agent until it hits the ground

		anim = transform.Find("Badguy").GetComponent<Animator>();
		//animation component attached to the model that is child of the enemy

		particleContainer = GameController.main.particleContainer;
		goal = GameController.main.goalCapsule;
		//set goal as dictated by the scene controller to the goal of this enemy


	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if(agent.isActiveAndEnabled && destReached(agent) && nextDestIndex < destList.Count){ //the agent is finished navigating to the ladder, and has another destination
			GameObject nextDestination = destList[nextDestIndex];
			//next destination in the list



			Debug.Log("State: " + anim.GetInteger("State"));

			if(targetFloor > currentFloor){
				//AGENT CLIMBS UP
				climbing = 1;
				rb.velocity = new Vector3(0f, climbSpeed, 0f);
				if(nextDestination.tag == "Ladder"){
					climbTargetElevation = nextDestination.GetComponent<LadderController>().topFloor;
				}

				anim.SetInteger("State", 3);
				//climbing
			}
			else if (targetFloor < currentFloor){
				//AGENT CLIMBS DOWN
				climbing = -1;
				rb.velocity = new Vector3(0f, -climbSpeed, 0f);
				if(nextDestination.tag == "Ladder"){
					climbTargetElevation = nextDestination.GetComponent<LadderController>().bottomFloor;
				}

				anim.SetInteger("State", 3);
				//climbing
			}
			//Debug.Log(climbing);
			rb.useGravity = false;
			agent.enabled = false;
			//Turn off navAgent component as it climbs
		}

		if(climbing != 0){ //Climbing
			//gameObject.transform.Translate(new Vector3(0f, climbing * climbSpeed * Time.deltaTime, 0f));
			//Debug.Log(climbing * climbSpeed * Time.deltaTime+" "+ rb.useGravity);
			if(gameObject.transform.position.y - halfEnemyHeight >= climbTargetElevation && climbing > 0){ //climbing up, at or exceeded targetElevation
				climbing = 0;
				//stop climbing
				rb.velocity = new Vector3();
				//stop climbing

				currentFloor = (int)climbTargetElevation;
				//change the currentFloor after climbing a ladder
				agent.enabled = true;
				//Turn on navagent component

				nextDestIndex++;
				//set next destination to the next one on the list
				agent.destination = destList[nextDestIndex].transform.position;
				//set the agent's destination to the upcoming destination

				anim.SetInteger("State", 1);
				//walking

			}
			if(gameObject.transform.position.y - halfEnemyHeight <= climbTargetElevation && climbing < 0){//climbing down, at or exceeded targetElevation
				climbing = 0;
				//stop climbing
				rb.velocity = new Vector3();
				//stop climbing

				currentFloor = (int)climbTargetElevation;
				//change the currentFloor after climbing a ladder
				agent.enabled = true;
				//Turn on navagent component

				nextDestIndex++;
				//set next destination to the next one on the list
				agent.destination = destList[nextDestIndex].transform.position;
				//set the agent's destination to the upcoming destination

				anim.SetInteger("State", 1);
				//walking

				//REPEATED CODE...will clean up later
			}
		}
	}

	void setNewDest(int newFloor, GameObject newGoal){
		targetFloor = newFloor;
		//set the targetfloor to the same floor as the object

		List<List<GameObject>> allPaths = generateAllPaths(currentFloor, newFloor, newGoal);
		//all the paths that can be taken to the goal

		if (allPaths.Count == 0) {
			//No paths
			agent.destination = transform.position;
			//set its destination to where it already is

			return;
			//don't run other code that searchs and set new paths
		}

		int selectedIndex = (int)(Random.value * allPaths.Count);
		//index of path selected

		destList = allPaths[selectedIndex];
		//select a random path from allPaths generated

		nextDestIndex = 0;
		//index of next destination in the selected path (destList)

		agent.updatePosition = true;
		agent.updateRotation = true;

		agent.destination = destList[0].transform.position;
		//go to next destination
	}

	List<List<GameObject>> generateAllPaths(int selectedFloor, int finalFloor, GameObject finalGoal){
		/*	floor = floor that is generating the path
		 * 	finalFloor = floor that contains the goal
		 * 	finalGoal = transform that contains the position of the goal
		 */

		List<List<GameObject>> possiblePaths = new List<List<GameObject>>();
		//possible paths to be sent back as r.v.

		if (targetFloor > selectedFloor){
			//target on a higher floor than the floor that is selected

			//Search for ladders from this floor that go up
			GameObject ladders = GameController.main.ladderContainer;

			//Generates a list of possible ladders to take next
			List <GameObject> possibleLadders = new List<GameObject>();
			//list of possible ladders that go up

			foreach(Transform child in ladders.transform){
				if(child.tag == "Ladder"){
					if(child.GetComponent<LadderController>().bottomFloor == selectedFloor){
						//select only ladders whose bottom is on this floor
						possibleLadders.Add(child.gameObject);
					}
				}
			}
			//

			//For each possible ladder path, add the ladder to the front of the path and add all combined paths to the possiblePaths
			foreach(GameObject ladder in possibleLadders){
				List<List<GameObject>> allPossiblePathsAhead = generateAllPaths(ladder.GetComponent<LadderController>().topFloor, finalFloor, finalGoal);

				foreach(List<GameObject> path in allPossiblePathsAhead){
					List<GameObject> possiblePath = new List<GameObject>();
					possiblePath.Add(ladder);
					possiblePath.AddRange(path);
					possiblePaths.Add(possiblePath);
				}
			}
		}

		else if (targetFloor < selectedFloor){
			//target on a lower floor than the floor that is selected

			//Search for ladders from this floor that go up
			GameObject ladders = GameController.main.ladderContainer;

			//Generates a list of possible ladders to take next
			List <GameObject> possibleLadders = new List<GameObject>();
			//list of possible ladders that go up

			foreach(Transform child in ladders.transform){
				if(child.tag == "Ladder"){
					if(child.GetComponent<LadderController>().topFloor == selectedFloor){
						//select only ladders whose bottom is on this floor
						possibleLadders.Add(child.gameObject);
					}
				}
			}
			//

			//For each possible ladder path, add the ladder to the front of the path and add all combined paths to the possiblePaths
			foreach(GameObject ladder in possibleLadders){
				List<List<GameObject>> allPossiblePathsAhead = generateAllPaths(ladder.GetComponent<LadderController>().topFloor, finalFloor, finalGoal);

				foreach(List<GameObject> path in allPossiblePathsAhead){
					List<GameObject> possiblePath = new List<GameObject>();
					possiblePath.Add(ladder);
					possiblePath.AddRange(path);
					possiblePaths.Add(possiblePath);
				}
			}
		}
		else{//goal is on this layer
			List<GameObject> possiblePath = new List<GameObject>();
			possiblePath.Add(finalGoal);
			possiblePaths.Add(possiblePath);
			//just return the goal instead of looking for more ladders
		}

		return possiblePaths;
	}

	bool destReached(UnityEngine.AI.NavMeshAgent mNavMeshAgent){
		if (!mNavMeshAgent.pathPending){
			if (mNavMeshAgent.remainingDistance <= mNavMeshAgent.stoppingDistance){
				if (!mNavMeshAgent.hasPath || mNavMeshAgent.velocity.sqrMagnitude == 0f){
					return true;
				}
			}
		}
		return false;
	}

	public void lifted(){
		//runs when lifted off the ground (by traps or hand)

		if(!levitated){
			levitated = true;

			anim.SetInteger("State", 0);
			//idle (change to flailing later)

			Debug.Log("State: " + anim.GetInteger("State"));

			rb.useGravity = true;
			//apply gravity

			agent.enabled = false;
			//stop navigating

			StartCoroutine(wait(0.25f));
		}

	}

	IEnumerator wait(float delay){
		yield return new WaitForSeconds(delay);
	}

	public void landed(int elevation){
		if(levitated){
			levitated = false;

			anim.SetInteger("State", 1);
			//walking
			Debug.Log("State: " + anim.GetInteger("State"));

			rb.useGravity = false;
			//stop applying gravity

			agent.enabled = true;
			//enable the NavMeshAgent

			currentFloor = elevation;

			setNewDest(targetFloor, goal);
			//set new destination for the NavMeshAgent

			Debug.Log("landed");
		}
	}

	public void death(){
		agent.enabled = false;
		//stop navigating (set location to current position)

		anim.SetInteger("State", -1);
		//fall

		Debug.Log("State: " + anim.GetInteger("State"));

		Debug.Log("DEATHSPLOSION!");

		StartCoroutine(disableCollider(0.2f));

		StartCoroutine(waitForExplode(1f));
		//wait 2 second, before dissolving
	}

	private IEnumerator disableCollider(float delay){
		yield return new WaitForSeconds(delay);

		rb.constraints = RigidbodyConstraints.FreezePosition;
		//freeze position

		collider.enabled = false;
		//disable collider
	}

	private IEnumerator waitForExplode(float delay){
		yield return new WaitForSeconds(delay);

		Debug.Log("DEATHSPLOSION!");

		rb.constraints = RigidbodyConstraints.FreezePosition;
		//freeze position

		collider.enabled = false;
		//disable collider

		GameObject tempExplosion = Instantiate (deathsplosion, transform.position, Quaternion.Euler(-90f, 0f, 0f), particleContainer.transform) as GameObject;
		tempExplosion.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
		Destroy (this.gameObject);
	}

	public void attackWizard(){
		agent.enabled = false;
		//stop navigating

		rb.useGravity = true;
		//apply gravity

		anim.SetInteger("State", 2);
		//attacking

		Debug.Log("State: " + anim.GetInteger("State"));
	}
	
}
	

