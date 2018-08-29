using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject gameobject;		//cubeman用
	private bool LeftUpper_temp = false;
	private bool RightUpper_temp = false;

	private bool DoubleUpper_temp = false;	//ゲーム中断用
	private float countdown = 4.0f;
	private int countdownseconds;
	private bool interruption_temp = false;				//中断用テンプ

	public GameObject[] cube;
	float[,] cubePosition;

	private float[] _timing;	//ノーツのタイミング
	private int[] _lineNum;		//ノーツの種類 		//0:RP /1:RE /2:RK /3:LP /4:LE /5:LK 
	public string filePass;
	private int _notesCount = 0;	//ノーツの順番

	private AudioSource _audioSource;	//音楽取得用
	private AudioSource _audioSource2;	//音楽取得用
	private float _startTime = 0;

	public float timeOffset = -1;		//落ちるまでの時間

	private bool _isPlaying = false;	//開始フラグ
	public GameObject startButton;
	public GameObject intro;

	public Text scoreText;				//スコア表示用
	public Text comboText;				//コンボ表示用
	public static int _score = 0;
	private int _combo = 0;
	public static int _maxcombo = 0;
	private int bonus_temp = 0;			//ボーナス得点計算用
	private int bonus_temp2 = 0;
	public GameObject combo_particle;
	public Text ten_combolabel;
	public GameObject ten_combolabel2;

	//public Text kcaltext;				//消費カロリー
	//public static float _kcal;

	private int stage;

	public GameObject finishLabel;

	// Use this for initialization
	void Start () {
		_audioSource = GameObject.Find ("GameMusic").GetComponent<AudioSource> ();
		_audioSource2 = GameObject.Find ("GameSE").GetComponent<AudioSource> ();
		_timing = new float[1024];
		_lineNum = new int[1024];
		//落下場所
		cubePosition = new float[,]{{3.0f, 5.0f, 0.0f},{3.0f, 5.0f, 0.0f},{3.0f, 5.0f, 0.0f},{-3.0f, 5.0f, 0.0f},{-3.0f, 5.0f, 0.0f},{-3.0f, 5.0f, 0.0f},{3.0f, 5.0f, 0.0f},{-3.0f, 5.0f, 0.0f} };
		LoadCSV();
		_score = 0;		//スコアの初期化
		_maxcombo = 0;
		//_kcal = 0;
		stage = StageSelectController2.getStage();
	}

	// Update is called once per frame
	void Update () {

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

		//ゲーム開始前
		if (_isPlaying == false) {
			LeftUpper_temp = gameobject.GetComponent<CubemanController> ().LeftUpper_detected;
			RightUpper_temp = gameobject.GetComponent<CubemanController> ().RightUpper_detected;

			if (LeftUpper_temp == true || RightUpper_temp == true) {	//アッパーでStart
				StartGame ();
			}
		}

		//ゲーム開始後
		if (_isPlaying) {
			CheckNextNotes ();
			scoreText.text = "Score: "+_score.ToString ();
			comboText.text = "Combo: "+_combo.ToString ();
			//kcaltext.text = "Calorie Consumption: " + _kcal.ToString ("N1") +" kcal";
		}

	}

	public void StartGame(){
		//Debug.Log ("startgame");
		_audioSource2.Play ();
		startButton.SetActive (false);		//スタートボタン消す
		intro.SetActive (false);			//"Upper to start"を消す
		_startTime = Time.time;				//スタートした時間
		_audioSource.Play ();				//音楽再生
		_isPlaying = true;					//開始フラグ・オン
	}

	void CheckNextNotes(){
		//落ちる時間を考慮したタイミング　＜　現在の時間　かつ　ノーツが終わりでない
		while (_timing [_notesCount] + timeOffset < GetMusicTime () && _timing [_notesCount] != 0) {
			SpawnNotes (_lineNum[_notesCount]);
			_notesCount++;		//次のノーツの時間閾値へ
		}
		if(_timing [_notesCount] == 0){
			StartCoroutine ("sleep");
			//Debug.Log ("end");
		}
	}

	void SpawnNotes(int num){
		Instantiate (cube[num], new Vector3(cubePosition[num,0],cubePosition[num,1],cubePosition[num,2]), Quaternion.identity);
	}

	//CSVファイルの読み込み
	void LoadCSV(){
		int i = 0, j;
		TextAsset csv = Resources.Load (filePass) as TextAsset;
		StringReader reader = new StringReader (csv.text);
		while (reader.Peek () > -1) {
			string line = reader.ReadLine ();
			string[] values = line.Split (',');
			for (j = 0; j < values.Length; j++) {
				_timing [i] = float.Parse (values[0]);
				_lineNum [i] = int.Parse (values [1]);
			}
			i++;
		}
	}

	float GetMusicTime(){
		return Time.time - _startTime;		//音声再生からの時間を返す
		//Debug.Log(Time.time - _startTime);
	}

	public void GoodTimingFunc(int num){
		//Debug.Log ("Line:" + num + " good!");
		//Debug.Log (GetMusicTime());
		//_score++; 
		_combo++;							//コンボ加算
		if (_combo % 10 == 0 && _combo != 0) {
			bonus_temp++;
			Instantiate (combo_particle, new Vector3(0f,0f,1.0f), Quaternion.identity);		//コンボエフェクトの再生
			ten_combolabel.text = (bonus_temp*10).ToString ()+"Combo!";
			ten_combolabel2.SetActive (true);
			StartCoroutine ("sleep_03");
			bonus_temp2 = bonus_temp;
			if (bonus_temp > 3) {
				bonus_temp2 = 4;
			}
		}
		_score = _score + (100 + 25 * bonus_temp2);	//スコア計算：基本点１００点、１０コンボで１．２５倍、２０コンボで１．５倍

		if (_maxcombo < _combo) {
			_maxcombo = _combo;			//maxcomboの取得
		}

		/*if (num == 2 || num == 5) {
			_kcal += 0.2f;				//キックは0.2kcal
		} else {
			_kcal += 0.1f;
		}*/

	}

	public void MissDetectFunc(int num2){	//missの判定
		//Debug.Log ("miss_detected!");
		_combo = 0;
		bonus_temp = 0;
		if (num2 == 1) {
			_score = _score - 300;			//dangerならスコア減点
		}
	}

	IEnumerator sleep(){
		//Debug.Log ("開始");
		yield return new WaitForSeconds (6);	//5秒待つ
		//Debug.Log("5秒経過");
		finishLabel.SetActive (true);			//finish表示
		yield return new WaitForSeconds (5);
		//Debug.Log ("8秒経過");
		EndGame ();			//シーン遷移
	}

	IEnumerator sleep_03(){
		yield return new WaitForSeconds (0.5f);	//5秒待つ
		ten_combolabel2.SetActive (false);			//finish表示
	}


	void EndGame(){			//シーン遷移用関数
		//Debug.Log ("result scene");
		getScore ();
		getMaxcombo ();
		if (stage == 1) {
			SceneManager.LoadScene ("Stage1Result");
		} else if (stage == 2) {
			SceneManager.LoadScene ("Stage2Result");
		} else {
			SceneManager.LoadScene ("Stage3Result");
		}
	}

	public static int getScore(){	//result画面に数値を送る用
		return _score;
	}

	public static int getMaxcombo(){	//result画面に数値を送る用
		return _maxcombo;
	}

	/*public static float getKcal(){
		return _kcal;
	}*/

	public void ReturnToMenu(){
		_audioSource2.Play ();
		FadeManager.Instance.LoadScene ("StartScene_2",1.0f);
	}

}
