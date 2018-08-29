using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMove : MonoBehaviour {

	private float speed = 0.1f;
	private Rigidbody rb;
	public GameObject cd;

	// Use this for initialization
	void Start () {

		rb = this.GetComponent<Rigidbody> ();
		rb.useGravity = false;
		this.GetComponent<Rigidbody>().AddForce(
			(transform.forward) * speed,
			ForceMode.Impulse);
	}

	// Update is called once per frame
	void Update () {
		bool cdflag = cd.GetComponent<Kimisui> ().countdownflag;
		if (cdflag == false) {
			rb.useGravity = true;
		}
	}
}
