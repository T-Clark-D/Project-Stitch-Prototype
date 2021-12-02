using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool m_isHooked = false;
    public bool m_isVulnerable = false;
    public bool m_aiIsOff = false;
    public bool m_frozen = false;
    protected float m_freezeElapsedTime = 0f;
    protected bool m_thawing = false;
    protected float m_vulnerabilityElapsedTime = 0f;
    protected bool m_vulnerabilityIsOnTimer = false;
    protected ParticleSystem m_explosionEffectSystem;
    public float m_freezeTime = 1f;
    public float m_vulnerabilityTime = 2f;

    protected SpriteRenderer m_SR;
    protected Rigidbody2D m_RB;
    protected Animator m_anim;
    protected Collider2D m_collider;

    protected virtual void Start()
    {
        m_explosionEffectSystem =  GameObject.Find("ExplosionEffect").GetComponent<ParticleSystem>();
        m_SR = GetComponent<SpriteRenderer>();
        m_RB = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
        m_collider = GetComponent<Collider2D>();
    }
    protected virtual void Update()
    {
        if (m_frozen && m_thawing)
            m_freezeElapsedTime += Time.deltaTime;

        if (m_thawing && m_freezeElapsedTime >= m_freezeTime)
        {
            Unfreeze();
        }

        if (m_isVulnerable && m_vulnerabilityIsOnTimer)
            m_vulnerabilityElapsedTime += Time.deltaTime;

        if (m_vulnerabilityIsOnTimer && m_vulnerabilityElapsedTime >= m_vulnerabilityTime)
        {
            ToggleVulnerability();

            m_vulnerabilityIsOnTimer = false;
        }
    }
    public virtual void ToggleHook()
    {
        if (m_isHooked)
        {
            // Unhooking
            m_isHooked = false;

            // Checking if we need to unfreeze / Make ourselves strong again on a timer, or not.
            if (m_vulnerabilityIsOnTimer)
            {
                // Player unhooked and boosted. We are vulnerable for some time.

                // Starting the timer.
                m_freezeElapsedTime = 0f;

                m_thawing = true;
            }
            else
            {
                // Player unhooked without boosting. We get strong right away.
                Unfreeze();
                ToggleVulnerability();
            }
        }
        else
        {
            m_isHooked = true;
            Freeze();

            ToggleVulnerability();
        }

        
    }
        public void Freeze()
        {
            m_frozen = true;

            // Object becomes immovable.
            m_RB.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            
        }

        public void Unfreeze()
        {
            m_frozen = false;

            // Leaving only rotation frozen.
            m_RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

    public virtual void ToggleVulnerability()
    {
        m_isVulnerable = !m_isVulnerable;

        if (m_isVulnerable)
        {
            // Object cannot stop movement.
            m_collider.isTrigger = true;
        }
        else
        {
            m_collider.isTrigger = false;
        }
    }

    public virtual void StartVulnerabilityTimer()
    {
        m_vulnerabilityElapsedTime = 0f;
        m_vulnerabilityIsOnTimer = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If it's the player that enters our body
        if (collision.gameObject.CompareTag("Player"))
        {
            if (m_isVulnerable)
            {
                // We explode, giving the player a speedboost.
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();

                player.RetractHook();
                // Uncomment to make the player boost when coming into contact with a hooked enemy.
                //player.Boost();

                // Trigger the particle effect
                m_explosionEffectSystem.gameObject.transform.position = this.gameObject.transform.position;
                m_explosionEffectSystem.Play();

                Destroy(this.gameObject);
            }
        }
    }
}
