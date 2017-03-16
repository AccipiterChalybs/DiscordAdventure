using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNearPlayer : MonoBehaviour {

    public Transform player;
    public Vector3 offsetVec = new Vector3(0, 0.25f, 0);
    public float range = 2;
    public float speed = 1;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController2>().transform;
	}
	
	// Update is called once per frame
	void Update () {
		if ((player.position - this.transform.position).magnitude > range)
        {
            Vector3 moveVec = (player.position + offsetVec - this.transform.position).normalized;
            this.transform.forward = moveVec;
            this.transform.position += speed * Time.deltaTime * moveVec;
        }
	}
}
