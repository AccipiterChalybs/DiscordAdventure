using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public Transform enemyToSpawn;
    public Transform spawnLocation;
    public float cooldownMax = 21f;
    public float cooldownMin = 10f;
    private float timer;
    	
	// Update is called once per frame
	void Update () {

        if (timer < 0)
        {
            timer = Random.Range(cooldownMin, cooldownMax);
            Instantiate(enemyToSpawn, spawnLocation.position, Quaternion.identity);
            if (cooldownMax - 2 > cooldownMin) cooldownMax -= 2;
            if (cooldownMin - 1 > 1) cooldownMin -= 1;
        }
        timer -= Time.deltaTime;
    }
}
