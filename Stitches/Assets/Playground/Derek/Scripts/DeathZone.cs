using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    GameHandler GH;

    // Start is called before the first frame update
    void Start()
    {
        GH = GameObject.Find("GameHud").GetComponent<GameHandler>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            GH.kill();
        }
    }
}
