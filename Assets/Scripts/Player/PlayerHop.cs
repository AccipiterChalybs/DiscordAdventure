using UnityEngine;
using System.Collections;

public class PlayerHop : MonoBehaviour {

    public float hopForce = 2.5f;

    public float megaHopForce = 10;

    public float cooldown = 0.1f;
    private float cooldownTimer = 0;

    public int jumpNum;
    private int jumpsAvailable = 1;

    private PlayerController2 pc;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        pc = GetComponent<PlayerController2>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (jumpsAvailable>0 && Input.GetButton("Jump"))
        {
            rb.AddForce(megaHopForce * Vector3.up, ForceMode.Impulse);
            jumpsAvailable--;
        }
    }
	

    void OnCollisionStay(Collision other)
    {
        if (Vector3.Dot(Vector3.up, other.contacts[0].normal) > 0.81f)
        {
            if (pc.hopping && cooldownTimer <=0)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(hopForce * Vector3.up, ForceMode.Impulse);
                cooldownTimer = cooldown;
                jumpsAvailable = jumpNum;
            }
        }
    }
}
