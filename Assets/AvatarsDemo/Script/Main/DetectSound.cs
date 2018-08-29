using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectSound : MonoBehaviour {

	public AudioClip Sound;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource> ();
		audioSource.PlayOneShot (Sound);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
