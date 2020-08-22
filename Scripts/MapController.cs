using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapController : MonoBehaviour {

	private string thisState;
	private GameObject point;
	private float moveValue;
	private bool isMoved;
	private static bool created = false;

	public float x0 = 722.43f;
	public float y0 = 22.7f;
	public float radius = 618.5f;
	public float kekX = 171f;
	public float kekX2 = 1274.5f;
	public float kekX3 = 720f;

	void Awake()
	{
		if (!created) {
			DontDestroyOnLoad (GameObject.Find ("Music"));
			created = true;
		}
		if (GameObject.FindGameObjectsWithTag ("Music").Length > 1)
			Destroy (GameObject.FindGameObjectsWithTag ("Music")[1]);
	}

	void Start()
	{
		thisState = "task1";
		point = GameObject.Find ("Point");
		moveValue = -1.12f;
	}

	public void ClickMapButtons()
	{
		switch (EventSystem.current.currentSelectedGameObject.name) 
		{
		case "task1":
			ChangeScene ("task1", 1);
			break;
		case "task2":
			ChangeScene ("task2", 2);
			break;
		case "win":
			Animator anim = GameObject.Find ("WinWindow").GetComponent<Animator> ();
			if (thisState == "win" && !isMoved) anim.SetBool ("ClickButton", !anim.GetBool("ClickButton"));
			thisState = "win";
			break;
		}
	}

	private void ChangeScene(string name, int number) 
	{
		if (thisState == name && !isMoved) SceneManager.LoadScene (number);
		thisState = name;
	}

	void FixedUpdate()
	{
		Image winButton = GameObject.FindGameObjectWithTag("Button").GetComponent<Image>();
		if (Config.completedTask1 && Config.complitedTask2) {
			winButton.color = Color.green;
			winButton.raycastTarget = true;
		} else {
			winButton.color = Color.gray;
			winButton.raycastTarget = false;
		}
		if (Config.completedTask1)
			GameObject.Find ("task1").GetComponent<Image> ().color = Color.green;
		if (Config.complitedTask2)
			GameObject.Find("task2").GetComponent<Image> ().color = Color.green;
		
		isMoved = false;
		switch(thisState)
		{
			case "task1":
				if (kekX < point.transform.position.x) 
				{
					moveValue -= Time.fixedDeltaTime;
					Move ();
				}
				break;
			case "win":
				if (kekX2 > point.transform.position.x) 
				{
					moveValue += Time.fixedDeltaTime;
					Move ();
				}
				break;
			case "task2":
				if (kekX3 + 5f < point.transform.position.x) 
				{
					moveValue -= Time.fixedDeltaTime;
					Move ();
				} 
				if (kekX3 - 5f > point.transform.position.x)  
				{
					moveValue += Time.fixedDeltaTime;
					Move ();
				}
				break;
		}
		if (point.GetComponent<Image> ().color.a == 0) isMoved = true; 
		if (isMoved) 
		{
			StopAllCoroutines ();
			point.GetComponent<Animator> ().SetBool ("ScalePoint", false);
		}
		if(!isMoved) StartCoroutine (Wait());
	}

	void Move() 
	{
		float x = x0 + Mathf.Sin (moveValue) * radius;
		float y = y0 + Mathf.Cos (moveValue) * radius;
		point.transform.position = new Vector3 (x, y, 0);
		isMoved = true;
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds (30);
		point.GetComponent<Animator> ().SetBool ("ScalePoint", true);
	}

}
