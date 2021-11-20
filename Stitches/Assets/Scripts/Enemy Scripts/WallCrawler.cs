using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCrawler : Enemy
{
    private enum Direction { Right, Down, Left, Up, None }

    /// <summary>
    /// Determines whether the crawler is on a vertical surface or a horizontal surface.
    /// </summary>
    public bool m_isVertical = false;
    public float m_speed = 1f;
    public float m_aggroRange = 10f;

    public float m_attackRange = 1f;
    public float m_wallCheckDistance = 0.02f;
    public float m_groundCheckDistance = 0.2f;
    public float m_pauseTimer = 2f;
    public float m_attackPauseTimer = 1f;
    /// <summary>
    /// Determines whether the crawler will be placed in the scene originally using gravity or not.
    /// If true, the crawler will have gravity enabled for its initial placement.
    /// </summary>
    public bool m_placeWithGravity = false;

    [SerializeField] private PlayerController m_player;
    [SerializeField] private TongueController m_tongue;
    private Vector3 m_enemySize;
    private Direction m_direction = Direction.None;
    private Direction m_lastDirection = Direction.None;
    private bool m_hasBeenPlaced = false;
    private float m_pauseTimeElapsed = 0f;
    private bool m_isPaused = false;
    private bool m_hasTurned = false;

    private bool m_hasAttacked = false;
    private float m_attackPauseTimeElapsed = 0f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        m_tongue = this.GetComponentInChildren<TongueController>();

        m_enemySize = m_collider.bounds.size;

        if(m_placeWithGravity)
        {
            if (!m_hasBeenPlaced)
            {
                m_RB.gravityScale = 1f;
            }
        }
        else
        {
            m_hasBeenPlaced = true;
        }
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (m_isPaused)
            m_pauseTimeElapsed += Time.deltaTime;

        if(m_isPaused && m_pauseTimeElapsed >= m_pauseTimer)
        {
            m_isPaused = false;
        }

        if (m_hasAttacked)
            m_attackPauseTimeElapsed += Time.deltaTime;

        if(m_hasAttacked && m_attackPauseTimeElapsed >= m_attackPauseTimer)
        {
            m_hasAttacked = false;
        }

        if(!m_hasAttacked)
            HandleAttack();
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {    
        if(!m_hasBeenPlaced)
        {
            if (collision.collider.CompareTag("Platform"))
            {
                m_hasBeenPlaced = true;
                m_RB.gravityScale = 0f;

                m_pauseTimeElapsed = 0f;
                m_isPaused = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if(m_hasBeenPlaced)
        {
            m_hasTurned = false;

            HandleFall();

            if(!m_hasTurned)
                HandleCrawl();

            if(!m_tongue.m_tongueOut)
                Movement();

            if ( !m_isPaused &&
                (m_direction == Direction.Left && m_lastDirection == Direction.Right
                || m_direction == Direction.Right && m_lastDirection == Direction.Left ))
            {
                m_isPaused = true;
                m_pauseTimeElapsed = 0f;
            }
        }
    }

    private void HandleFall()
    {
        if (IsGrounded())
            return;

        if (m_isPaused)
            return;

        // We are dangling in the void. Time to turn!

        if (m_direction == Direction.Right)
        {
            // Going right.
            transform.Rotate(new Vector3(0, 0, -90));
            m_RB.MoveRotation(Quaternion.Euler(0, 0, -90));

            m_hasTurned = true;
        }
        else if (m_direction == Direction.Left)
        {
            // Going left.
            transform.Rotate(new Vector3(0, 0, 90));
            m_RB.MoveRotation(Quaternion.Euler(0, 0, 90));

            m_hasTurned = true;                        
        }
        else if (m_direction == Direction.None)
        {
            // We are dangling in the void. Time to turn!
            transform.Rotate(new Vector3(0, 0, 180));
            m_RB.MoveRotation(Quaternion.Euler(0, 0, 180));

            m_hasTurned = true;
        }

        ConnectToGround();
    }

    private void Movement()
    {
        if (m_isPaused)
            return;

        float originalDistanceToPlayer = (m_player.gameObject.transform.position - gameObject.transform.position).magnitude;

        if(originalDistanceToPlayer <= m_aggroRange)
        {
            m_lastDirection = m_direction;

            // Calculate whether left or right is closer to our target.
            Vector3 newPointLeft = gameObject.transform.position + (-gameObject.transform.right * m_speed * Time.fixedDeltaTime);
            Vector3 newPointRight = gameObject.transform.position + (gameObject.transform.right * m_speed * Time.fixedDeltaTime);

            float distanceToPlayerFromLeft = (m_player.gameObject.transform.position - newPointLeft).magnitude;
            float distanceToPlayerFromRight = (m_player.gameObject.transform.position - newPointRight).magnitude;

            if (distanceToPlayerFromLeft > originalDistanceToPlayer && distanceToPlayerFromRight > originalDistanceToPlayer)
            {
                // No movement keeps us closer. We dont move.
                m_direction = Direction.None;
            }
            else
            {
                Vector3 newPoint = distanceToPlayerFromLeft < distanceToPlayerFromRight ? newPointLeft : newPointRight;
                m_direction = distanceToPlayerFromLeft < distanceToPlayerFromRight ? Direction.Left : Direction.Right;

                m_RB.MovePosition(new Vector2(newPoint.x, newPoint.y));
            }
        }       
    }

    private void HandleCrawl()
    {
        if (m_isPaused)
            return;

        // Calculating where our collider corners are, at the base of our enemy.
        Vector3 downVector = -transform.up.normalized * (m_enemySize.y / 2);
        Vector3 rightVector = transform.right.normalized * (m_enemySize.x / 2);

        Vector3 rightCorner = transform.position + rightVector + (rightVector * 0.02f) + downVector; // Adding 0.02f to the y value so that we dont clip with the ground and detect nothing on our raycast.
        Vector3 leftCorner = transform.position - rightVector - (rightVector * 0.02f) + downVector; // Adding 0.02f to the y value so that we dont clip with the ground and detect nothing on our raycast.

        if (m_direction == Direction.Right)
        {
            // Going right.

            Debug.DrawRay(rightCorner, transform.right * m_wallCheckDistance, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(rightCorner, transform.right, m_wallCheckDistance, ~LayerMask.GetMask("Enemies", "Player"));

            if (hit.collider != null && hit.collider.CompareTag("Platform"))
            {
                // We hit a platform, so we hit a wall!
                transform.Rotate(new Vector3(0, 0, 90));
                m_RB.MoveRotation(Quaternion.Euler(0, 0, 90));

                m_hasTurned = true;
            }
        }
        else if(m_direction == Direction.Left)
        {
            // Going left.
            Debug.DrawRay(leftCorner, -transform.right * m_wallCheckDistance, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(leftCorner, -transform.right, m_wallCheckDistance, ~LayerMask.GetMask("Enemies", "Player"));

            if (hit.collider != null && hit.collider.CompareTag("Platform"))
            {
                // We hit a platform, so we hit a wall!
                transform.Rotate(new Vector3(0, 0, -90));
                m_RB.MoveRotation(Quaternion.Euler(0, 0, -90));

                m_hasTurned = true;
            }
        }
    }

    /// <summary>
    /// Moves the crawler slightly and checks for ground. Repeats until ground is found.
    /// </summary>
    private void ConnectToGround()
    {
        if (m_direction == Direction.None)
            return;

        Vector3 moveVector = m_direction == Direction.Left ? -transform.right.normalized : transform.right.normalized;
        Vector3 originalPosition = gameObject.transform.position;

        // If this count is reached, we are in the complete void. We should fall.
        int maxCount = 100;
        int count = 0;

        // Check for grounded. If not grounded, we move again.
        while(!IsGrounded() && count < maxCount)
        {
            // Move the crawler a bit.
            Vector2 newPoint = gameObject.transform.position + (moveVector * m_speed * Time.fixedDeltaTime);
            gameObject.transform.position = newPoint;

            count++;
        }

        if(count >= maxCount)
        {
            // We are in the complete void.
            // Returning to first position, and falling.
            gameObject.transform.position = originalPosition;

            m_RB.gravityScale = 1f;
            m_hasBeenPlaced = false;
            m_placeWithGravity = true;
        }
    }

    private void HandleAttack()
    {
        float distanceToPlayer = (m_player.gameObject.transform.position - gameObject.transform.position).magnitude;

        if (distanceToPlayer <= m_attackRange)
        {
            // Can't attack if our tongue is already out
            if(!m_tongue.m_tongueOut)
            {
                //Debug.Log("Attacking. TongueOut is " + m_tongue.m_tongueOut);
                m_tongue.LaunchTongue(m_player.gameObject.transform.position);
                m_attackPauseTimeElapsed = 0f;
            }
        }
    }

    public bool IsGrounded()
    {
        // Calculating where our collider corners are, at the base of our enemy.
        Vector3 downVector = -transform.up.normalized * (m_enemySize.y / 2);
        Vector3 rightVector = transform.right.normalized * (m_enemySize.x / 2);

        Vector3 rightCorner = transform.position + rightVector + (rightVector * 0.02f) + downVector; // Adding 0.02f to the y value so that we dont clip with the ground and detect nothing on our raycast.
        Vector3 leftCorner = transform.position - rightVector - (rightVector * 0.02f) + downVector; // Adding 0.02f to the y value so that we dont clip with the ground and detect nothing on our raycast.

        if (m_direction == Direction.Right)
        {
            // Going right.

            Debug.DrawRay(leftCorner, -transform.up * m_groundCheckDistance, Color.yellow);
            // Casting a ray downwards, to detect ground.
            RaycastHit2D hit = Physics2D.Raycast(leftCorner, -transform.up, m_groundCheckDistance, ~LayerMask.GetMask("Enemies", "Player"));

            if (hit.collider == null)
            {
                // Check if our other foot is set on ground.
                // Casting a ray downwards, to detect ground.
                Debug.DrawRay(rightCorner, -transform.up * m_groundCheckDistance, Color.yellow);
                RaycastHit2D hit2 = Physics2D.Raycast(rightCorner, -transform.up, m_groundCheckDistance, ~LayerMask.GetMask("Enemies", "Player"));

                if (hit2.collider == null)
                {
                    // We are dangling in the void. 
                    return false;
                }
            }
        }
        else if (m_direction == Direction.Left)
        {
            // Going left.
            Debug.DrawRay(rightCorner, -transform.up * m_groundCheckDistance, Color.yellow);
            // Casting a ray downwards, to detect ground.
            RaycastHit2D hit = Physics2D.Raycast(rightCorner, -transform.up, m_groundCheckDistance, ~LayerMask.GetMask("Enemies", "Player"));

            if (hit.collider == null)
            {
                // Check if our other foot is set on ground.
                // Casting a ray downwards, to detect ground.
                Debug.DrawRay(leftCorner, -transform.up * m_groundCheckDistance, Color.yellow);
                RaycastHit2D hit2 = Physics2D.Raycast(leftCorner, -transform.up, m_groundCheckDistance, ~LayerMask.GetMask("Enemies", "Player"));

                if (hit2.collider == null)
                {
                    // We are dangling in the void.
                    return false;
                }
            }
        }
        else if (m_direction == Direction.None)
        {
            Debug.DrawRay(leftCorner, -transform.up * m_groundCheckDistance, Color.yellow);
            // Casting a ray downwards, to detect ground.
            RaycastHit2D hit = Physics2D.Raycast(leftCorner, -transform.up, m_groundCheckDistance, ~LayerMask.GetMask("Enemies", "Player"));

            if (hit.collider == null)
            {
                // Check if our other foot is set on ground.
                // Casting a ray downwards, to detect ground.
                Debug.DrawRay(rightCorner, -transform.up * m_groundCheckDistance, Color.yellow);
                RaycastHit2D hit2 = Physics2D.Raycast(rightCorner, -transform.up, m_groundCheckDistance, ~LayerMask.GetMask("Enemies", "Player"));

                if (hit2.collider == null)
                {
                    // We are dangling in the void.
                    return false;
                }
            }
        }

        return true;
    }
}
