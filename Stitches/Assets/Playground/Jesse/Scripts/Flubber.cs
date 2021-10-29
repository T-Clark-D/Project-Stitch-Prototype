using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flubber : MonoBehaviour
{
    public Vector2 worldPoint = new Vector2(27, 13);
    public float agroRadius;
    public float interval;
    public float force = 15;
    public bool m_isHooked = false;


    private float lastForce = 0;
    private SpriteRenderer SR;
    private Rigidbody2D RB;
    private CircleCollider2D m_collider;
    [SerializeField] private ParticleSystem m_explosionEffectSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        interval = 1.0f;
        SR = GetComponent<SpriteRenderer>();
        m_collider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //FlubberOne();
        //FlubberTwo();
        //FlubberThree();

        //flys around world point until player comes in proximity
        FlubberFour();
        flipDirection();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we are hooked
        if(m_isHooked)
        {
            // If it's the player that enters our body
            if(collision.gameObject.CompareTag("Player"))
            {
                // We explode, giving the player a speedboost.
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.Boost();

                // Trigger the particle effect
                m_explosionEffectSystem.gameObject.transform.position = this.gameObject.transform.position;
                m_explosionEffectSystem.Play();

                Destroy(this.gameObject);
            }
        }
    }

    public void ToggleHook()
    {
        if(m_isHooked)
        {
            // Unhooking
            m_isHooked = false;

            // Leaving only rotation frozen.
            RB.constraints = RigidbodyConstraints2D.FreezeRotation;

            // Reactivating collider
            m_collider.isTrigger = false;
        }
        else
        {
            m_isHooked = true;

            // Object becomes immovable.
            RB.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

            // Object cannot stop movement.
            m_collider.isTrigger = true;
        }
    }

    private void FlubberFour()
    {
        LayerMask mask = LayerMask.GetMask("Player");
        Collider2D player = Physics2D.OverlapCircle((Vector2)transform.position, agroRadius, mask);
        Vector2 targetPoint = worldPoint;
        if (player != null) 
        {
            if ((player.transform.position - this.transform.position).magnitude < agroRadius)
            {
                targetPoint = player.transform.position;
            }
        }
        // If we are hooked by the player, we cant move.
        if(!m_isHooked)
        {
            if (Time.timeSinceLevelLoad - lastForce >= interval)
            {
                RB.AddForce((targetPoint - (Vector2)this.transform.position).normalized * force, ForceMode2D.Impulse);
                lastForce = Time.timeSinceLevelLoad;
            }
        }
    }
    private void flipDirection()
    {
        if (RB.velocity.x > 0)
        {
            SR.flipX = true;
        }
        else
        {
            SR.flipX = false;
        }
    }

    //extra AIs 

    /*
    private void FlubberThree()
    {
        transform.Rotate(Vector3.up , Space.Self);
    }

   
    private void FlubberTwo()
    {
        if (Time.timeSinceLevelLoad - lastForce >= interval)
        {
            RB.AddForce((player.transform.position - this.transform.position).normalized * force, ForceMode2D.Impulse);
            lastForce = Time.timeSinceLevelLoad;
        }
    }

    private void FlubberOne()
    {
        if (Time.timeSinceLevelLoad - lastForce >= interval)
        {
            switch (Random.Range(0, 2))
            {
                case 0:
                    RB.AddForce(Vector3.up * force, ForceMode2D.Impulse);
                    break;
                case 1:
                    RB.AddForce(new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)).normalized * force, ForceMode2D.Impulse);
                    break;
            }
            RB.AddForce(Vector3.up * force, ForceMode2D.Impulse);
            lastForce = Time.timeSinceLevelLoad;
        }
    }*/
}
