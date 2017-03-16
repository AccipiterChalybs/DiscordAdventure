using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour {
    public bool player;

    public Transform remains;

    public void Destroy()
    {
        if (player)
        {
            StartCoroutine("restartGame");
        } else
        {
            //TODO: this should all really be in its own subclass of destroyable for enemies
            MoveToPlayer mtp = GetComponent<MoveToPlayer>();
            if (mtp)
            {
                DestroyObject(mtp.agent.gameObject);

                if (remains) Instantiate(remains, this.transform.position, this.transform.rotation);
            }
            DestroyObject(this.gameObject);
        }

    }

    IEnumerator restartGame()
    {
        this.GetComponentInChildren<GameOverCamera>().enabled = true;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(5);
        Time.timeScale = 1;
        Application.LoadLevel(0);
    }
}
