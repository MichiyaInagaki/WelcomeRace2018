using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Kimisui : MonoBehaviour {

	public GameObject gameobject2;		//cubeman用
	private bool LeftUpper_temp = false;
	private bool RightUpper_temp = false;

	private bool DoubleUpper_temp = false;	//ゲーム中断用
	private float countdown2 = 4.0f;
	private int countdownseconds2;
	private bool interruption_temp = false;				//中断用テンプ

	public GameObject gameobject;
	public GameObject gameoverLabel;
	public GameObject titlelogo;
	public Text timerText;
	public Text countDownText;
	public GameObject intro;
	public GameObject intro2;

	private int minute;
	private float seconds;
	private float oldSeconds;
	private bool gameoverflag;
	private float countdown;
	private int countdownseconds;
	public bool countdownflag = true;

	private bool _isPlaying = false;	//開始フラグ
	private AudioSource _audioSource;	//音楽取得用
	private AudioSource _audioSource2;	//音楽取得用
	public GameObject startButton2;

	public static float _timescore_second = 0;
	public static int _timescore_minute = 0;


	// Use this for initialization
	void Start () {
		_audioSource = GameObject.Find ("GameMusic").GetComponent<AudioSource> ();
		_audioSource2 = GameObject.Find ("GameSE").GetComponent<AudioSource> ();
		minute = 0;
		seconds = 0f;
		oldSeconds = 0f;
		gameoverflag = false;
		countdown = 4.0f;			//3秒前からカウントダウン
		countdownflag = true;
		_audioSource.Play ();				//音楽再生
	}


	// Update is called once per frame
	void Update () {

		//ゲーム中断用
		if (interruption_temp == false) {
			DoubleUpper_temp = gameobject2.GetComponent<CubemanController> ().DoubleUpper_detected;
			if (DoubleUpper_temp == true) {			//ダブルアッパー検出
				countdown2 -= Time.deltaTime;		//カウントダウン
				Debug.Log (countdown2);
				countdownseconds2 = (int)countdown2;
				if (countdownseconds2 == 0) {
					interruption_temp = true;
					ReturnToMenu ();				//メニューへ
				}
			} else {
				countdown2 = 4.0f;
			}
		}

		if (_isPlaying == false) {
			LeftUpper_temp = gameobject2.GetComponent<CubemanController> ().LeftUpper_detected;
			RightUpper_temp = gameobject2.GetComponent<CubemanController> ().RightUpper_detected;

			if (LeftUpper_temp == true || RightUpper_temp == true) {	//アッパーで選択
				StartkimisuiGame ();
			}
		}

		if (_isPlaying) {
			Destroy (titlelogo,3.0f);
			Destroy (intro);
			Destroy (intro2);
			countDownText.enabled = true;
			countdown -= Time.deltaTime;		//カウントダウン
			//Debug.Log (countdown);
			countdownseconds = (int)countdown;
			countDownText.text = countdownseconds.ToString ();
			if (countdownseconds == 0) {
				countdownflag = false;
				countDownText.enabled = false;
			}


			if (gameoverflag == false && countdownflag == false) {	//経過時間
				seconds += Time.deltaTime;
				if (seconds >= 60f) {
					minute++;
					seconds = seconds - 60;
				}
				if ((int)seconds != (int)oldSeconds) {
					timerText.text = minute.ToString ("00") + ":" + ((int)seconds).ToString ("00");
				}
				oldSeconds = seconds;
			}

			Vector3 BallPosition = gameobject.transform.position;
			if (BallPosition.y <= -10) {
				//Debug.Log ("GameOver!");
				gameoverLabel.SetActive (true);
				gameoverflag = true;
				_timescore_second = seconds;
				_timescore_minute = minute;
				StartCoroutine ("ending");
			}
		}
	}
		
	public void StartkimisuiGame(){
		_audioSource2.Play ();
		startButton2.SetActive (false);		//スタートボタン消す

		_isPlaying = true;					//開始フラグ・オン
	}

	IEnumerator ending(){
		yield return new WaitForSeconds (3);	//5秒待つ
		EndingGame ();			//シーン遷移
	}

	void EndingGame(){
		getTimeScoreSecond ();
		getTimeScoreMinute ();
		SceneManager.LoadScene ("kimisuiResult");
	}

	public static float getTimeScoreSecond(){	//result画面に数値を送る用
		return _timescore_second;
	}

	public static int getTimeScoreMinute(){	//result画面に数値を送る用
		return _timescore_minute;
	}

	public void ReturnToMenu(){
		_audioSource2.Play ();
		FadeManager.Instance.LoadScene ("StartScene_2",1.0f);
	}

}
