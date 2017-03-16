using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwards : MonoBehaviour {
    public float moveSpeed = 5;
	
	
    public void Start()
    {
        this.GetComponent<Rigidbody>().velocity = this.transform.forward * moveSpeed;
    }
}
