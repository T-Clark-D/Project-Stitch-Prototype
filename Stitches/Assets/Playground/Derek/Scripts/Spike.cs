using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    GameHandler GH;
    [SerializeField] int mSpikeRecoil;  // 150 a pretty good knockback

    // Start is called before the first frame update
    void Start()
    {
        GH = GameObject.Find("GameHud").GetComponent<GameHandler>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            Vector2 direction = (transform.position - col.transform.position).normalized;
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(-direction * mSpikeRecoil);
            GH.takeDamage();
        }
    }
}
