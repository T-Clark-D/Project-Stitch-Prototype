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
        
       if(m_RB.velocity.magnitude < 10)
        {
           m_RB.AddForce(m_RB.velocity.normalized * 1000);
        }
       // m_RB.AddForce(m_RB.velocity.normalized * 1000);
        transform.up = collision.GetContact(0).normal;
        m_anim.SetTrigger("Bounce");
    }
}
