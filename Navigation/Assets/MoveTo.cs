// MoveTo.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic; //used for lists

public class MoveTo : MonoBehaviour {

	public Transform goal;
	//Transform object of the goal that this character wants to reach, in game space

	Ray ray;
	RaycastHit hit;

	NavMeshAgent agent;

	int currentFloor = 1;
	//floor that this character is currently on
	int targetFloor = 1;
	//floor that this character wants to reach

	int climbSpeed = 3;
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

	void Start () {
		//hit.transform.Translate(new Vector3(0f, 0f, 0f));

		agent = GetComponent<NavMeshAgent>();
		//goal.transform.Translate(new Vector3(0f, 2f, 0f));
		agent.destination = goal.position;

	}
	void Update() {
		if (Input.GetMouseButtonDown(0)){ // on click
			Debug.Log("hi");
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// casts a ray from camera to the point where your mouse is hovering over
			if (Physics.Raycast(ray, out hit, Mathf.Infinity)){
				
				if (hit.collider.tag == "Navigable"){
					targetFloor = hit.collider.gameObject.name[0] - 48; //character code offset is 48
					//set the targetfloor to the same floor as the object
					if (targetFloor > currentFloor){ //target on a higher floor
						destList = new Vector3[targetFloor - currentFloor + 1];
						for(int i = 0; i < targetFloor - currentFloor; i++){
							string ladderName = (currentFloor + i)+"-"+(currentFloor + i + 1);
							Vector3 ladder = GameObject.Find(ladderName).transform.position;
							destList[i] = ladder;
							//agent.destination = ladder.transform.position;
						}
						destList[targetFloor - currentFloor] = hit.point;

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
						Debug.Log(currentFloor - targetFloor);
						destList[currentFloor - targetFloor] = hit.point;

						//REPEATED CODE...will modify control flow later.
						agent.destination = destList[0];
						//set its first target as the 0th target
						nextDestIndex = 1;
					}
					else{ //target is on the same floor
						destList = new Vector3[0];
						//destroy previous list
						agent.destination = hit.point;
						//destination point is directly navigated without the aid of an array
					}

				}
			}
		}
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
	/*void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Ladder"){
			//climbSpeed = 1;
		}
	}
	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag == "Ladder"){
			//climbSpeed = -1;
		}
	}*/
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


