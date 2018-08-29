using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject[] notes;
	private float[] _timing;		//CSVの記録時間
	private int[] _lineNum;			//CSVのノーツ種類番号

	public string filePass;
	private int _notesCount = 0;	//何番目のノーツか

	private AudioSource _audioSource;	//音楽取得用
	private float _startTime = 0;

	public float timeOffset = -1;		//落ちるまでの時間

	private bool _isPlaying = false;	//開始フラグ
	public GameObject startButton;

	public Text scoreText;				//スコア表示用
	private int _score = 0;

	void Start(){
		_audioSource = GameObject.Find ("GameMusic").GetComponent<AudioSource> ();
		_timing = new float[1024];
		_lineNum = new int[1024];
		LoadCSV ();
	}

	void Update () {
		if (_isPlaying) {
			CheckNextNotes ();
			scoreText.text = _score.ToString ();
		}

	}

	public void StartGame(){
		startButton.SetActive (false);		//スタートボタン消す
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
	}

	void SpawnNotes(int num){
		Instantiate (notes[num], 
			new Vector3 (-4.0f + (2.0f * num), 10.0f, 0),
			Quaternion.identity);
	}

	void LoadCSV(){
		int i = 0, j;
		TextAsset csv = Resources.Load (filePass) as TextAsset;
		StringReader reader = new StringReader (csv.text);
		while (reader.Peek () > -1) {

			string line = reader.ReadLine ();
			string[] values = line.Split (',');
			for (j = 0; j < values.Length; j++) {
				_timing [i] = float.Parse( values [0] );
				_lineNum [i] = int.Parse( values [1] );
			}
			i++;
		}
	}

	float GetMusicTime(){
		return Time.time - _startTime;		//音声再生からの時間を返す
	}

	public void GoodTimingFunc(int num){
		Debug.Log ("Line:" + num + " good!");
		Debug.Log (GetMusicTime());
		_score++;
	}

}
