using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour {

    public bool all;
    public string enemyTag;

    public bool destroySelf;

    void OnTriggerEnter(Collider other)
    {
        if (all || other.CompareTag(enemyTag))
        {
            Destroyable destroyer = other.GetComponent<Destroyable>();
            if (destroyer) destroyer.Destroy();
        }
        if (destroySelf) Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision other)
    {
        if (all || other.collider.CompareTag(enemyTag))
        {
            Destroyable destroyer = other.collider.GetComponent<Destroyable>();
            if (destroyer) destroyer.Destroy();

        }
        if (destroySelf) Destroy(this.gameObject);
    }
}
