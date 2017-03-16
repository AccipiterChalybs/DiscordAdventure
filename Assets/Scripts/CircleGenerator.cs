using UnityEngine;
using System.Collections;

public class CircleGenerator : MonoBehaviour
{

    public Transform prefabToSpawn;

    public float radius = 3;
    public int segments;
    public bool randomOffset = false;
    public float fixedOffset = 0;

    // Use this for initialization
    void Start()
    {
        float angleOffset = (randomOffset) ? (float) Random.Range(0, 2*Mathf.PI) : fixedOffset;
        for (int i = 0; i < segments; ++i)
        {
            float x = radius * Mathf.Cos(angleOffset + i * 2.0f * Mathf.PI / segments);
            float z = radius * Mathf.Sin(angleOffset + i * 2.0f * Mathf.PI / segments);
            Instantiate(prefabToSpawn, this.transform.position + new Vector3(x, 0, z), Quaternion.identity);
        }

    }
}
