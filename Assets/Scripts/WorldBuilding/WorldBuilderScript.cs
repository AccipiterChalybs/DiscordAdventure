using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilderScript : MonoBehaviour {

    public Transform landmassPrefab;
    public Transform treePrefab;

    public void BuildWorld()
    {
        for (int i=0; i<20; ++i)
        {
            float x = Random.Range(-10.0f, 10.0f) * Random.Range(-10f, 10f);
            float z = Random.Range(-10.0f, 10.0f) * Random.Range(-10f, 10f);
            x += (10-Mathf.Abs(z) / 10) * Random.Range(-1.0f, 1.0f);
            Transform landmass = Instantiate(landmassPrefab, new Vector3(x, 10-i / 5.0f, z), Quaternion.Euler(0, Random.Range(0, 4)*360, 0));
            landmass.parent = this.transform;
        }
    }
}
