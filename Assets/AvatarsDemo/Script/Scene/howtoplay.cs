using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class howtoplay : MonoBehaviour {

	public GameObject gameobject;		//cubeman用
	private AudioSource _audioSource;	//音楽取得用
	private AudioSource _audioSource2;	//SE取得用
	private bool Fade_temp;
	private bool LeftUpper_temp = false;
	private bool RightUpper_temp = false;

	// Use this for initialization
	void Start () {
		Fade_temp = true;
		_audioSource = GameObject.Find ("GameMusic").GetComponent<AudioSource> ();
		_audioSource.Play ();				//音楽再生
		_audioSource2 = GameObject.Find ("GameSE").GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {							//マウス用
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
