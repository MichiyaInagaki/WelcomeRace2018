using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesTimingMaker2 : MonoBehaviour {

	public GameObject gameobject;
	bool LeftPanch_temp = false;
	bool LeftUpper_temp = false;
	bool LeftKnee_temp = false;
	bool RightPanch_temp = false;
	bool RightUpper_temp = false;
	bool RightKnee_temp = false;
	bool RightAvoid_temp = false;
	bool LeftAvoid_temp = false;

	private AudioSource _audioSource;
	private float _startTime = 0;
	private CSVWriter2 _CSVWriter;

	private bool _isPlaying = false;
	public GameObject startButton;

	private double GetTimingFlag;

	void Start () {
		_audioSource = GameObject.Find("GameMusic").GetComponent<AudioSource> ();
		_CSVWriter = GameObject.Find ("CSVWriter2").GetComponent<CSVWriter2> ();
	}

	void Update () {
		if (_isPlaying) {
			DetectKeys ();
		}
		LeftPanch_temp = gameobject.GetComponent<CubemanController> ().LeftPanch_detected;	//左パンチ検出のフラグ読み込み
		LeftUpper_temp = gameobject.GetComponent<CubemanController> ().LeftUpper_detected;
		LeftKnee_temp = gameobject.GetComponent<CubemanController> ().LeftKnee_detected;
		RightPanch_temp = gameobject.GetComponent<CubemanController> ().RightPanch_detected;	
		RightUpper_temp = gameobject.GetComponent<CubemanController> ().RightUpper_detected;
		RightKnee_temp = gameobject.GetComponent<CubemanController> ().RightKnee_detected;
		RightAvoid_temp = gameobject.GetComponent<CubemanController> ().RightAvoid_detected;
		LeftAvoid_temp = gameobject.GetComponent<CubemanController> ().LeftAvoid_detected;
	}

	public void StartMusic(){
		startButton.SetActive (false);
		_audioSource.Play ();
		_startTime = Time.time;
		_isPlaying = true;
	}

	void DetectKeys(){
		if (RightPanch_temp == true) {
			//Debug.Log ("RP");
			WriteNotesTiming (0);
		}

		if (RightUpper_temp == true) {
			//Debug.Log ("RU");
			WriteNotesTiming (1);
		}

		if (RightKnee_temp == true) {
			//Debug.Log ("RK");
			WriteNotesTiming (2);
		}

		if (LeftPanch_temp == true) {
			//Debug.Log ("LP");
			WriteNotesTiming (3);
		}

		if (LeftUpper_temp == true) {
			//Debug.Log ("LU");
			WriteNotesTiming (4);
		}

		if (LeftKnee_temp == true) {
			//Debug.Log ("LK");
			WriteNotesTiming (5);
		}

		if (RightAvoid_temp == true) {
			//Debug.Log ("LK");
			WriteNotesTiming (6);
		}

		if (LeftAvoid_temp == true) {
			//Debug.Log ("LK");
			WriteNotesTiming (7);
		}
	}

	void WriteNotesTiming(int num){

		//Debug.Log (GetTiming ());
		if (GetTiming() - GetTimingFlag > 0.3) {
			_CSVWriter.WriteCSV (GetTiming ().ToString () + "," + num.ToString());
		}
		GetTimingFlag = GetTiming();
	}

	float GetTiming(){
		return Time.time - _startTime;
	}

}
