using UnityEngine;
using System.Collections;

public class RainCloud : MonoBehaviour {
    public Transform prefabToRain;

    public float radius = 7.5f;

    public float cooldown = 2.5f;

    private float cooldownTimer;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0)
        {
            Vector2 pt = Random.insideUnitCircle;
            Instantiate(prefabToRain, transform.position + radius * new Vector3(pt.x, 0, pt.y), Random.rotation);

            cooldownTimer = cooldown;
        }

	}
}
