using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyer : Enemy
{
    public AudioClip[] m_flyingSounds;
    public AudioSource m_flyingAudioSource;
    public AudioClip[] m_attackSounds;
    public AudioSource m_attackAudioSource;

    private Vector2 m_startPosition;
    private float circleFlyAngle = 360;
    private bool circling = true;
    private bool attacking = false;

    private ParticleSystem m_flyerExplosion;
    //private Collider2D m_collider;

    public float flyRadius;
    public float flySpeed;
    public float agroRadius;

    public Collider2D m_parentCollider;
    new public Collider2D m_collider;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        //m_collider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(m_collider, m_parentCollider, true);
        m_collider.enabled = true;


        m_flyerExplosion = GameObject.Find("Flyer_Explosion").GetComponent<ParticleSystem>();
        m_startPosition = transform.localPosition;
        print(m_startPosition);

        // Play audio clip
        int randomIndex = UnityEngine.Random.Range(0, m_flyingSounds.Length);
        m_flyingAudioSource.clip = m_flyingSounds[randomIndex];
        m_flyingAudioSource.loop = true;
        m_flyingAudioSource.Play();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //HandleFlying();

        DiveBomb();

        if (!attacking) transform.localPosition = m_startPosition;
    }

    private void DiveBomb()
    {
        LayerMask mask = LayerMask.GetMask("Player");
        Collider2D player = Physics2D.OverlapCircle((Vector2)transform.position, agroRadius, mask);
        Vector2 targetPoint = m_startPosition;
        if (player != null)
        {
            //if ((player.transform.position - this.transform.position).magnitude < agroRadius)
            {
                targetPoint = player.transform.position;
            }
        }
        if (!m_isHooked && targetPoint != m_startPosition && !attacking)
        {
            m_anim.SetTrigger("dive");
            attacking = true;
            circling = false;
            Vector2 attackDirection = (targetPoint - (Vector2)this.transform.position).normalized;
            m_RB.freezeRotation = false;
            float flyAngle = Vector2.Angle(Vector2.up, attackDirection);
            if (attackDirection.x > 0) flyAngle = -flyAngle;
            m_RB.SetRotation(flyAngle - 135);
            m_RB.velocity = attackDirection * flySpeed * 5;

            // Play audio clip
            int randomIndex = UnityEngine.Random.Range(0, m_attackSounds.Length);
            m_attackAudioSource.PlayOneShot(m_attackSounds[randomIndex]);
        }
    }

    private void HandleFlying()
    {
        if (circling)
        {
            transform.position = m_startPosition + new Vector2(Mathf.Sin(circleFlyAngle), Mathf.Cos(circleFlyAngle)) * flyRadius;
            circleFlyAngle -= Time.deltaTime * flySpeed;
            if (circleFlyAngle <= 0) circleFlyAngle = 360;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        m_flyerExplosion.gameObject.transform.position = this.gameObject.transform.position;
        m_flyerExplosion.Play();
        Destroy(this.gameObject);
    }
}
