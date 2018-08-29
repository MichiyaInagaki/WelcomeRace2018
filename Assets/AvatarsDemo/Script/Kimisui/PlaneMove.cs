using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMove : MonoBehaviour {

	public GameObject gameobject;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		Vector3 HandRotate = gameobject.GetComponent<CubemanController> ().HandRotation;
		Vector3 SpineRotate = gameobject.GetComponent<CubemanController> ().SpineRotation;
		transform.eulerAngles = new Vector3(SpineRotate.x, HandRotate.y, HandRotate.z);
	}
}
