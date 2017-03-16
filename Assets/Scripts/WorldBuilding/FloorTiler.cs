using UnityEngine;
using System.Collections;

public class FloorTiler : MonoBehaviour {

    public Transform floor;

    public int xTile = 2;
    public int yTile = 2;

	// Use this for initialization
	void Start () {
	    for (int x = 0; x<xTile; ++x)
        {
            for (int y=0; y < yTile; ++y)
            {
                float xo = 2 * x - 2*y - xTile*1.5f + yTile*1;
                float zo = 2 * x + 2*y - yTile*.75f-xTile*.75f;
                Instantiate(floor, transform.position + new Vector3(xo, 0, zo), Quaternion.identity);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
