using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.IO;

public class Task2 : MonoBehaviour {
	
	public Sprite noMovePic;
	public Sprite swapPic;
	public Sprite playerPic;
	public Sprite matchPic;

	private GameObject firstClickButton;
	private GameObject secondClickButton;

	private bool flag;
	public bool vertHoriz;

	private Vector2 posNoMove;
	private int[,] matrix;


	void Start()
	{
		GameObject.Find ("FinishTask2").GetComponent<Image> ().raycastTarget = false;
		flag = true;
		vertHoriz = false;

		CreateMatrix ();
		FillMatrix ();
		UpdateMatrix ();
	}

	private void CreateMatrix() 
	{
		this.matrix = new int[3, 4];
		for (int y = 0; y < 3; y++)
			for (int x = 0; x < 4; x++)
				matrix [y, x] = 0;

		matrix [2, 3] = 1;
		matrix [2, 2] = 1;

		int randPosSwap = UnityEngine.Random.Range (0, 9);

		int randPosPlayer = randPosSwap;
		while(randPosPlayer == randPosSwap)
			randPosPlayer = UnityEngine.Random.Range (0, 10);

		int randPosNoMove = randPosSwap;
		while (randPosNoMove == randPosSwap || randPosNoMove == randPosPlayer)
			randPosNoMove = UnityEngine.Random.Range (0, 9);

		matrix [randPosSwap / 4, randPosSwap % 4] = 1;
		matrix [randPosPlayer / 4, randPosPlayer % 4] = 2;
		matrix [randPosNoMove / 4, randPosNoMove % 4] = 3;

		posNoMove = new Vector2 (randPosNoMove / 4, randPosNoMove % 4);
	}

	private void FillMatrix() 
	{
		for (int y = 0; y < 3; y++)
			for (int x = 0; x < 4; x++) 
			{
				Image sellPic = GameObject.Find (y.ToString() + x.ToString()).GetComponent<Image> ();
				switch (matrix [y, x]) {
				case 1:
					sellPic.sprite = matchPic; break;
				case 2:
					sellPic.sprite = playerPic; break;
				case 3:
					sellPic.sprite = noMovePic; break;
				default:
					sellPic.sprite = swapPic; break;
				}
			}
	}

	private void UpdateMatrix()
	{
		GameObject.Find ("FinishTask2").GetComponent<Image> ().raycastTarget = false;
		int count = 0;
		bool isCombo = false;
		for (int x = 0; x < 3; x++)
		{
			count = 0;
			for (int y = 0; y < 4; y++) 
			{
				if (GameObject.Find (x.ToString() + y.ToString()).GetComponent<Image> ().sprite == matchPic)
					count++;
				else
					count = 0;
				if (count == 3)
					isCombo = true;
			}
		}
		count = 0;
		for (int y = 0; y < 4; y++)
			for (int x = 0; x < 3; x++) 
			{
				if (GameObject.Find (x.ToString () + y.ToString ()).GetComponent<Image> ().sprite == matchPic)
					count++;
				else
					break;
				if (count == 3)
					isCombo = true;
			}
		
		if (isCombo)
			for (int x = 0; x < 3; x++)
				for (int y = 0; y < 4; y++) {
					Image temp = GameObject.Find (x.ToString () + y.ToString ()).GetComponent<Image> ();
					if (temp.sprite == matchPic)
						temp.sprite = swapPic;
				}
		if (GameObject.Find ("22").GetComponent<Image> ().sprite == playerPic)
			GameObject.Find ("FinishTask2").GetComponent<Image> ().raycastTarget = true;


		GameObject[] allSells = GameObject.FindGameObjectsWithTag ("Button");
		foreach (GameObject sell in allSells) {
			Image component = sell.GetComponent<Image> ();
			component.raycastTarget = (component.sprite == noMovePic || component.sprite == matchPic) ? false : true;
		}
				
	}

	public void GameController()
	{
		if (flag)
		{
			firstClickButton = EventSystem.current.currentSelectedGameObject;
			flag = false;
		}
		else 
		{
			secondClickButton = EventSystem.current.currentSelectedGameObject;
			int x1 = Int32.Parse(firstClickButton.name.Substring (0, 1)),
				y1 = Int32.Parse(firstClickButton.name.Substring (1, 1)),
				x2 = Int32.Parse(secondClickButton.name.Substring (0, 1)),
				y2 = Int32.Parse(secondClickButton.name.Substring (1, 1));

			List<Vector2> path = new List<Vector2> ();

			if (vertHoriz) 
			{
				path = Horizontal (y1, x1, x2, path);
				path = Vertical (x2, y1, y2, path);
			}
			else 
			{
				path = Vertical (x1, y1, y2, path);
				path = Horizontal (y2, x1, x2, path);
			}

			if (!path.Contains (posNoMove)) {
				StartCoroutine (ReplacePic(path));
			}
				
			flag = true;
		}
	}

	private List<Vector2> Vertical(int tempX, int y1, int y2, List<Vector2> list) {
		if (y1 > y2)
			for (int y = y1; y >= y2; y--)
				list.Add (new Vector2 (tempX, y));
		if (y1 < y2)
			for (int y = y1; y <= y2; y++)
				list.Add (new Vector2 (tempX, y));
		return list;
	}

	private List<Vector2> Horizontal(int tempY, int x1, int x2, List<Vector2> list) {
		if (x2 > x1) 
			for (int x = x1; x <= x2; x++)
				list.Add (new Vector2 (x, tempY));
		if (x2 < x1) 
			for (int x = x1; x >= x2; x--)
				list.Add (new Vector2 (x, tempY));
		return list;
	}

	IEnumerator ReplacePic(List<Vector2> path) {
		for (int i = 0; i < path.Count - 1; i++) {
			string nameFirst = path[i].x.ToString () + path[i].y.ToString (),
				   nameSecond = path[i+1].x.ToString () + path[i+1].y.ToString ();
			Sprite temp = GameObject.Find (nameFirst).GetComponent<Image> ().sprite;
			GameObject.Find (nameFirst).GetComponent<Image> ().sprite = GameObject.Find (nameSecond).GetComponent<Image> ().sprite;
			GameObject.Find (nameSecond).GetComponent<Image> ().sprite = temp;
			yield return new WaitForSeconds (0.1f);
		}
		UpdateMatrix();
	}
		
	public void SwitchMode()
	{
		Scrollbar scrollBar = GameObject.Find ("Scrollbar2").GetComponent<Scrollbar> ();
		if (scrollBar.value <= 0.5) 
		{
			scrollBar.value = 0;
			vertHoriz = false;
		} 
		else 
		{
			scrollBar.value = 1;
			vertHoriz = true;
		}
	}

}