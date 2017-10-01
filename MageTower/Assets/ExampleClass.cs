using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//no need to comment for namespaces

public class ExampleClass : MonoBehaviour{
	/* ROLE:
	 * Used to show how to write comment documentation for a class
	 */ 

	//^Short 1-2 line description of the class^
	//Briefly, what is it attached to and what does it do?



	//CLASS VARIABLES <-- Use these ALL CAP headers to organize the code!

	//COMPONENT VARIABLES - components attached to GameObject
	Rigidbody rb;
	//No need to comment because we all know it is simply a component attached to the GameObject that this script is attached to

	//STATIC CLASS VARIABLES - variables that belong to the class rather than an instance of the class
	public static int staticExampleVar = 0;
	//Short 1 line description of the variable and what it stands for

	//PUBLIC CLASS VARIABLES - variables accessible by other classes
	public int publicExampleVar = 0;
	//Short 1 line description of the variable and what it stands for

	//PRIVATE CLASS VARIABLES - variables accessible only by this class
	int privateExampleVar = 0;
	//Short 1 line description of the variable and what it stands for




	//FUNCTIONS

	//BUILT-IN FUNCTIONS - called automatically by Unity
	void Awake(){
		//No need to describe when Awake/Start/Update/FixedUpdate/OnTriggerEnter or such built-in functions get called

		//ASSIGN COMPONENT VARIABLES
		rb = GetComponent<Rigidbody>();
	}

	//PUBLIC FUNCTIONS - can be called by another class
	public int addAndCheckFreeze(int increase){
		/* PARAMETERS:
		 * increase = number to be added to the variable "privateExampleVar"
		 * DOES:
		 * Increases variable "privateExampleVar" by parameter "increase" and stop GameObject from moving
		 * RETURN VALUE:
		 * Returns the value of "privateExampleVar" after it has been increased
		 */

		privateExampleVar += increase;
		//increases "privateExampleVar" by "increase"

		checkFreeze();

		return privateExampleVar;
		//no need to mark what the return value gives back as that is already specified
	}

	//PRIVATE FUNCTIONS - can only be called by this class
	void checkFreeze(){
		/* DOES:
		 * Stops the GameObject from moving if "privateExampleVar" is less than 0
		 * Allows the GameObject to move if "privateExampleVar" is not less than 0
		 */

		if(privateExampleVar < 0){
			//No need to comment something like [if "privateExampleVar" is less than 0] << As a general rule, comment every line, unless it is REALLY trivial like this

			rb.isKinematic = true;
			//forces cannot be applied to the Rigidbody anymore
		}
		else{
			rb.isKinematic = false;
			//reallow forces to be applied to the Rigidbody
		}

	}
}