using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour {

    private Rigidbody rb;

    public float backwardsPenalty = 0.5f;
    public float moveForce = 3.2f;
    public float rotationSpeed = 0.5f;

    public bool hopping;

    public bool disableMove;

    private Vector3 pushbackForce;
    public float pushbackForceLevel = 12;

	// Use this for initialization
	void Start ()
    {
        rb = this.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //-------Movement: just forward / back by chaning velocity 
        Vector3 direction = new Vector3(0, 0, 0);

        direction.z = Input.GetAxis("Vertical");

        if (direction.z < 0) direction.z /= 2.0f;

        if (!disableMove)
        rb.velocity = (rb.velocity.y) * Vector3.up + (direction.z * moveForce) * transform.forward + pushbackForce;

        pushbackForce *= 0.9f;
        disableMove = false;

        //--------Rotation: rotation around Y : direct so we can lock rigidbody rotation
        float rotationX = Input.GetAxis("Horizontal");

        Vector3 ea = this.transform.rotation.eulerAngles;
        if (Mathf.Abs(rotationX) > 0.10f)
        {
            //squared for more contrast - more precise slow rotation, but still can go fast
            ea.y += rotationX * Mathf.Abs(rotationX) * Time.deltaTime * rotationSpeed;
        }
        this.transform.rotation = Quaternion.Euler(ea);

        //if rotating or moving, make character do a short hop
        hopping = direction.magnitude > 0.25f || Mathf.Abs(rotationX) > 0.10f;

    }

    void OnCollisionEnter(Collision other)
    {
        if (Vector3.Dot(Vector3.up, other.contacts[0].normal) < 0.5f)
        {
            pushbackForce = other.contacts[0].normal*pushbackForceLevel;
            pushbackForce.y = 0;
            rb.velocity = (rb.velocity.y) * Vector3.up + pushbackForce;
            disableMove = true;
        }
        else
        {
            disableMove = false;
            pushbackForce = Vector3.zero;
        }
    }
}
