using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour {

    public Transform projectilePrefab;

    public string button="Fire1";
    public float cooldown = 0.5f;
    private float timer;

	// Update is called once per frame
	void Update () {
		if (Input.GetButton(button) && timer<0)
        {
            timer = cooldown;
            Instantiate(projectilePrefab, this.transform.position, this.transform.rotation);
        }
        timer -= Time.deltaTime;
        if (DiscordPlayerControl.rapid_fire_active > 0) timer -= 4 * Time.deltaTime;
	}
}
