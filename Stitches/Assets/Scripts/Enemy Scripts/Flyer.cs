using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyer : Enemy
{
    private Vector2 m_startPosition;
    private float circleFlyAngle = 360;
    private bool circling = true;
    private bool attacking = false;

    private ParticleSystem m_flyerExplosion;

    public float flyRadius;
    public float flySpeed;
    public float agroRadius;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_flyerExplosion = GameObject.Find("Flyer_Explosion").GetComponent<ParticleSystem>();
        m_startPosition = transform.position;
        print(m_startPosition);
    }

    // Update is called once per frame
    protected override void Update()
    {
        HandleFlying();
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
            attacking = true;
            circling = false;
            Vector2 attackDirection = (targetPoint - (Vector2)this.transform.position).normalized;
            m_RB.freezeRotation = false;
            float flyAngle = Vector2.Angle(Vector2.up, attackDirection);
            if (attackDirection.x > 0) flyAngle = -flyAngle;
            m_RB.SetRotation(flyAngle);
            m_RB.velocity = attackDirection*flySpeed*5;
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
