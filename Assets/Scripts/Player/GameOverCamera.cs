using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCamera : MonoBehaviour {

    public float rotationSpeed = 60;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(transform.parent.position, Vector3.up, 
            rotationSpeed*Time.unscaledDeltaTime);
	}
}
