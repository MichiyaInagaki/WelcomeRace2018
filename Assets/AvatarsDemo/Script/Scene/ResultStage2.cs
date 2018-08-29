using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultStage2 : MonoBehaviour {

	public GameObject gameobject;		//cubeman用
	private bool LeftUpper_temp = false;
	private bool RightUpper_temp = false;
	private bool Fade_temp;

	private int score;
	private int maxcombo;
	private static int high_score = 0;

	public Text scoreText;
	public Text maxcomboText;
	public Text highscoreText;
	public Text Rank;

	private AudioSource _audioSource;	//音楽取得用
	private AudioSource _audioSource2;	//音楽取得用

	// Use this for initialization
	void Start () {
		Fade_temp = true;
		_audioSource = GameObject.Find ("GameMusic").GetComponent<AudioSource> ();
		_audioSource.Play ();				//音楽再生
		_audioSource2 = GameObject.Find ("GameSE").GetComponent<AudioSource> ();

		score = GameController.getScore();
		maxcombo = GameController.getMaxcombo();
		scoreText.text = "Score: "+score.ToString ();
		maxcomboText.text = "Max Combo: "+maxcombo.ToString ();

		//highscore
		if(score >= high_score){
			high_score = score;
		}
		highscoreText.text = "High Score: " + high_score.ToString ();

		//rank
		if (score <= 2400) {
			Rank.text = "Rank: F";
		} else if (2400 < score && score <= 3400) {
			Rank.text = "Rank: C";
		}else if (3400 < score && score <= 4400) {
			Rank.text = "Rank: C+";
		}else if (4400 < score && score <= 5500) {
			Rank.text = "Rank: B";
		}else if (5500 < score && score <= 6500) {
			Rank.text = "Rank: B+";
		}else if (6500 < score && score <= 7500) {
			Rank.text = "Rank: A";
		}else if (7500 < score && score <= 8500) {
			Rank.text = "Rank: A+";
		}else if (8500 < score) {
			Rank.text = "Rank: S";
		}
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
