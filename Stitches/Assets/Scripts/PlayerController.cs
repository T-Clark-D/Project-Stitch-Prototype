using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float m_pullUpSpeed = 15f;
    public float m_baseGravityScale = 1f;

    [SerializeField] private GameObject m_grapplingHookObject;
    private HookController m_grapplingHookController;
    private Rigidbody2D m_rigidBody;
    private Vector3 m_hookDirection;

    // Start is called before the first frame update
    void Start()
    {
        m_grapplingHookController = m_grapplingHookObject.GetComponent<HookController>();
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void FixedUpdate()
    {
        if (m_grapplingHookController.m_pullingUp)
        {
            m_hookDirection = m_grapplingHookController.GetHookDirection();

            m_rigidBody.MovePosition(transform.position + (m_hookDirection * m_pullUpSpeed * Time.fixedDeltaTime));
        }
    }

    void HandleMovement()
    {
        if(Input.GetMouseButtonDown(0))
        {
            m_grapplingHookController.LaunchHook(Input.mousePosition);
        }
        if(Input.GetMouseButtonDown(1))
        {
            if(m_grapplingHookController.m_tethered && !m_grapplingHookController.m_pullingUp)
            {
                m_grapplingHookController.PullUp();

                // Disengaging gravity on player
                m_rigidBody.gravityScale = 0f;

                m_rigidBody.velocity = new Vector2(0, 0);
                m_rigidBody.angularVelocity = 0f;
            }
        }     
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(m_grapplingHookController.m_pullingUp)
        {
            if (collision.collider.CompareTag("Platform"))
            {
                m_rigidBody.gravityScale = m_baseGravityScale;

                m_grapplingHookController.m_pullingUp = false;
                m_grapplingHookController.RetractHook();
            }
        }
    }

    public void ResetGravity()
    {
        m_rigidBody.gravityScale = m_baseGravityScale;
    }
}
