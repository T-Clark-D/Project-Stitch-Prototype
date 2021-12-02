using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death : MonoBehaviour
{
    GameHandler GH;
    private void Start()
    {
        GH = GameObject.Find("GameHud").GetComponent<GameHandler>();
    }
    // Start is called before the first frame update
    private void OnCollisionTrigger(Collision collision)
    {
        GH.takeDamage();
        GH.takeDamage();
        GH.takeDamage();
        GH.takeDamage();
        GH.takeDamage();
        GH.takeDamage();
        GH.takeDamage();
        GH.takeDamage();
        GH.takeDamage();
    }
}
