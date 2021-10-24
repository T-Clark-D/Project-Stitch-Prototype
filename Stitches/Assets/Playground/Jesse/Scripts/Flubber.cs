using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flubber : MonoBehaviour
{
    public Vector2 worldPoint = new Vector2(27, 13);
    public float agroRadius;
    public float interval;
    public float force = 15;


    private float lastForce = 0;
    private SpriteRenderer SR;
    private Rigidbody2D RB;
    
    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        interval = 1.0f;
        SR = GetComponent<SpriteRenderer>();
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

    private void FlubberFour()
    {
        LayerMask mask = LayerMask.GetMask("Player");
        Collider2D player = Physics2D.OverlapCircle((Vector2)transform.position, agroRadius, mask);
        Vector2 targetPoint = worldPoint;
        if (Physics2D.OverlapCircle((Vector2)transform.position, agroRadius, mask) != null) 
        {
            if ((player.transform.position - this.transform.position).magnitude < 20)
            {
                targetPoint = player.transform.position;
            }
        }
        if (Time.timeSinceLevelLoad - lastForce >= interval)
        {
            RB.AddForce((targetPoint  - (Vector2)this.transform.position).normalized * force, ForceMode2D.Impulse);
            lastForce = Time.timeSinceLevelLoad;
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
