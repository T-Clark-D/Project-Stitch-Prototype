using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public float m_pullUpSpeed = 15f;
    public float m_initialPullUpSpeed = 15f;
    public float m_baseGravityScale = 1f;
    public float m_baseMass = 1f;
    public float m_minimumRopeLength = 0.5f;
    public float m_pullingUpPlayerMass = 0.25f;
    public float m_boostSpeed = 2f;
    public float m_gravityOffTimeForBoost = 1.5f;

    //Used to fix bug where grappling hook fires again when releasing mouse button after hitting ungrapplable object
    public bool m_ungrapplableBuffer = false;

    private bool m_firstTimePullingUp = true;
    // Set to true when we boost.
    private bool m_justBoosted = false;
    private float m_gravityOffTimeElapsed = 0f;
    private bool m_gravityIsOffAndOnTimer = false;

    [SerializeField] private GameObject m_grapplingHookObject;
    private HookController m_grapplingHookController;
    private Rigidbody2D m_rigidBody;
    private Vector3 m_hookDirection;
    private DistanceJoint2D m_distJoint;
    private SpriteRenderer[] m_SR;

    // Start is called before the first frame update
    void Start()
    {
        m_SR = GetComponentsInChildren<SpriteRenderer>();
        m_grapplingHookController = m_grapplingHookObject.GetComponent<HookController>();
        m_distJoint = GetComponent<DistanceJoint2D>();
        m_rigidBody = GetComponent<Rigidbody2D>();

        m_rigidBody.mass = m_baseMass;
        m_rigidBody.gravityScale = m_baseGravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(Math.Abs(m_rigidBody.velocity.x) > 0.1)
        {
            foreach (SpriteRenderer sr in m_SR)
            {
                if (m_rigidBody.velocity.x > 0)
                {
                    sr.flipX = false;
                }
                else
                {
                    sr.flipX = true;
                }
            }
        }
     
        HandleMovement();

        if(m_gravityIsOffAndOnTimer)
            m_gravityOffTimeElapsed += Time.deltaTime;

        if(m_gravityOffTimeElapsed >= m_gravityOffTimeForBoost)
        {
            m_rigidBody.gravityScale = m_baseGravityScale;
        }
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
            if(!m_justBoosted && !m_ungrapplableBuffer)
            {
                m_grapplingHookController.LaunchHook(Input.mousePosition);

                m_firstTimePullingUp = true;
            }
            else
            {
                m_justBoosted = false;
                m_ungrapplableBuffer = false;
            }
        }

        if(Input.GetMouseButton(1))
        {
            if(m_grapplingHookController.m_isHookedToAnEnemy)
            {
                // Starting a timer for the zero gravity.
                m_gravityOffTimeElapsed = 0f;
                m_rigidBody.gravityScale = 0f;
                m_gravityIsOffAndOnTimer = true;

                m_grapplingHookController.StartVulnerabilityTimer();

                Boost();
            }
            else if(m_grapplingHookController.m_tethered && m_distJoint.distance >= m_minimumRopeLength)
            {
                if(m_firstTimePullingUp)
                {
                    m_grapplingHookController.m_pullingUp = true;

                    // Disengaging gravity on player
                    m_rigidBody.gravityScale = 0f;

                    //m_rigidBody.velocity = new Vector2(0, 0);
                    //m_rigidBody.angularVelocity = 0f;
                    m_rigidBody.mass = m_pullingUpPlayerMass;

                    // Giving an initial boost to the player
                    Vector3 direction = m_grapplingHookController.GetHookDirection(false);
                    m_hookDirection = direction.normalized;
                    m_rigidBody.AddForce(m_hookDirection * m_initialPullUpSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
                    m_distJoint.distance = direction.magnitude;

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
        //m_rigidBody.AddForce(m_rigidBody.velocity.normalized * m_boostSpeed, ForceMode2D.Impulse);
        m_rigidBody.AddForce(m_grapplingHookController.GetHookDirection() * m_boostSpeed, ForceMode2D.Impulse);

        // Unhook
        m_grapplingHookController.RetractHook();

        m_firstTimePullingUp = true;
        m_justBoosted = true;
    }

    public void RetractHook()
    {
        m_grapplingHookController.RetractHook();
        m_firstTimePullingUp = true;
    }
}
