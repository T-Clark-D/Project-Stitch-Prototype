using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerLevelChange : MonoBehaviour
{
    LevelChanger change;
    private void Start()
    {
        change = FindObjectOfType<LevelChanger>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            change.changeLevel = true;
        }
    }
}
