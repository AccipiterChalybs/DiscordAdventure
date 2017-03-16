using UnityEngine;
using System.Collections;

public class RandomScaleConstructor : MonoBehaviour {

    public float minScale;
    public float maxScale;

	// Use this for initialization
	void Start () {
        float scale = Random.Range(minScale, maxScale);
        this.transform.localScale = new Vector3(scale, scale, scale);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
