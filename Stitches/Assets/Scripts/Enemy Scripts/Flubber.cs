using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flubber : Enemy
{
    public Vector2 worldPoint = new Vector2(27, 13);
    public float agroRadius;
    public float interval;
    public float force = 15;

    /// <summary>
    /// Time the enemy will stay frozen after the player unhooking from it.
    /// </summary>
    
    /// <summary>
    /// Time the enemy will stay vulnerable after the player unhooking from it.
    /// </summary>
    

    private float lastForce = 0;
    

    
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        interval = 1.0f;
        if (m_frozen)
            Freeze();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (!m_aiIsOff)
        {
            //FlubberOne();
            //FlubberTwo();
            //FlubberThree();

            //flys around world point until player comes in proximity
            FlubberFour();
        }
        FlipDirection();
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
                m_RB.AddForce((targetPoint - (Vector2)this.transform.position).normalized * force, ForceMode2D.Impulse);
                lastForce = Time.timeSinceLevelLoad;
            }
        }
    }
    private void FlipDirection()
    {
        if (m_RB.velocity.x > 0)
        {
            m_SR.flipX = true;
        }
        else
        {
            m_SR.flipX = false;
        }
    }

   

    //extra AIs 

    /*
 

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
