using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GarupanCnotroller : MonoBehaviour {

	public GameObject particle;		//Particleを取得
	public GameObject particle2;
	public GameObject gameobject;	//パンチ検出のフラグ取得用 Cubeman
	private bool LeftUpper_temp = false;
	private bool RightUpper_temp = false;

	private bool DoubleUpper_temp = false;	//ゲーム中断用
	private float countdown = 4.0f;
	private int countdownseconds;
	private bool interruption_temp = false;				//中断用テンプ

	public Text timerText3;			//タイマー用
	public float totalTime3 = 3f;
	int seconds3;
	public Text timerText;			//タイマー用
	public float totalTime = 20f;
	int seconds;

	private bool _isPlaying = false;	//開始フラグ
	public GameObject startButton;
	private AudioSource _audioSource;	//音楽取得用
	private AudioSource _audioSource2;	//音楽取得用

	public static int garupan_score = 0;
	public Text garupan_scoreText;				//スコア表示用

	private bool endgametemp = false;
	public GameObject finishLabel;
	public GameObject intro;
	public GameObject intro2;

	public GameObject unity_chan_right;
	public GameObject unity_chan_left;
	private Vector3 rightposition;
	private Vector3 leftposition;

	// Use this for initialization
	void Start () {
		_audioSource = GameObject.Find ("GameMusic").GetComponent<AudioSource> ();
		_audioSource2 = GameObject.Find ("GameSE").GetComponent<AudioSource> ();
		garupan_score = 0;

	}
	
	// Update is called once per frame
	void Update () {
		rightposition = unity_chan_right.transform.position;
		leftposition = unity_chan_left.transform.position;

		//ゲーム中断用
		if (interruption_temp == false) {
			DoubleUpper_temp = gameobject.GetComponent<CubemanController> ().DoubleUpper_detected;
			if (DoubleUpper_temp == true) {			//ダブルアッパー検出
				countdown -= Time.deltaTime;		//カウントダウン
				//Debug.Log (countdown);
				countdownseconds = (int)countdown;
				if (countdownseconds == 0) {
					interruption_temp = true;
					ReturnToMenu ();				//メニューへ
				}
			} else {
				countdown = 4.0f;
			}
		}

		if (_isPlaying == false) {
			LeftUpper_temp = gameobject.GetComponent<CubemanController> ().LeftUpper_detected;
			RightUpper_temp = gameobject.GetComponent<CubemanController> ().RightUpper_detected;

			if (LeftUpper_temp == true || RightUpper_temp == true) {	//アッパーで選択
				StartGarupanGame ();
			}
		}

		if (_isPlaying) {
			totalTime3 -= Time.deltaTime;
			seconds3 = (int)totalTime3;
			timerText3.text= seconds3.ToString();
			Destroy (intro);
			Destroy (intro2);
			if(seconds3 < 1){
				timerText3.enabled = false;
				StartGame ();
			}
		}

	}

	public void StartGarupanGame(){
		_audioSource2.Play ();
		startButton.SetActive (false);		//スタートボタン消す
		_audioSource.Play ();				//音楽再生
		_isPlaying = true;					//開始フラグ・オン
	}

	public void StartGame(){
		totalTime -= Time.deltaTime;
		seconds = (int)totalTime;
		timerText.text= seconds.ToString();
		if(seconds < 1){
			timerText.enabled = false;
			endgametemp = true;
			StartCoroutine ("ending_garupan");	//ending game
		}
		Puncher ();
		garupan_scoreText.text = "Score: "+garupan_score.ToString ();
	}

	void Puncher(){
		bool right_temp = gameobject.GetComponent<CubemanController> ().RightPanch_detected;	//右パンチ検出のフラグ読み込み
		bool left_temp = gameobject.GetComponent<CubemanController> ().LeftPanch_detected;

		if (endgametemp == false) {
			if (right_temp == true) {
				Instantiate (particle, new Vector3(rightposition.x+1.0f, rightposition.y, rightposition.z-0.5f), transform.rotation);
				garupan_score++;
			}
			if (left_temp == true) {
				Instantiate (particle2, new Vector3(leftposition.x-1.0f, leftposition.y, leftposition.z-0.5f), transform.rotation);
				garupan_score++;
			}
		}
	}

	IEnumerator ending_garupan(){
		finishLabel.SetActive (true);
		yield return new WaitForSeconds (3);	//5秒待つ
		EndingGame_garupan ();			//シーン遷移
	}

	void EndingGame_garupan(){
		getPunchScore ();
		SceneManager.LoadScene ("garupanResult");
	}

	public static int getPunchScore(){	//result画面に数値を送る用
		return garupan_score;
	}

	public void ReturnToMenu(){
		_audioSource2.Play ();
		FadeManager.Instance.LoadScene ("StartScene_2",1.0f);
	}

}
