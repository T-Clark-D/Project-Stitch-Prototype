using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoppy : Enemy
{
    [SerializeField] bool m_isFacingRight = true;
    [SerializeField] float maxJumpHeight;
    [SerializeField] Transform player;
    [SerializeField] float jumpCooldown;
    [SerializeField] float agroRange;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform groundDetection;
    private float patrolTimer = 7;
    private float patrolCooldown;
    private float coolDown;
    private bool canJump;
    private float distance;
    private bool inRange;
    private bool patrolling = true;
    private RaycastHit2D onGround;

    protected override void Start()
    {
        m_RB = GetComponent<Rigidbody2D>();
        m_SR = GetComponent<SpriteRenderer>();
        coolDown = jumpCooldown;
    }
    protected override void Update()
    {
        distance = Vector2.Distance(transform.position, player.position);   // Check distance between hoppy and player.
        onGround = Physics2D.Raycast(groundDetection.position, Vector2.down, 2);   // Check if hoppy is on the ground before jumping again.
        rangeCheck();                                                       // If in range then is able to attack, else hoppy must patrol.
        coolDownCheck();                                                    // Check if the jump cooldown is finished.
    }
    void FixedUpdate()
    {
        if (inRange)
        {
            attack();
        }
        else if(patrolling)
        {
            patrol();
        }

  
    }

    void attack()
    {
        lookAtPlayer();     // Sprite will flip towards the player's direction.

        if (canJump && onGround.collider == true)
        {
            jumpToPlayer();     // If the jump cooldown is zero and hoppy is grounded, then jump.
            canJump = false;
        }
        else
        {
            inRange = false;
        }
    }

    void patrol()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);        // hoppy's patrol movement.

        RaycastHit2D isGrounded = Physics2D.Raycast(groundDetection.position, Vector2.down, 2);     // Check if hoppy reached the edge of a platform before moving in the opposite direction.
        if(isGrounded.collider == false && onGround.collider == true)
        {
            flip();
        }
    }

    void rangeCheck()
    {
        if (distance < agroRange)       // In attack mode if in agro range.
        {
            inRange = true;
            patrolling = false;
        }
        else
        {
            inRange = false;            // If not in attack mode, then in patrol mode.
            patrolCoolDownCheck();
        }
    }

    void coolDownCheck()                // Check if hoppy can jump again.
    {
        if (coolDown <= 0 && !canJump)
        {
            canJump = true;
            coolDown = jumpCooldown;          
        }
        else
        {
            coolDown -= Time.deltaTime;

        }
    }

    void patrolCoolDownCheck()          // Check if hoppy can start patrolling again or be on the lookout for the player getting in range again.
    {
        if (patrolCooldown <= 0 && !patrolling)
        {
            patrolCooldown = patrolTimer;
            patrolling = true;
        }
        else
        {
            patrolCooldown -= Time.deltaTime;

        }
    }

    void flip()     // Flip the sprite when the edge of the platform is reached or is looking at the player in attack mode.
    {
        if (m_isFacingRight)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            m_isFacingRight = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            m_isFacingRight = true;
        }
    }

    void lookAtPlayer()     // Flip the sprite to look in the player's direction.
    {

        
        if (transform.position.x > player.position.x && m_isFacingRight)
        {
            flip();
            m_isFacingRight = false;
        }
        else if (transform.position.x < player.position.x && !m_isFacingRight)
        {
            flip();
            m_isFacingRight = true;
        }

    }

    void jumpToPlayer()
    {
        float distanceToPlayer = player.position.x - transform.position.x;
        float offset = Random.Range(1, 5);

        if(distance > maxJumpHeight)
        {
            distance = maxJumpHeight;
        }
        else if(distance < 5)
        {
            distance = 5;
        }


        m_RB.AddForce(new Vector2(distanceToPlayer, distance + offset), ForceMode2D.Impulse);
    }
}
