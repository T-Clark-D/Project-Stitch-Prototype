using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHP : MonoBehaviour
{
    GameHandler GH;
    bool hpUP = false;
    // Start is called before the first frame update
    void Start()
    {
        GH = GameObject.Find("GameHud").GetComponent<GameHandler>();
    }

    void Update()
    {
        if(hpUP)
        {
            GH.addHP();
            hpUP = false;
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            hpUP = true;
        }
    }
}
