using UnityEngine;
using System.Collections;

public class ZoomInOut : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.KeypadPlus))
		{
			Camera.main.transform.Translate (Vector3.forward);
		}

		if(Input.GetKey(KeyCode.KeypadMinus))
		{
			Camera.main.transform.Translate (-Vector3.forward);
		}

	}
}
