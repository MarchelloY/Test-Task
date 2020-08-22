using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Task1: MonoBehaviour {

	private static int score;
	private GameObject TryAgain;
	private GameObject Finish;

	void Start()
	{
		score = 0;
		TryAgain = GameObject.Find ("ReloadTask1");
		Finish = GameObject.Find ("Finish");
		if(Finish.activeSelf) Finish.SetActive (false);
		if(TryAgain.activeSelf) TryAgain.SetActive (false);
	}

	public void CorrectClick()
	{
		GameObject clickButton = EventSystem.current.currentSelectedGameObject;
		ChangeButtonSettings (clickButton);
		if(clickButton.name.Contains("_")) 
			ChangeButtonSettings(GameObject.Find (clickButton.name.Remove(2)));
		else 
			ChangeButtonSettings(GameObject.Find (clickButton.name + "_2"));
		score += 1000;
		UpdateScore ();
	}

	private void ChangeButtonSettings (GameObject button)
	{
		button.GetComponent<Outline> ().effectDistance = new Vector2 (5, 5);
		button.GetComponent<Button> ().interactable = false;
	}
		
	public void IncorrectClick()
	{
		if (score > 0)
		{
			score -= 500;
			TryAgain.SetActive (true);
		}
		UpdateScore ();
	}
		
	void UpdateScore() 
	{
		string result = "";
		if(score == 0) result = "0000";
		if (score.ToString().Length == 3) result = "0" + score;
		if(score.ToString().Length == 4) result = score.ToString ();
		if (score == 5000) {
			Finish.SetActive (true);
			Config.completedTask1 = true;
		}
		GameObject.Find ("Score").GetComponent<Text> ().text = result;
	}

}
