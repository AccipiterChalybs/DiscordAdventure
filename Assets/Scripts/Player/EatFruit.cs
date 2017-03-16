using UnityEngine;
using System.Collections;

public class EatFruit : MonoBehaviour {

    public string FRUIT_TAG = "Fruit";

    public Transform playerMesh;

    private float startingMass;

    public float massGain = 0.25f;
    public float massLoss = 0.25f;

    private Rigidbody rb;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        startingMass = rb.mass;
    }

    void Update()
    {
        rb.mass -= massLoss * Time.deltaTime;
        if (rb.mass < startingMass) rb.mass = startingMass;
        float currentMass = rb.mass;
        playerMesh.localScale = new Vector3(currentMass / startingMass, currentMass / startingMass, currentMass / startingMass);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(FRUIT_TAG))
        {
            float currentMass = rb.mass;
            currentMass += massGain;
            rb.mass = currentMass;
            playerMesh.localScale = new Vector3(currentMass / startingMass, currentMass / startingMass, currentMass / startingMass);
            Destroy(other.gameObject);
        }
    }
}
