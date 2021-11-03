using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyController : MonoBehaviour
{
    public PlayerController m_player;

    private Rigidbody2D m_rigidBody;
    private SpriteRenderer m_spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Making the body's neck face towards the player's head.
        Vector3 moveDirection = m_player.transform.position - transform.position;

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
      
        if (m_rigidBody.velocity.x > 0)
        {
            m_spriteRenderer.flipX = false;
        }
        else
        {
            m_spriteRenderer.flipX = true;
        }
    }
}
