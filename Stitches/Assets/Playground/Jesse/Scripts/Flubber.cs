using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flubber : MonoBehaviour
{
    public float interval;
    private Rigidbody2D RB;
    private float lastForce = 0;
    public float force = 15;
    public GameObject player;
    public SpriteRenderer SR;
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
        FlubberTwo();
        FlubberThree();
        flipDirection();
    }

    private void FlubberThree()
    {
        transform.Rotate(Vector3.up , Space.Self);
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
    }
}
