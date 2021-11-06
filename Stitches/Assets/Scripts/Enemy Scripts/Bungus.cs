using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bungus : Enemy
{
    new void Start()
    {
        base.Start();
        print("bitch");
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        m_RB.AddForce(m_RB.velocity.normalized*1000);
        transform.up = collision.GetContact(0).normal;
        m_anim.SetTrigger("Bounce");
    }
}
