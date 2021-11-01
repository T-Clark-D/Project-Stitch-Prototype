using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float m_pullUpSpeed = 15f;
    public float m_baseGravityScale = 1f;
    public float m_baseMass = 1f;
    public float m_minimumRopeLength = 0.5f;
    public float m_pullingUpPlayerMass = 0.25f;
    public float m_boostSpeed = 2f;

    private bool m_firstTimePullingUp = true;
    // Set to true when we boost.
    private bool m_justBoosted = false;

    [SerializeField] private GameObject m_grapplingHookObject;
    private HookController m_grapplingHookController;
    private Rigidbody2D m_rigidBody;
    private Vector3 m_hookDirection;
    private DistanceJoint2D m_distJoint;

    // Start is called before the first frame update
    void Start()
    {
        m_grapplingHookController = m_grapplingHookObject.GetComponent<HookController>();
        m_distJoint = GetComponent<DistanceJoint2D>();
        m_rigidBody = GetComponent<Rigidbody2D>();

        m_rigidBody.mass = m_baseMass;
        m_rigidBody.gravityScale = m_baseGravityScale;
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
            Vector3 direction = m_grapplingHookController.GetHookDirection(false);
            m_hookDirection = direction.normalized;

            m_rigidBody.AddForce(m_hookDirection * m_pullUpSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
            //m_rigidBody.MovePosition(transform.position + (m_hookDirection * m_pullUpSpeed * Time.fixedDeltaTime));

            m_distJoint.distance = direction.magnitude;
        }
    }

    void HandleMovement()
    {
        if(Input.GetMouseButtonDown(0))
        {
            m_grapplingHookController.LaunchHook(Input.mousePosition);
        }

        // Letting go of the left click.
        if(Input.GetMouseButtonUp(0))
        {
            // If we just boosted, ignore the let go.
            if(!m_justBoosted)
            {
                m_grapplingHookController.LaunchHook(Input.mousePosition);

                m_firstTimePullingUp = true;
            }
            else
            {
                m_justBoosted = false;
            }
        }

        if(Input.GetMouseButton(1))
        {
            if(m_grapplingHookController.m_tethered && m_distJoint.distance >= m_minimumRopeLength)
            {
                if(m_firstTimePullingUp)
                {
                    m_grapplingHookController.m_pullingUp = true;

                    // Disengaging gravity on player
                    m_rigidBody.gravityScale = 0f;

                    //m_rigidBody.velocity = new Vector2(0, 0);
                    //m_rigidBody.angularVelocity = 0f;
                    m_rigidBody.mass = m_pullingUpPlayerMass;

                    m_firstTimePullingUp = false;
                }
            }
            else
            {
                // The player reached maximum pull height
                m_rigidBody.gravityScale = m_baseGravityScale;
                m_rigidBody.mass = m_baseMass;
                m_firstTimePullingUp = true;
            }
        }
        else
        {
            if (m_grapplingHookController.m_tethered)
            {
                m_grapplingHookController.m_pullingUp = false;

                // Engaging gravity on player
                m_rigidBody.gravityScale = m_baseGravityScale;
                m_rigidBody.mass = m_baseMass;

                m_firstTimePullingUp = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D pCollision)
    {
        if(m_grapplingHookController.m_pullingUp)
        {
            if (pCollision.collider.CompareTag("Platform"))
            {
                m_rigidBody.gravityScale = m_baseGravityScale;
            }
        }
    }

    public void ResetGravity()
    {
        m_rigidBody.gravityScale = m_baseGravityScale;
        m_rigidBody.mass = m_baseMass;
    }

    public void Boost()
    {
        m_rigidBody.AddForce(m_rigidBody.velocity.normalized * m_boostSpeed, ForceMode2D.Impulse);

        // Unhook
        m_grapplingHookController.RetractHook();

        m_firstTimePullingUp = true;
        m_justBoosted = true;
    }
}
