using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCubeMove : MonoBehaviour {

	float speed = 80f;
	public Rigidbody rb;
	public bool cube_missdetected = false;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.AddForce (0,-speed,0);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
