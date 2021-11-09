using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchYeeter : Enemy
{
    // Start is called before the first frame update
    public float interval;
    public GameObject match;
    private float lastYeet;
    private int throwForce = 20;
    public GameObject player;
    bool lookingLeft;
    private int mod;

    protected override void Start()
    {
        base.Start();
        interval = 1.0f;
        lookingLeft = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

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
        FlipDirection();
       
    }
    private void FlipDirection()
    {
        if (player.transform.position.x - this.transform.position.x > 0 && lookingLeft)
        {
              m_SR.flipX = true;
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
             m_SR.flipX = false;
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
