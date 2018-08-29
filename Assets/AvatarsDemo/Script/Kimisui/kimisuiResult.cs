using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class kimisuiResult : MonoBehaviour {

	public GameObject gameobject;		//cubeman用
	private bool LeftUpper_temp = false;
	private bool RightUpper_temp = false;
	private bool Fade_temp;

	private int timer_minute;
	private float timer_second;
	public Text timetext;
	public Text HighScore;
	private static int high_timer_minute = 0;
	private static float high_timer_second = 0;
	private AudioSource _audioSource;	//音楽取得用
	private AudioSource _audioSource2;	//音楽取得用

	// Use this for initialization
	void Start () {
		Fade_temp = true;
		_audioSource = GameObject.Find ("GameMusic").GetComponent<AudioSource> ();
		_audioSource.Play ();				//音楽再生
		_audioSource2 = GameObject.Find ("GameSE").GetComponent<AudioSource> ();

		timer_second = Kimisui.getTimeScoreSecond();
		timer_minute = Kimisui.getTimeScoreMinute ();
		timetext.text = "Score: "+ timer_minute.ToString ("00") + ":" + ((int)timer_second).ToString ("00");

		//highscore
		if ((timer_minute >= high_timer_minute) && (timer_second >= high_timer_second)) {
			high_timer_second = timer_second;
			high_timer_minute = timer_minute;
		}
		HighScore.text = "High Score: "+ high_timer_minute.ToString ("00") + ":" + ((int)high_timer_second).ToString ("00");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			_audioSource2.Play ();
			FadeManager.Instance.LoadScene ("StartScene_2",2.0f);
		}

		LeftUpper_temp = gameobject.GetComponent<CubemanController> ().LeftUpper_detected;
		RightUpper_temp = gameobject.GetComponent<CubemanController> ().RightUpper_detected;

		if (Fade_temp == true) {
			if (LeftUpper_temp == true || RightUpper_temp == true) {	//アッパーで選択
				_audioSource2.Play ();
				FadeManager.Instance.LoadScene ("StartScene_2", 2.0f);
				Fade_temp = false;
			}
		}
	}
}
