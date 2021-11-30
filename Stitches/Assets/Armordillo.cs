using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armordillo : MonoBehaviour
{
    private Rigidbody2D m_RB;
    [SerializeField] private GameObject m_topPlatform;
    [SerializeField] private GameObject m_leftPlatform;
    [SerializeField] private GameObject m_rightPlatform;
    [SerializeField] private GameObject m_topLeft;
    [SerializeField] private GameObject m_topRight;

    private GameObject m_moveTo = null;

    [SerializeField] private float m_inverseBounceSpeed = 0.2f;
    [SerializeField] private float m_rollSpeed = 75f;

    private bool m_moveToCenter = false;
    private bool m_moveToRight = false;
    private bool m_moveToLeft = false;
    private bool m_moveToTopLeft = false;
    private bool m_moveToTopRight = false;

    private bool m_movingLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        m_RB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardPoint();
    }

    private void MoveTowardPoint()
    {
        if (m_moveToCenter)
        {
            m_moveTo = m_topPlatform;
        }
        else if (m_moveToLeft)
        {
            m_moveTo = m_leftPlatform;
        }
        else if (m_moveToRight)
        {
            m_moveTo = m_rightPlatform;
        }
        else if (m_moveToTopLeft)
        {
            m_moveTo = m_topLeft;
        }
        else if (m_moveToTopRight)
        {
            m_moveTo = m_topRight;
        }
        else
        {
            m_moveTo = null;
        }
        if (m_moveTo != null) transform.position = Vector3.MoveTowards(transform.position, m_moveTo.transform.position, m_inverseBounceSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_moveToCenter = false;
        m_moveToLeft = false;
        m_moveToRight = false;
        m_moveToTopLeft = false;
        m_moveToTopRight = false;

        switch (collision.gameObject.name)
        {
            case "Bounce_Pad":
                m_RB.gravityScale = 0;
                if (m_RB.velocity.x > 0)
                {
                    m_movingLeft = false;
                    m_moveToLeft = true;
                }
                else
                {
                    m_moveToRight = true;
                    m_movingLeft = true;
                }
                m_RB.velocity = Vector3.zero;
                break;
            case "Top_Platform":
                if(m_movingLeft) m_moveToTopRight = true;
                else m_moveToTopLeft = true;
                break;
            case "Left_Platform":
            case "Right_Platform":
                m_moveToCenter = true;
                break;
            case "Top_Left":
                m_RB.velocity = new Vector2(-0.5f, -1)*m_rollSpeed;
                break;
            case "Top_Right":
                m_RB.velocity = new Vector2(0.5f, -1)*m_rollSpeed;
                break;
        }
    }
}
