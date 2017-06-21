using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public Transform goal;
	//Transform object of the goal that this character wants to reach, in game space

	Ray ray;
	RaycastHit hit;

	NavMeshAgent agent;

	int currentFloor;
	//floor that this character is currently on
	int targetFloor;
	//floor that this character wants to reach

	int climbSpeed;
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

	Vector3 [] destList = new Vector3[0];
	//list of destinations that this character must traverse to reach its goal
	int nextDestIndex = 0;
	//the index of the character's next destination, stored in destList

	//passing values to the enemy object upon instantiation
	public void Initialize(int cf, int tf, int cs){
		currentFloor = cf;
		targetFloor = tf;
		climbSpeed = cs;
		//set values to local variables
	}

	// Use this for initialization
	void Start ()
	{
		Debug.Log("spawned");
		agent = GetComponent<NavMeshAgent>();
		//goal.transform.Translate(new Vector3(0f, 2f, 0f));

		goal = SceneController.main.goal.transform;
		//set goal as dictated by the scene controller to the goal of this enemy
		//agent.destination = goal.position;
		//set new destination to be the goal's position

		setNewDest(SceneController.main.goalFloor, goal);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(agent.isActiveAndEnabled && destReached(agent) && nextDestIndex < destList.Length){ //the agent is finished navigating to the ladder, and has another destination
			if(targetFloor > currentFloor){
				//AGENT CLIMBS UP
				climbing = 1;
			}
			else{
				//AGENT CLIMBS DOWN
				climbing = -1;
			}
			string ladderName = (currentFloor + 1 * climbing)+"-"+currentFloor;
			GameObject endOfLadder = GameObject.Find(ladderName);
			climbTargetElevation = endOfLadder.transform.position.y;
			//set the targetElevation

			agent.enabled = false;
			//Turn off navagent component
		}

		if(climbing != 0){ //Climbing
			gameObject.transform.Translate(new Vector3(0, climbing * climbSpeed * Time.deltaTime, 0));
			if(gameObject.transform.position.y >= climbTargetElevation && climbing > 0){ //climbing up, at or exceeded targetElevation
				climbing = 0;
				//stop climbing
				currentFloor++;
				//increase the currentFloor after climbing a floor
				agent.enabled = true;
				//Turn on navagent component
				agent.destination = destList[nextDestIndex];
				//set the agent's destination to the upcoming destination
				nextDestIndex++;
				//set next destination to the next one on the list
			}
			if(gameObject.transform.position.y <= climbTargetElevation && climbing < 0){//climbing down, at or exceeded targetElevation
				climbing = 0;
				//stop climbing
				currentFloor--;
				//decrease the currentFloor after climbing a floor
				agent.enabled = true;
				//Turn on navagent component
				agent.destination = destList[nextDestIndex];
				//set the agent's destination to the upcoming destination
				nextDestIndex++;
				//set next destination to the next one on the list

				//REPEATED CODE...will clean up later
			}
		}
	}

	void setNewDest(int newFloor, Transform newGoal){
		targetFloor = newFloor;
		//set the targetfloor to the same floor as the object

		if (targetFloor > currentFloor){ //target on a higher floor
			destList = new Vector3[targetFloor - currentFloor + 1];
			for(int i = 0; i < targetFloor - currentFloor; i++){
				string ladderName = (currentFloor + i)+"-"+(currentFloor + i + 1);
				Vector3 ladder = GameObject.Find(ladderName).transform.position;
				destList[i] = ladder;
				//agent.destination = ladder.transform.position;
			}
			destList[targetFloor - currentFloor] = newGoal.position;

			agent.destination = destList[0];
			//set its first target as the 0th target
			nextDestIndex = 1;
		}
		else if(targetFloor < currentFloor){ //target on a lower floor
			destList = new Vector3[currentFloor - targetFloor + 1];
			for(int i = 0; i > targetFloor - currentFloor; i--){
				//Debug.Log(currentFloor+" "+targetFloor+" "+(currentFloor-i-1));
				string ladderName = (currentFloor + i)+"-"+(currentFloor + i - 1);
				//Debug.Log(ladderName);
				Vector3 ladder = GameObject.Find(ladderName).transform.position;
				destList[-i] = ladder;
				//agent.destination = ladder.transform.position;
			}

			destList[currentFloor - targetFloor] = newGoal.position;

			//REPEATED CODE...will modify control flow later.
			agent.destination = destList[0];
			//set its first target as the 0th target
			nextDestIndex = 1;
		}
		else{ //target is on the same floor
			destList = new Vector3[0];
			//destroy previous list
			agent.destination = newGoal.position;
			//destination point is directly navigated without the aid of an array
		}

	}

	bool destReached(NavMeshAgent mNavMeshAgent){
		if (!mNavMeshAgent.pathPending){
			if (mNavMeshAgent.remainingDistance <= mNavMeshAgent.stoppingDistance){
				if (!mNavMeshAgent.hasPath || mNavMeshAgent.velocity.sqrMagnitude == 0f){
					return true;
				}
			}
		}
		return false;
	}
	
}
	

