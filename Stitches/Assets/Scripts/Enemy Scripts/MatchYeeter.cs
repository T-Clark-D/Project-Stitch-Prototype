using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchYeeter : MonoBehaviour
{
    // Start is called before the first frame update
    public float interval;
    public GameObject match;
    private Rigidbody2D RB;
    private CircleCollider2D m_collider;
    private float lastYeet;
    private int throwForce = 20;
    private SpriteRenderer SR;
    public GameObject player;
    bool lookingLeft;
    private int mod;
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        interval = 1.0f;
        lookingLeft = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (lookingLeft)
        {
            mod = -1;
        }
        else
        {
            mod = 1;
        }

        if (Time.timeSinceLevelLoad - lastYeet >= interval)
        {
            GameObject m_match = Instantiate(match,this.transform.position ,Quaternion.identity);
            m_match.GetComponent<Rigidbody2D>().AddForce(new Vector2(mod*1,1)* throwForce, ForceMode2D.Impulse);
            m_match.GetComponent<Rigidbody2D>().AddForceAtPosition(Vector2.right, new Vector2(-2.77f, -3.51f), ForceMode2D.Impulse);
            lastYeet = Time.timeSinceLevelLoad;
        }
        flipDirection();
       
    }
    private void flipDirection()
    {
        if (player.transform.position.x - this.transform.position.x > 0 && lookingLeft)
        {
              SR.flipX = true;
            lookingLeft = false;
            Transform[] fires = GetComponentsInChildren<Transform>();
            foreach (Transform f in fires)
            {
                if(!f.gameObject.Equals(this.gameObject))
                f.localPosition = new Vector2(-f.localPosition.x, f.localPosition.y);
            }
        }
        else if (player.transform.position.x - this.transform.position.x < 0 && !lookingLeft)
        {
             SR.flipX = false;
            lookingLeft = true;
            Transform[] fires = GetComponentsInChildren<Transform>();
            foreach (Transform f in fires)
            {
                if (!f.gameObject.Equals(this.gameObject))
                    f.localPosition = new Vector2(-f.localPosition.x, f.localPosition.y);
            }
        }
    }
}
