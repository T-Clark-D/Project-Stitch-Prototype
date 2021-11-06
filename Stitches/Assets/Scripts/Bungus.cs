using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bungus : MonoBehaviour
{
    private SpriteRenderer SR;
    private Rigidbody2D RB;
    private CircleCollider2D m_collider;
    private Animator m_anim;
    // Start is called before the first frame update
    void Start()
    {
        m_anim = GetComponent<Animator>();
        SR = GetComponent<SpriteRenderer>();
        RB = GetComponent<Rigidbody2D>();
        RB.AddForce(Vector3.right * 50);
    }

    // Update is called once per frame
    void Update()
    {

        print("yooo");
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        RB.AddForce(RB.velocity.normalized*1000);

        
        print(collision.GetContact(0).normal);
        //RB.transform.Rotate(0, 0, Vector2.Angle(transform.up, collision.GetContact(0).normal));
        //RB.rotation = RB.rotation + Vector2.Angle(transform.up, collision.GetContact(0).normal);
       // transform.LookAt(collision.transform);
        transform.up = collision.GetContact(0).normal;
        m_anim.SetTrigger("Bounce");
    }
}
