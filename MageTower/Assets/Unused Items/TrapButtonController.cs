using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TrapButtonController : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler {

	public Button controllingButton;
	//button that this controller is attached to

	private GameObject[] expand;
	//list of expandable buttons given the tag ButtonLevel1

	//public int mouseOnCount = 0;
	//can be used to count the amount of rollovers

	private List <float> buttonWidths = new List<float>();
	private List <float> buttonHeights = new List<float>();
	//base size of buttons

	private List <float> targetWidths = new List<float>();
	private List <float> targetHeights = new List<float>();
	//target size (size you want it to be at the current moment)


	public void OnPointerEnter(PointerEventData eventData)
	{
		for(int i = 0; i < expand.Length; i++){
			targetWidths[i] = buttonWidths[i]*1.2f;
			targetHeights[i] = buttonHeights[i]*1.2f;
			//set target sizes to 1.2 times of base size on roll over
		}
		//mouseOnCount = mouseOnCount+1;
		//Debug.Log(mouseOnCount);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		for(int i = 0; i < expand.Length; i++){
			targetWidths[i] = buttonWidths[i]*0.5f;
			targetHeights[i] = buttonHeights[i]*0.5f;
			//set target sizes to half of base size on roll over
		}
	}

	// Use this for initialization
	void Start () {
		//Button btn = controllingButton.GetComponent<Button>();
		//btn.onClick.AddListener(TaskOnClick);


		expand = GameObject.FindGameObjectsWithTag("ButtonLevel1");
		//fill list with objects with tag "ButtonLevel1"; affected by this controller

		for(int i = 0; i < expand.Length; i++){
			RectTransform currentRect = expand[i].transform as RectTransform;

			buttonWidths.Add(currentRect.rect.width);
			buttonHeights.Add(currentRect.rect.height);
			targetWidths.Add(currentRect.rect.width*0.5f);
			targetHeights.Add(currentRect.rect.height*0.5f);
			//record base sizes and targetSizes
		}

	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < expand.Length; i++){ //loop to update every rectangle
			RectTransform currentRect = expand[i].transform as RectTransform;

			float factor = 4f;
			//speed factor used to gradually move toward target sizes
			currentRect.sizeDelta = new Vector2(currentRect.rect.width + (targetWidths[i] - currentRect.rect.width)/factor, currentRect.rect.height + (targetHeights[i] - currentRect.rect.height)/factor);
			//linear approximation to get an effect that gradually moves towards the width and height I want
		}
	}
	void TaskOnClick(){
		/*for(int i = 0; i < expand.Length; i++){
			RectTransform currentRect = expand[i].transform as RectTransform;
			currentRect.sizeDelta = new Vector2(buttonWidths[i]*1.2f, buttonWidths[i]*1.2f);
		}
		Debug.Log ("You have clicked the button!");*/
		//OLD CODE. Works but not as good as using IPointers
	}
}