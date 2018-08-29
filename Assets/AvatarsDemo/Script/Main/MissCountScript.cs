using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissCountScript : MonoBehaviour {

	public bool missdetect = false;
	private GameController _gameManager;

	// Use this for initialization
	void Start () {
		_gameManager = GameObject.Find ("GameController").GetComponent<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider hit){
		if (hit.CompareTag ("RightDangerCube") || hit.CompareTag ("LeftDangerCube")) {
			Destroy (hit.gameObject);			//DangerCubeなら回避成功で何もしない
		}else{
			_gameManager.MissDetectFunc (0);	//他のブロックなら、miss検出→ゲームコントローラーへ
			Destroy (hit.gameObject);
		}
	}
		
}
