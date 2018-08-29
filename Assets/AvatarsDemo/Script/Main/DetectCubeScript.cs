using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DetectCubeScript : MonoBehaviour {

	public GameObject gameobject;		//cubeman
	public GameObject particle;
	public GameObject particle2;

	private GameController _gameManager;	//GameController

	bool LeftPanch_temp = false;
	bool LeftUpper_temp = false;
	bool LeftKnee_temp = false;
	bool RightPanch_temp = false;
	bool RightUpper_temp = false;
	bool RightKnee_temp = false;

	private Vector3 AvoidRotate;

	//public Text kcaltext;				//消費カロリー
	//public static float _kcal;

	void Start(){
		_gameManager = GameObject.Find ("GameController").GetComponent<GameController> ();
		//_kcal = 0;
	}

	void Update(){
		LeftPanch_temp = gameobject.GetComponent<CubemanController> ().LeftPanch_detected;	//左パンチ検出のフラグ読み込み
		LeftUpper_temp = gameobject.GetComponent<CubemanController> ().LeftUpper_detected;
		LeftKnee_temp = gameobject.GetComponent<CubemanController> ().LeftKnee_detected;
		RightPanch_temp = gameobject.GetComponent<CubemanController> ().RightPanch_detected;	
		RightUpper_temp = gameobject.GetComponent<CubemanController> ().RightUpper_detected;
		RightKnee_temp = gameobject.GetComponent<CubemanController> ().RightKnee_detected;
		AvoidRotate = gameobject.GetComponent<CubemanController> ().AvoidRotation;
		//Debug.Log ("Rotation: "+AvoidRotate.z);
		//KcalDetect();
	}

	void OnTriggerStay (Collider hit){
		
		if (hit.CompareTag ("RightPunchCube") && RightPanch_temp==true) {
			Instantiate (particle, transform.position, transform.rotation);		//パーティクルエフェクトの発生
			_gameManager.GoodTimingFunc (0);		//GameControllerスクリプトのGoodTimingFunc関数に値を渡す
			Destroy (hit.gameObject);		//hitした物体を消す
			RightPanch_temp = false;
		}

		if (hit.CompareTag ("RightUpperCube") && RightUpper_temp==true) {
			//Debug.Log ("HitRightUpper!");
			Instantiate (particle, transform.position, transform.rotation);
			_gameManager.GoodTimingFunc (1);
			Destroy (hit.gameObject);
			RightUpper_temp = false;
		}

		if (hit.CompareTag ("RightKneeCube") && RightKnee_temp==true) {
			//Debug.Log ("HitRightKnee!");
			Instantiate (particle, transform.position, transform.rotation);
			_gameManager.GoodTimingFunc (2);
			Destroy (hit.gameObject);
			RightKnee_temp = false;
		}

		if (hit.CompareTag ("LeftPunchCube") && LeftPanch_temp==true) {
			//Debug.Log ("HitLeftPunch!");
			Instantiate (particle, transform.position, transform.rotation);
			_gameManager.GoodTimingFunc (3);
			Destroy (hit.gameObject);
			LeftPanch_temp = false;
		}

		if (hit.CompareTag ("LeftUpperCube") && LeftUpper_temp==true) {
			//Debug.Log ("HitLeftUpper!");
			Instantiate (particle, transform.position, transform.rotation);
			_gameManager.GoodTimingFunc (4);
			Destroy (hit.gameObject);
			LeftUpper_temp = false;
		}

		if (hit.CompareTag ("LeftKneeCube") && LeftKnee_temp==true) {
			//Debug.Log ("HitLeftKnee!");
			Instantiate (particle, transform.position, transform.rotation);
			_gameManager.GoodTimingFunc (5);
			Destroy (hit.gameObject);
			LeftKnee_temp = false;
		}
	}

	void OnTriggerEnter (Collider hit){

		if (hit.CompareTag ("RightDangerCube") && (AvoidRotate.z > 85)) {
			Instantiate (particle2, transform.position, transform.rotation);
			_gameManager.MissDetectFunc (1);
			Destroy (hit.gameObject);
		}

		if (hit.CompareTag ("LeftDangerCube") && (AvoidRotate.z < 95)) {
			Instantiate (particle2, transform.position, transform.rotation);
			_gameManager.MissDetectFunc (1);
			Destroy (hit.gameObject);
		}
	}

	/*public void KcalDetect(){				//カロリー計算
		if (RightKnee_temp == true || LeftKnee_temp == true) {
			_kcal += 0.2f;				//キックは0.2kcal
		} else if(LeftPanch_temp==true||LeftUpper_temp==true||RightPanch_temp==true||RightUpper_temp==true){
			_kcal += 0.1f;
		}
		kcaltext.text = "Calorie Consumption: " + _kcal.ToString ("N1") +" kcal";
	}

	public static float getKcal(){
		return _kcal;
	}*/

}
