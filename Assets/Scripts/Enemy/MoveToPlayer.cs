using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToPlayer : MonoBehaviour {

    public NavMeshAgent trackerPrefab;

    public NavMeshAgent agent;

    public Transform target;

    private Rigidbody rb;

    public float updatePathInterval=1;
    private float updateTimer;

    public float moveForce = 5;
    public float rotationSpeed = 5;

    private Vector3 upVec = new Vector3(0, 0.25f,0);

	// Use this for initialization
	void Start () {
        agent = Instantiate(trackerPrefab, transform.position, Quaternion.identity);

        //TODO should update to better way to do this
        target = FindObjectOfType<PlayerController2>().transform;

        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (updateTimer > updatePathInterval)
        {
            agent.transform.position = this.transform.position;
            agent.SetDestination(target.position);
            updateTimer = 0;
        }
        updateTimer += Time.deltaTime;


        Vector3 dir = (agent.transform.position - transform.position);
        if (dir.magnitude < 0.25f) dir = agent.transform.forward;
        Vector3 directTarget = (target.position - transform.position);
        if (directTarget.magnitude < 3) dir = directTarget;
        dir = dir.normalized;


        if (DiscordPlayerControl.chaos_control_active > 0)
        {
            rb.isKinematic = true;
        } else if (rb.isKinematic)
        {
            rb.isKinematic = false;
        }


        rb.AddForceAtPosition(dir * moveForce, this.transform.position + upVec);
        //this.transform.Rotate(Vector3.Cross(dir, Vector3.up), rotationSpeed);
    }

    void OnCollisionEnter(Collision col)
    {
        if (Vector3.Dot(Vector3.up, col.contacts[0].normal) < 0.5f)
        {
            Vector3 newV = rb.velocity;
            newV += 3*(target.position - transform.position).normalized;
            newV.y = (10);
            rb.velocity = newV;
        }
    }
}