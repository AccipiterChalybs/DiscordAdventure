using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMode : MonoBehaviour {

    public float cooldownMax = 5f;
    public float cooldownMin = 1f;
    private float timer;

    public string enemyTag = "Enemy";
    public Transform projectilePrefab;

    // Update is called once per frame
    void Update () {


        if (timer < 0)
        {
            timer = Random.Range(cooldownMin, cooldownMax);
            Collider[] hit = Physics.OverlapSphere(this.transform.position, 5);
            foreach (Collider c in hit)
            {
                if (c.CompareTag(enemyTag))
                {
                    Transform projectile = Instantiate(projectilePrefab, this.transform.position, Quaternion.LookRotation(c.transform.position - this.transform.position));
                    projectile.GetComponent<MeshRenderer>().material = this.GetComponent<MeshRenderer>().material;//this.GetComponent<Recolour>().GetMaterial();
                    return;
                }
            }
        }
        timer -= Time.deltaTime;
    }
}
