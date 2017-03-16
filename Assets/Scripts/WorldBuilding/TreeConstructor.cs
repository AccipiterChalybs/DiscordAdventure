using UnityEngine;
using System.Collections;

public class TreeConstructor : MonoBehaviour {

    public Transform treeTop;
    public Transform treeTrunk;

    public float minTopScale = 1.5f;
    public float maxTopScale = 2.5f;

    public float minExtraScale = 1;
    public float maxExtraScale = 2;

    public BoxCollider safeZone;
    public bool checkCollision = false;

	// Use this for initialization
	void Start () {
        Vector3 center = safeZone.center + this.transform.position;
        Vector3 extents = safeZone.size/2.0f-new Vector3(0.01f, 0.01f, 0.01f);
        safeZone.enabled = false;

        treeTop.GetComponent<BoxCollider>().enabled = false;
        treeTrunk.GetComponent<BoxCollider>().enabled = false;


        if (checkCollision)
        {
            if (Physics.CheckBox(center, extents, this.transform.rotation))
            {
                foreach (Collider oa in Physics.OverlapBox(center, extents))
                    print(oa.gameObject.name);

                print("Collision");
                Destroy(this.gameObject);
                return;
            }
        }

        treeTop.GetComponent<BoxCollider>().enabled = true;
        treeTrunk.GetComponent<BoxCollider>().enabled = true;

        Collider[] hits = Physics.OverlapSphere(this.transform.position, 0.25f);
        foreach (Collider col in hits)
        {
           // if (col.gameObject.CompareTag("No Spawn Area"))
            {
        //        Destroy(this.gameObject);
            }
        }

        float topScale = Random.Range(minTopScale, maxTopScale);
        treeTop.localScale = new Vector3(topScale, topScale, topScale);

        float extraScale = Random.Range(minExtraScale, maxExtraScale);
        this.transform.localScale = new Vector3(extraScale, extraScale, extraScale);
	}
}
