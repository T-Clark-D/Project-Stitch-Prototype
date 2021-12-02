using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [SerializeField] GameObject fire;
    Vector3 worldPoint;
    bool lit;
    GameHandler GH;

    // Start is called before the first frame update
    void Start()
    {
        lit = false;
        worldPoint = transform.position;
        GH = GameObject.Find("GameHud").GetComponent<GameHandler>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(!lit && col.tag == "Player")
        {
            fire.SetActive(true);
            GH.setRespawnPoint(worldPoint);
            lit = true;
        }
    }

}
