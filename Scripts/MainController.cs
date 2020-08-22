using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainController: MonoBehaviour {

	private GameObject setting;
	private GameObject author;
	private GameObject start;
	private Scrollbar scrollBar;

	void Start()
	{
		if (GameObject.Find ("Scrollbar") != null) 
		{
			scrollBar = GameObject.Find ("Scrollbar").GetComponent<Scrollbar> ();
			if (Config.playMusic) scrollBar.value = 1;
			else scrollBar.value = 0;
		}

		setting = GameObject.Find ("Setting");
		author = GameObject.Find ("Author");
		start = GameObject.Find ("Canvas");
	}

	public void ButtonsController() {
		switch (EventSystem.current.currentSelectedGameObject.name) 
		{
		case "StartButton":
			AnimationController (start.GetComponent<Animator> ());
			break;
		case "SettingButton":
			AnimationController (setting.GetComponent<Animator> ());
			break;
		case "AuthorButton":
			AnimationController (author.GetComponent<Animator> ());
			break;
		case "BackButton":
			SceneManager.LoadScene (0);
			break;
		case "ReloadTask1":
			SceneManager.LoadScene (1);
			break;
		case "FinishTask2":
			SceneManager.LoadScene (0);
			Config.complitedTask2 = true;
			break;
		}
	}

	private void AnimationController(Animator anim)
	{
		anim.SetBool ("ClickButton", !anim.GetBool("ClickButton"));
	}

	public void MusicController() 
	{
		AudioSource audioSource = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource> ();
		if (scrollBar.value <= 0.5) 
		{
			scrollBar.value = 0;
			Config.playMusic = false;
			audioSource.mute = true;
		} 
		else 
		{
			scrollBar.value = 1;
			Config.playMusic = true;
			audioSource.mute = false;
		}
	}

}