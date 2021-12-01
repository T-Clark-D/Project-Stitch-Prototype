using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armordillo : MonoBehaviour
{
    private Rigidbody2D m_RB;
    [SerializeField] private GameObject m_topPlatform;
    [SerializeField] private GameObject m_leftPlatform;
    [SerializeField] private GameObject m_rightPlatform;

    [SerializeField] private GameObject m_bottomBouncePad;
    [SerializeField] private GameObject m_topLeftBouncePad;
    [SerializeField] private GameObject m_topRightBouncePad;

    private Vector3 m_bottomPadEmerged = new Vector3(0, -44, 310);
    private Vector3 m_topLeftPadEmerged = new Vector3(38, 22, 310);
    private Vector3 m_topRightPadEmerged = new Vector3(-38, 22, 310);

    private Vector3 m_bottomPadHidden = new Vector3(0, -47, 310);
    private Vector3 m_topLeftPadHidden = new Vector3(41, 23, 310);
    private Vector3 m_topRightPadHidden = new Vector3(-41, 23, 310);

    private GameObject m_moveTo = null;

    [SerializeField] private float m_inverseBounceSpeed = 0.2f;
    [SerializeField] private float m_rollSpeed = 75f;

    [SerializeField] private bool m_moveToTopPlatform = false;
    [SerializeField] private bool m_moveToRightPlatform = false;
    [SerializeField] private bool m_moveToLeftPlatform = false;

    [SerializeField] private bool m_moveToBottomPad = false;
    [SerializeField] private bool m_moveToTopLeftPad = false;
    [SerializeField] private bool m_moveToTopRightPad = false;

    [SerializeField] private bool m_padsEmerging = false;
    [SerializeField] private bool m_padsHidden = false;

    [SerializeField] private bool m_bottomOrigin = false;
    [SerializeField] private bool m_topLeftOrigin = false;
    [SerializeField] private bool m_topRightOrigin = false;
    [SerializeField] private bool m_hitFromLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        m_RB = GetComponent<Rigidbody2D>();
        m_RB.velocity = new Vector2(1,-1) * 10;
        HideEmergePads();
    }

    private void HideEmergePads()
    {
        if (m_padsHidden)
        {
            m_bottomBouncePad.transform.localPosition = m_bottomPadEmerged;
            m_topLeftBouncePad.transform.localPosition = m_topLeftPadEmerged;
            m_topRightBouncePad.transform.localPosition = m_topRightPadEmerged;
        }
        else if (!m_padsHidden)
        {
            m_bottomBouncePad.transform.localPosition = m_bottomPadHidden;
            m_topLeftBouncePad.transform.localPosition = m_topLeftPadHidden;
            m_topRightBouncePad.transform.localPosition = m_topRightPadHidden;
        }
        m_padsHidden = !m_padsHidden;
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardPoint();
        if(!m_padsEmerging) StartCoroutine(PadsEmerge());

    }

    IEnumerator PadsEmerge()
    {
        m_padsEmerging = true;
        yield return new WaitForSeconds(Random.Range(4,7));
        HideEmergePads();
        yield return new WaitForSeconds(Random.Range(6, 12));
        HideEmergePads();
        m_padsEmerging = false;
    }

    private void MoveTowardPoint()
    {
        if (m_moveToTopPlatform)
        {
            m_moveTo = m_topPlatform;
        }
        else if (m_moveToLeftPlatform)
        {
            m_moveTo = m_leftPlatform;
        }
        else if (m_moveToRightPlatform)
        {
            m_moveTo = m_rightPlatform;
        }
        else if (m_moveToBottomPad)
        {
            m_moveTo = m_bottomBouncePad;
        }
        else if (m_moveToTopLeftPad)
        {
            m_moveTo = m_topLeftBouncePad;
        }
        else if (m_moveToTopRightPad)
        {
            m_moveTo = m_topRightBouncePad;
        }
        else
        {
            m_moveTo = null;
        }

        if (m_moveTo != null) transform.position = Vector3.MoveTowards(transform.position, m_moveTo.transform.position, m_inverseBounceSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollisions(collision);
    }

    private void HandleCollisions(Collision2D collision)
    {
        m_moveToTopPlatform = false;
        m_moveToLeftPlatform = false;
        m_moveToRightPlatform = false;
        m_moveToBottomPad = false;
        m_moveToTopLeftPad = false;
        m_moveToTopRightPad = false;

        switch (collision.gameObject.name)
        {
            case "Bounce_Pad_Bottom":
                if (transform.localPosition.x > 0 || m_topRightOrigin)
                {
                    m_moveToRightPlatform = true;
                    m_bottomOrigin = true;
                    m_topLeftOrigin = false;
                    m_topRightOrigin = false;
                    m_hitFromLeft = true;
                }
                else if (transform.localPosition.x < 0 || m_topLeftOrigin)
                {
                    m_moveToLeftPlatform = true;
                    m_bottomOrigin = true;
                    m_topLeftOrigin = false;
                    m_topRightOrigin = false;
                    m_hitFromLeft = false;
                }
                m_RB.velocity = Vector3.zero;
                break;
            case "Bounce_Pad_TopRight":
                if (m_bottomOrigin || m_topLeftOrigin || transform.localPosition.x < -38)
                {
                    m_moveToTopPlatform = true;
                    m_bottomOrigin = false;
                    m_topLeftOrigin = false;
                    m_topRightOrigin = true;
                    m_hitFromLeft = false;
                }
                else if (transform.localPosition.x > -38)
                {
                    m_moveToRightPlatform = true;
                    m_bottomOrigin = false;
                    m_topLeftOrigin = false;
                    m_topRightOrigin = true;
                    m_hitFromLeft = true;
                }
                
                m_RB.velocity = Vector3.zero;
                break;
            case "Bounce_Pad_TopLeft":
                if (m_bottomOrigin || m_topRightOrigin || transform.localPosition.x > 38)
                {
                    m_moveToTopPlatform = true;
                    m_bottomOrigin = false;
                    m_topLeftOrigin = true;
                    m_topRightOrigin = false;
                    m_hitFromLeft = true;
                }
                else if (transform.localPosition.x < 38)
                {
                    m_moveToLeftPlatform = true;
                    m_bottomOrigin = false;
                    m_topLeftOrigin = true;
                    m_topRightOrigin = false;
                    m_hitFromLeft = false;
                }
                m_RB.velocity = Vector3.zero;
                break;
            case "Top_Platform":
                if (m_bottomOrigin && m_hitFromLeft)
                {
                    m_moveToLeftPlatform = true;
                }
                else if (m_bottomOrigin && !m_hitFromLeft)
                {
                    m_moveToRightPlatform = true;
                }
                else if (m_topLeftOrigin && m_hitFromLeft)
                {
                    m_moveToLeftPlatform = true;
                }
                else if (m_topLeftOrigin && !m_hitFromLeft)
                {
                    m_moveToTopRightPad = true;
                }
                else if (m_topRightOrigin && m_hitFromLeft)
                {
                    m_moveToTopLeftPad = true;
                }
                else if (m_topRightOrigin && !m_hitFromLeft)
                {
                    m_moveToRightPlatform = true;
                }
                break;
            case "Left_Platform":
                if (m_bottomOrigin && m_hitFromLeft)
                {
                    m_moveToTopLeftPad = true;
                }
                else if (m_bottomOrigin && !m_hitFromLeft)
                {
                    m_moveToTopPlatform = true;
                }
                else if (m_topLeftOrigin && m_hitFromLeft)
                {
                    m_moveToRightPlatform = true;
                }
                else if (m_topLeftOrigin && !m_hitFromLeft)
                {
                    m_moveToRightPlatform = true;
                }
                else if (m_topRightOrigin && m_hitFromLeft)
                {
                    m_moveToTopPlatform = true;
                }
                else if (m_topRightOrigin && !m_hitFromLeft)
                {
                    m_moveToBottomPad = true;
                }
                break;
            case "Right_Platform":
                if (m_bottomOrigin && m_hitFromLeft)
                {
                    m_moveToTopPlatform = true;
                }
                else if (m_bottomOrigin && !m_hitFromLeft)
                {
                    m_moveToTopRightPad = true;
                }
                else if (m_topLeftOrigin && m_hitFromLeft)
                {
                    m_moveToTopRightPad = true;
                }
                else if (m_topLeftOrigin && !m_hitFromLeft)
                {
                    m_moveToTopPlatform = true;
                }
                else if (m_topRightOrigin && m_hitFromLeft)
                {
                    m_moveToLeftPlatform = true;
                }
                else if (m_topRightOrigin && !m_hitFromLeft)
                {
                    m_moveToLeftPlatform = true;
                }
                break;
            case "Player":
            case "GrapplingHook":
                //m_RB.gravityScale = 1;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.name)
        {
            case "Top_Boost":
                if (m_RB.velocity.x > 0)
                {
                    m_RB.velocity = Vector2.right * m_rollSpeed;
                }
                else 
                {
                    m_RB.velocity = Vector2.left * m_rollSpeed;
                }
                break;
            case "BottomLeft_Boost":
                if (m_RB.velocity.x > 0)
                {
                    m_RB.velocity = new Vector2(1, -2).normalized * m_rollSpeed;
                }
                else
                {
                    m_RB.velocity = new Vector2(-1, 1).normalized * m_rollSpeed;
                }
                break;
            case "BottomRight_Boost":
                if (m_RB.velocity.x > 0)
                {
                    m_RB.velocity = new Vector2(1, 1).normalized * m_rollSpeed;
                }
                else
                {
                    m_RB.velocity = new Vector2(-1, -2).normalized * m_rollSpeed;
                }
                break;

        }
    }
}
