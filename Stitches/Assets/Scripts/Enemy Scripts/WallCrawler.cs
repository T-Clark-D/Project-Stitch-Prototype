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
    public float m_wallCheckDistance = 0.02f;

    [SerializeField] private PlayerController m_player;
    private Vector3 m_enemySize;
    private Direction m_direction = Direction.None;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_enemySize = m_collider.bounds.size;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        HandleCrawl();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        float originalDistanceToPlayer = (m_player.gameObject.transform.position - gameObject.transform.position).magnitude;

        if(originalDistanceToPlayer <= m_aggroRange)
        {
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
        // Calculating where our collider corners are, at the base of our enemy.
        Vector3 downVector = -transform.up.normalized * (m_enemySize.y / 2);
        Vector3 rightVector = transform.right.normalized * (m_enemySize.x / 2);

        Vector3 rightCorner = transform.position + new Vector3(rightVector.x + 0.02f, downVector.y, m_enemySize.z / 2); // Adding 0.02f to the y value so that we dont clip with the ground and detect nothing on our raycast.
        Vector3 leftCorner = transform.position + new Vector3(-(rightVector.x + 0.02f), downVector.y, m_enemySize.z / 2); // Adding 0.02f to the y value so that we dont clip with the ground and detect nothing on our raycast.

        if(m_direction == Direction.Right)
        {
            // Going right.

            RaycastHit2D hit = Physics2D.Raycast(rightCorner, transform.right, m_wallCheckDistance, LayerMask.GetMask("Enemy", "Player"));

            if (hit.collider != null && hit.collider.CompareTag("Platform"))
            {
                // We hit a platform, so we hit a wall!
                transform.Rotate(new Vector3(0, 0, 90));
                m_RB.MoveRotation(Quaternion.Euler(0, 0, 90));
            }
        }
        else if(m_direction == Direction.Left)
        {
            // Going left.
            RaycastHit2D hit = Physics2D.Raycast(leftCorner, -transform.right, m_wallCheckDistance, LayerMask.GetMask("Enemy", "Player"));

            if (hit.collider != null && hit.collider.CompareTag("Platform"))
            {
                // We hit a platform, so we hit a wall!
                transform.Rotate(new Vector3(0, 0, -90));
                m_RB.MoveRotation(Quaternion.Euler(0, 0, -90));
            }
        }
    }
}
