using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bungus : MonoBehaviour
{
    public AudioClip[] m_bounceSounds;
    public AudioSource m_bounceAudioSource;
    private Rigidbody2D m_RB;
    private Animator m_anim;

    new void Start()
    {
        m_RB = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
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

        // Play audio clip
        int randomIndex = UnityEngine.Random.Range(0, m_bounceSounds.Length);
        m_bounceAudioSource.PlayOneShot(m_bounceSounds[randomIndex]);
    }
}
