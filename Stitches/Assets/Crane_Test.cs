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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_CraneBody.transform.position += Vector3.right * Time.deltaTime;
        if(m_Player != null) m_Player.transform.position = Vector3.MoveTowards(m_Player.transform.position, m_CraneCenter.position, 3 * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            m_Player = collision.gameObject;
            m_Player.transform.SetParent(this.transform);
            //m_Player.transform.localPosition = new Vector3(-15, collision.gameObject.transform.localPosition.y, 0);
            
            m_PolyCollider.enabled = true;
            Physics2D.IgnoreCollision(m_LandCollider, collision.collider);
            Physics2D.IgnoreCollision(m_PolyCollider, collision.collider);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        m_Player = null;
        collision.gameObject.transform.SetParent(null);
        Physics2D.IgnoreCollision(m_LandCollider, collision.collider, false);
        Physics2D.IgnoreCollision(m_PolyCollider, collision.collider, false);
    }
}
