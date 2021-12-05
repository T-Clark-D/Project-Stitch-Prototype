using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane_Test : MonoBehaviour
{
    [SerializeField] private CharacterController m_CraneBody;
    [SerializeField] private PolygonCollider2D m_PolyCollider;
    [SerializeField] private BoxCollider2D m_LandCollider;
    [SerializeField] private Transform m_CraneCenter;

    private GameObject m_Player = null;

    [SerializeField] private float m_moveSpeed = 2.5f;

    private bool m_startMoving = false;
    private bool m_doneMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_startMoving)
        {
            m_CraneBody.transform.position += Vector3.right * m_moveSpeed * Time.deltaTime;
            if (m_Player != null) m_Player.transform.position = Vector3.MoveTowards(m_Player.transform.position, m_CraneCenter.position, 10 * Time.deltaTime);
        }
        if(GetComponentInParent<Transform>().position.x >= 200)
        {
            m_doneMoving = true;
            m_startMoving = false;
        }

        if (m_doneMoving)
        {
            if(m_Player != null) m_Player.transform.SetParent(null);
            
            m_Player.transform.position = new Vector3(m_Player.transform.position.x, m_Player.transform.position.y, 0);

            Physics2D.IgnoreCollision(m_LandCollider, m_Player.GetComponent<Collider2D>(), false);
            Physics2D.IgnoreCollision(m_PolyCollider, m_Player.GetComponent<Collider2D>(), false);
            m_Player.GetComponent<Rigidbody2D>().gravityScale = 1;
            m_Player = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(!m_doneMoving)
            {
                m_startMoving = true;

                m_Player = collision.gameObject;
                m_Player.transform.SetParent(this.transform);
                m_Player.GetComponent<Rigidbody2D>().gravityScale = 0;
                //m_Player.transform.localPosition = new Vector3(-15, collision.gameObject.transform.localPosition.y, 0);

                m_PolyCollider.enabled = true;
                Physics2D.IgnoreCollision(m_LandCollider, collision.collider);
                Physics2D.IgnoreCollision(m_PolyCollider, collision.collider);
            }
                
        }
    }
}
