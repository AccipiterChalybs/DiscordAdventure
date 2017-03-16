using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedActivation : MonoBehaviour {
    
    public float activateAfter = 10f;
	
	// Update is called once per frame
	void Update () {
        activateAfter -= Time.deltaTime;
        if (activateAfter < 0)
        {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<DramaticEntrance>().enabled = true;
            GetComponent<MoveToPlayer>().enabled = true;
        }
	}
}
