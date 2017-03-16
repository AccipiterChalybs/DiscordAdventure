using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DramaticEntrance : MonoBehaviour {
    public Transform effectPrefab;

	// Use this for initialization
	void Start () {
        Instantiate(effectPrefab, this.transform.position, this.transform.rotation);
	}
}
