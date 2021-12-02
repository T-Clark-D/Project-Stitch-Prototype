using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightTrigger : MonoBehaviour
{
    lightColor lightCol;
    void Start()
    {
        lightCol = FindObjectOfType<lightColor>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            lightCol.changeColors = true;
        }
    }
}
