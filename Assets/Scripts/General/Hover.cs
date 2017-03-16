using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour {

    public float amplitude=1;
    public float frequency=1;
    private float timer;

    void Start()
    {
        timer += Random.Range(0, 100);
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 offset = new Vector3(0, amplitude * Time.deltaTime * Mathf.Sin(timer += (frequency*Time.deltaTime)), 0);
        this.transform.position += offset;
	}
}
