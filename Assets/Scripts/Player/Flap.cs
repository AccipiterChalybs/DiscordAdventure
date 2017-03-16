using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flap : MonoBehaviour {

    public float startRotation;
    public float endRotation;
    public float flapSpeed = 1;

    private float t;
    private Quaternion startQuat;
    private Quaternion endQuat;

	// Use this for initialization
	void Start () {
        startQuat = Quaternion.Euler(0, startRotation, 0);
        endQuat = Quaternion.Euler(0, endRotation, 0);
	}
	
	// Update is called once per frame
	void Update () {
        t += Time.deltaTime * flapSpeed;
        int tInteger = (int)t;
        float flagPercent = (tInteger % 2 == 0) ? 1 - (t - tInteger) : (t - tInteger);
        this.transform.localRotation = Quaternion.Slerp(startQuat, endQuat, flagPercent);
	}
}
