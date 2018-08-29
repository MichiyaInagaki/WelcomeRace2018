using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelectController : MonoBehaviour {

	//public GameObject startButton1;
	//public GameObject startButton2;
	//public GameObject startButton3;

	private AudioSource _audioSource;	//音楽取得用
	private AudioSource _audioSource_SE;	//音楽取得用

	public static int _stage = 0;

	// Use this for initialization
	void Start () {
		_audioSource = GameObject.Find ("GameMusic").GetComponent<AudioSource> ();
		_audioSource.Play ();				//音楽再生
		_audioSource_SE = GameObject.Find ("GameSE").GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {

	}


	public void StartStage1(){
		_audioSource_SE.Play ();
		FadeManager.Instance.LoadScene ("Stage1",1.0f);
		_stage = 1;
	}

	public void StartStage2(){
		_audioSource_SE.Play ();
		FadeManager.Instance.LoadScene ("Stage2",1.0f);
		_stage = 2;
	}

	public void StartStage3(){
		_audioSource_SE.Play ();
		FadeManager.Instance.LoadScene ("Stage3",1.0f);
		_stage = 3;
	}

	public void Kimisui(){
		_audioSource_SE.Play ();
		FadeManager.Instance.LoadScene ("minigame1",1.0f);
	}

	public void Garupan(){
		_audioSource_SE.Play ();
		FadeManager.Instance.LoadScene ("minigame2",1.0f);
	}

	public void Howtoplay(){
		_audioSource_SE.Play ();
		FadeManager.Instance.LoadScene  ("HowToPlay",2.0f);
	}

	public static int getStage(){	//result画面に数値を送る用
		return _stage;
	}

}
