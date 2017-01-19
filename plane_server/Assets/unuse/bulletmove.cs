using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletmove : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector3 (0, 0, 30 * Time.deltaTime),Space.Self);
		Destroy (gameObject, 10);
	}
}
