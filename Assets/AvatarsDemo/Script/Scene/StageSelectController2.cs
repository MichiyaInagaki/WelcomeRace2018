using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelectController2 : MonoBehaviour {

	//public GameObject startButton1;
	//public GameObject startButton2;
	//public GameObject startButton3;
	public GameObject gameobject;		//cubeman用
	private AudioSource _audioSource;	//音楽取得用
	private AudioSource _audioSource_SE;	//音楽取得用
	private AudioSource _audioSource_SE2;	//音楽取得用
	private bool Fade_temp;

	public static int _stage = 0;

	bool LeftPanch_temp = false;
	bool LeftUpper_temp = false;
	bool LeftKnee_temp = false;
	bool RightPanch_temp = false;
	bool RightUpper_temp = false;
	bool RightKnee_temp = false;
	private int num = 0;
	public RectTransform pointer;		//▶のポインター
	private float[,] PointerPosition;

	// Use this for initialization
	void Start () {
		Fade_temp = true;
		_audioSource = GameObject.Find ("GameMusic").GetComponent<AudioSource> ();
		_audioSource.Play ();				//音楽再生
		_audioSource_SE = GameObject.Find ("GameSE").GetComponent<AudioSource> ();
		_audioSource_SE2 = GameObject.Find ("GameSE2").GetComponent<AudioSource> ();
		pointer = GameObject.Find ("Pointer").GetComponent<RectTransform> ();		//pointerの取得
		PointerPosition = new float[,]{{-310f, -125f, 0.0f},{-310f, -185f, 0.0f},{-310f, -248f, 0.0f},{110f, -125f, 0.0f},{110f, -185f, 0.0f},{355f, 255f, 0.0f}};
	}

	// Update is called once per frame
	void Update () {
		LeftPanch_temp = gameobject.GetComponent<CubemanController> ().LeftPanch_detected;	//左パンチ検出のフラグ読み込み
		LeftUpper_temp = gameobject.GetComponent<CubemanController> ().LeftUpper_detected;
		LeftKnee_temp = gameobject.GetComponent<CubemanController> ().LeftKnee_detected;
		RightPanch_temp = gameobject.GetComponent<CubemanController> ().RightPanch_detected;	
		RightUpper_temp = gameobject.GetComponent<CubemanController> ().RightUpper_detected;
		RightKnee_temp = gameobject.GetComponent<CubemanController> ().RightKnee_detected;
		if (Fade_temp == true) {
			KinectStageSelect ();
		}
	}

	public void KinectStageSelect(){
		if (RightKnee_temp == true) {			//ひざでセレクト
			_audioSource_SE2.Play ();
			num++;
			if (num > 5) {
				num = 0;
			}
		} else if (LeftKnee_temp == true) {
			_audioSource_SE2.Play ();
			num--;
			if (num < 0) {
				num = 5;
			}
		}

		pointer.localPosition = new Vector3 (PointerPosition[num,0],PointerPosition[num,1],PointerPosition[num,2]);		//pointerの位置制御

		if (LeftUpper_temp == true || RightUpper_temp == true) {	//アッパーで選択
			KinectSceneChange (num);	//シーン遷移用関数
		}
	}

	public void KinectSceneChange(int number){
		switch (number) {
		case 0:
			StartStage1();
			break;
		case 1:
			StartStage2 ();
			break;
		case 2:
			StartStage3 ();
			break;
		case 3:
			Kimisui ();
			break;
		case 4:
			Garupan ();
			break;
		case 5:
			Howtoplay ();
			break;
		}
	}



	public void StartStage1(){
		_audioSource_SE.Play ();
		FadeManager.Instance.LoadScene ("Stage1",1.0f);
		_stage = 1;
		Fade_temp = false;
	}

	public void StartStage2(){
		_audioSource_SE.Play ();
		FadeManager.Instance.LoadScene ("Stage2",1.0f);
		_stage = 2;
		Fade_temp = false;
	}

	public void StartStage3(){
		_audioSource_SE.Play ();
		FadeManager.Instance.LoadScene ("Stage3",1.0f);
		_stage = 3;
		Fade_temp = false;
	}

	public void Kimisui(){
		_audioSource_SE.Play ();
		FadeManager.Instance.LoadScene ("minigame1",1.0f);
		Fade_temp = false;
	}

	public void Garupan(){
		_audioSource_SE.Play ();
		FadeManager.Instance.LoadScene ("minigame2",1.0f);
		Fade_temp = false;
	}

	public void Howtoplay(){
		_audioSource_SE.Play ();
		FadeManager.Instance.LoadScene  ("HowToPlay",2.0f);
		Fade_temp = false;
	}

	public static int getStage(){	//result画面に数値を送る用
		return _stage;
	}

}
