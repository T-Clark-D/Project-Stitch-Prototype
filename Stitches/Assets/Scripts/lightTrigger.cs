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
        // Can trigger color change when the player enters the trigger box.
        if(col.tag == "Player")
        {
            lightCol.changeColors = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        // Switch the end and start color in lightColor script when exit.
        if (col.tag == "Player")
        {
            lightCol.changeColors = false;
            lightCol.switcharoo = true;
        }
    }
}
