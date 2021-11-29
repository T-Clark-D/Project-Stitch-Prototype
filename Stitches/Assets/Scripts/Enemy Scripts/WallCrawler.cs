using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public float m_timeBetweenAttacks = 2f;
    /// <summary>
    /// Determines whether the crawler will be placed in the scene originally using gravity or not.
    /// If true, the crawler will have gravity enabled for its initial placement.
    /// </summary>
    public bool m_placeWithGravity = false;

    [SerializeField] private PlayerController m_player;
    [SerializeField] private TongueController m_tongue;
    private BoxCollider2D m_groundCheckCollider;
    private Vector3 m_enemySize;
    private Direction m_direction = Direction.None;
    private Direction m_lastDirection = Direction.None;
    private bool m_hasBeenPlaced = false;
    private float m_pauseTimeElapsed = 0f;
    private bool m_isPaused = false;
    private bool m_hasTurned = false;
    private float m_nudgeAmount = 0.02f;
    private int m_maxNudgeCount = 100;

    private bool m_hasAttacked = false;
    private float m_attackPauseTimeElapsed = 0f;

    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        m_tongue = this.GetComponentInChildren<TongueController>();
        m_animator = gameObject.GetComponentInChildren<Animator>();

        m_groundCheckCollider = GetComponentsInChildren<BoxCollider2D>().Where(x => x.CompareTag("GroundCheck")).FirstOrDefault();
        m_groundCheckCollider.size = new Vector3(m_groundCheckCollider.bounds.size.x, m_collider.bounds.size.y, m_groundCheckCollider.bounds.size.z);

        m_enemySize = m_groundCheckCollider.bounds.size;

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
            m_RB.isKinematic = true;
            PreventOverlap();
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

                m_RB.isKinematic = true;
                PreventOverlap();
            }
        }
    }

    private void FixedUpdate()
    {

       
        if (m_hasBeenPlaced)
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
            //m_RB.MoveRotation(Quaternion.Euler(0, 0, -90));

            m_hasTurned = true;
        }
        else if (m_direction == Direction.Left)
        {
            // Going left.
            transform.Rotate(new Vector3(0, 0, 90));
            //m_RB.MoveRotation(Quaternion.Euler(0, 0, 90));

            m_hasTurned = true;                        
        }
        else if (m_direction == Direction.None)
        {
            // We are dangling in the void. Time to turn!
            transform.Rotate(new Vector3(0, 0, 180));
            //m_RB.MoveRotation(Quaternion.Euler(0, 0, 180));

            m_hasTurned = true;
        }

        ConnectToGround();
        PreventOverlap();
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

                transform.position = newPoint;
                //m_RB.MovePosition(new Vector2(newPoint.x, newPoint.y));
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

        Vector3 rightEdge = transform.position + rightVector + (rightVector * 0.02f); // Adding 0.02f to the y value so that we dont clip with the ground and detect nothing on our raycast.
        Vector3 leftEdge = transform.position - rightVector - (rightVector * 0.02f); // Adding 0.02f to the y value so that we dont clip with the ground and detect nothing on our raycast.

        if (m_direction == Direction.Right)
        {
            // Going right.
            if (m_spriteRenderer.flipX == true)
            {
                float origX = m_spriteRenderer.transform.localPosition.x;

                m_spriteRenderer.flipX = false;

                // Recentering the renderer, as it is off centered.
                m_spriteRenderer.transform.localPosition = new Vector3(-origX, m_spriteRenderer.transform.localPosition.y, m_spriteRenderer.transform.localPosition.z);
            }

            Debug.DrawRay(rightEdge, transform.right * m_wallCheckDistance, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(rightEdge, transform.right, m_wallCheckDistance, ~LayerMask.GetMask("Enemies", "Player"));

            if (hit.collider != null && hit.collider.CompareTag("Platform"))
            {
                // We hit a platform, so we hit a wall!
                transform.Rotate(new Vector3(0, 0, 90));
                //m_RB.MoveRotation(Quaternion.Euler(0, 0, 90));

                m_hasTurned = true;
            }
        }
        else if(m_direction == Direction.Left)
        {
            if (m_spriteRenderer.flipX == false)
            {
                float origX = m_spriteRenderer.transform.localPosition.x;

                m_spriteRenderer.flipX = true;

                // Recentering the renderer, as it is off centered.
                m_spriteRenderer.transform.localPosition = new Vector3(-origX, m_spriteRenderer.transform.localPosition.y, m_spriteRenderer.transform.localPosition.z);
            }

            // GetComponentInChildren<Transform>().position = new Vector3(-GetComponentInChildren<Transform>().localScale.x, GetComponentInChildren<Transform>().localScale.y, GetComponentInChildren<Transform>().localScale.z);

            // Going left.
            Debug.DrawRay(leftEdge, -transform.right * m_wallCheckDistance, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(leftEdge, -transform.right, m_wallCheckDistance, ~LayerMask.GetMask("Enemies", "Player"));

            if (hit.collider != null && hit.collider.CompareTag("Platform"))
            {
                // We hit a platform, so we hit a wall!
                transform.Rotate(new Vector3(0, 0, -90));
                //m_RB.MoveRotation(Quaternion.Euler(0, 0, -90));

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
        int maxCount = m_maxNudgeCount;
        int count = 0;

        Vector3 newPoint = gameObject.transform.position;

        // Check for grounded. If not grounded, we move again.
        while (!IsGrounded() && count < maxCount)
        {
            // Move the crawler a bit.
            newPoint = newPoint + (moveVector * m_nudgeAmount);
            gameObject.transform.position = newPoint;
            m_collider.transform.position = newPoint;
            m_groundCheckCollider.transform.position = newPoint;
            m_collider.attachedRigidbody.position = newPoint;

            count++;
        }

        if(count >= maxCount)
        {
            // We are in the complete void.
            // Returning to first position, and falling.
            gameObject.transform.position = originalPosition;
            m_collider.transform.position = originalPosition;
            m_groundCheckCollider.transform.position = originalPosition;
            m_collider.attachedRigidbody.position = originalPosition;

            m_RB.gravityScale = 1f;
            m_hasBeenPlaced = false;
            m_placeWithGravity = true;
        }
    }

    /// <summary>
    /// Prevents the collider from overlapping the platform.
    /// </summary>
    private void PreventOverlap()
    {
        if (!IsGrounded())
        {
            return;
        }

        Vector3 originalPosition = gameObject.transform.position;
        Vector3 moveVector = transform.up.normalized * (m_enemySize.y / 2);
        float idealDistanceFromPlatform = 0.2f;

        // If this count is reached, we are unable to prevent overlap.
        int maxCount = m_maxNudgeCount;
        int count = 0;

        bool hasBeenNudgedInOppositeDirection = false;
        bool lastMoveWasUp = false;
        bool isAtProperDistanceFromPlatform = false;

        // Calculating where our collider corners are, at the base of our enemy.
        Vector3 downVector = -transform.up.normalized * (m_enemySize.y / 2);
        Vector3 rightVector = transform.right.normalized * (m_enemySize.x / 2);

        Vector3 rightCorner = transform.position + rightVector + (rightVector * 0.02f) + downVector; // Adding 0.02f to the y value so that we dont clip with the ground and detect nothing on our raycast.
        Vector3 leftCorner = transform.position - rightVector - (rightVector * 0.02f) + downVector; // Adding 0.02f to the y value so that we dont clip with the ground and detect nothing on our raycast.
        Vector3 center = transform.position + (downVector * 0.02f) + downVector; // Adding 0.02f to the y value so that we dont clip with the ground and detect nothing on our raycast.;

        // Casting a ray downwards, to detect ground.
        RaycastHit2D hit = Physics2D.Raycast(leftCorner, -transform.up, m_groundCheckDistance, ~LayerMask.GetMask("Enemies", "Player"));

        if (hit.collider == null)
        {
            // Check if our other foot is set on ground.
            // Casting a ray downwards, to detect ground.
            hit = Physics2D.Raycast(rightCorner, -transform.up, m_groundCheckDistance, ~LayerMask.GetMask("Enemies", "Player"));

            if (hit.collider == null)
            {
                // Check if our center is on the ground
                // Casting a ray downwards, to detect ground.
                hit = Physics2D.Raycast(center, -transform.up, m_groundCheckDistance, ~LayerMask.GetMask("Enemies", "Player"));

                if (hit.collider == null)
                {
                    // We are dangling in the void. 
                    // This should never happen, as we are grounded.
                    throw new Exception("PreventOverlap error: Enemy was not grounded.");
                }
            }
        }

        if (hit.distance != idealDistanceFromPlatform)
        {
            isAtProperDistanceFromPlatform = false;
        }
        else
        {
            isAtProperDistanceFromPlatform = true;
        }

        Vector3 newPoint = gameObject.transform.position;

        while(!isAtProperDistanceFromPlatform && count < maxCount)
        {
            if(hit.distance > idealDistanceFromPlatform)
            {
                if(lastMoveWasUp)
                {
                    // This is our last move. Bring it back down and leave it there.
                    // Move the crawler a bit downwards.
                    newPoint = newPoint + (downVector.normalized * m_nudgeAmount / 2);
                    gameObject.transform.position = newPoint;
                    m_collider.transform.position = newPoint;
                    m_groundCheckCollider.transform.position = newPoint;
                    m_collider.attachedRigidbody.position = newPoint;

                    hasBeenNudgedInOppositeDirection = true;
                }
                else
                {
                    // Move the crawler a bit downwards.
                    newPoint = newPoint + (downVector.normalized * m_nudgeAmount);
                    gameObject.transform.position = newPoint;
                    m_collider.transform.position = newPoint;
                    m_groundCheckCollider.transform.position = newPoint;
                    m_collider.attachedRigidbody.position = newPoint;
                }
                lastMoveWasUp = false;
            }
            else if(hit.distance < idealDistanceFromPlatform)
            {
                if (!lastMoveWasUp)
                {
                    // This is our last move. Bring it back up a bit and leave it there.
                    // Move the crawler a bit upwards.
                    newPoint = newPoint + (-downVector.normalized * m_nudgeAmount / 2);
                    gameObject.transform.position = newPoint;
                    m_collider.transform.position = newPoint;
                    m_groundCheckCollider.transform.position = newPoint;
                    m_collider.attachedRigidbody.position = newPoint;

                    hasBeenNudgedInOppositeDirection = true;
                }
                else
                {
                    // Move the crawler a bit upwards.
                    newPoint = newPoint + (-downVector.normalized * m_nudgeAmount);
                    gameObject.transform.position = newPoint;
                    m_collider.transform.position = newPoint;
                    m_groundCheckCollider.transform.position = newPoint;
                    m_collider.attachedRigidbody.position = newPoint;
                }

                lastMoveWasUp = true;
            }

            count++;

            if(!hasBeenNudgedInOppositeDirection)
            {
                // Casting a ray downwards, to detect ground.
                hit = Physics2D.Raycast(leftCorner, -transform.up, m_groundCheckDistance, ~LayerMask.GetMask("Enemies", "Player"));

                if (hit.collider == null)
                {
                    // Check if our other foot is set on ground.
                    // Casting a ray downwards, to detect ground.
                    hit = Physics2D.Raycast(rightCorner, -transform.up, m_groundCheckDistance, ~LayerMask.GetMask("Enemies", "Player"));

                    if (hit.collider == null)
                    {
                        // Check if our center is on the ground
                        // Casting a ray downwards, to detect ground.
                        hit = Physics2D.Raycast(center, -transform.up, m_groundCheckDistance, ~LayerMask.GetMask("Enemies", "Player"));

                        if (hit.collider == null)
                        {
                            // We are dangling in the void. 
                            // This should never happen, as we are grounded.
                            throw new Exception("PreventOverlap error: Enemy was not grounded.");
                        }
                    }
                }

                if (hit.distance != idealDistanceFromPlatform)
                {
                    isAtProperDistanceFromPlatform = false;
                }
                else
                {
                    isAtProperDistanceFromPlatform = true;
                }
            }
        }

        if (count >= maxCount)
        {
            // We failed to place back our crawler. We make it go back to its original position.
            gameObject.transform.position = originalPosition;
            m_collider.transform.position = originalPosition;
            m_groundCheckCollider.transform.position = originalPosition;
            m_collider.attachedRigidbody.position = originalPosition;
        }
    }

    private void HandleAttack()
    {
        float distanceToPlayer = (m_player.gameObject.transform.position - gameObject.transform.position).magnitude;

        if (distanceToPlayer <= m_attackRange)
        {
            m_animator.SetTrigger("Attack");
            // Can't attack if our tongue is already out
            if (!m_tongue.m_tongueOut && m_tongue.m_canAttack)
            {
                //Debug.Log("Attacking. TongueOut is " + m_tongue.m_tongueOut);
                m_tongue.LaunchTongue(m_player.gameObject.transform.position);
                m_attackPauseTimeElapsed = 0f;
                //m_animator.ResetTrigger("Attack");
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
        Vector3 center = transform.position + (downVector * 0.02f) + downVector; // Adding 0.02f to the y value so that we dont clip with the ground and detect nothing on our raycast.;

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
                    // Check if our center is on the ground
                    // Casting a ray downwards, to detect ground.
                    Debug.DrawRay(center, -transform.up * m_groundCheckDistance, Color.yellow);
                    RaycastHit2D hit3 = Physics2D.Raycast(center, -transform.up, m_groundCheckDistance, ~LayerMask.GetMask("Enemies", "Player"));

                    if(hit3.collider == null)
                    {
                        // We are dangling in the void. 
                        return false;
                    }
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
                    // Check if our center is on the ground
                    // Casting a ray downwards, to detect ground.
                    Debug.DrawRay(center, -transform.up * m_groundCheckDistance, Color.yellow);
                    RaycastHit2D hit3 = Physics2D.Raycast(center, -transform.up, m_groundCheckDistance, ~LayerMask.GetMask("Enemies", "Player"));

                    if (hit3.collider == null)
                    {
                        // We are dangling in the void. 
                        return false;
                    }
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
                    // Check if our center is on the ground
                    // Casting a ray downwards, to detect ground.
                    Debug.DrawRay(center, -transform.up * m_groundCheckDistance, Color.yellow);
                    RaycastHit2D hit3 = Physics2D.Raycast(center, -transform.up, m_groundCheckDistance, ~LayerMask.GetMask("Enemies", "Player"));

                    if (hit3.collider == null)
                    {
                        // We are dangling in the void. 
                        return false;
                    }
                }
            }
        }

        return true;
    }
}
