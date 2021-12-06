using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoppy : Enemy
{
    public AudioClip[] m_jumpSounds;
    public AudioSource m_jumpAudioSource;
    public AudioClip[] m_attackSounds;
    public AudioSource m_attackAudioSource;

    [SerializeField] bool m_isFacingRight = true;
    [SerializeField] float maxJumpHeight;
    [SerializeField] Transform player;
    [SerializeField] float jumpCooldown;
    [SerializeField] float agroRange;
    [SerializeField] Transform groundDetection;
    [SerializeField] float minHopDist;
    [SerializeField] float maxHopDist;
    [SerializeField] float hopJumpHeight;

    private float m_patrolTimer = 7;
    private float m_pCooldown;
    private bool m_patrolling = true;

    private float m_coolDown;
    private bool m_isGrounded;
    private float m_distance;
    private RaycastHit2D m_onGround;
    private bool m_jumping;
    private CapsuleCollider2D m_legHitBox;

    protected override void Start()
    {
        base.Start();
        m_legHitBox = GetComponent<CapsuleCollider2D>();
        m_coolDown = jumpCooldown;
        m_legHitBox.enabled = false;
    }
    public void Update()
    {
        base.Update();
        m_distance = Vector2.Distance(transform.position, player.position);   // Check distance between hoppy and player.
        m_onGround = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.1f);   // Check if hoppy is on the ground before jumping again.
        if (m_distance < agroRange)
        {
            lookAtPlayer();     // Sprite will flip towards the player's direction.
        }

        
    }
    void FixedUpdate()
    {
        // Toggle ai on and off
        if(m_frozen)
        {
            return;
        }
        else
        {
            // Check if on ground before jumping
            CheckIfGrounded();

            // If player in range and the cooldown for jumping is zero then jump
            if (InRangeCheck() && JumpCooldown())
            {
                m_patrolling = false;
                if (m_isGrounded && !m_jumping)
                {
                    m_jumping = true;
                    m_anim.SetBool("Jumping", true);
                    m_legHitBox.enabled = true;
                    GetComponent<Spike>().enabled = true;
                }
                else if (m_isGrounded && m_jumping)
                {
                    m_jumping = false;
                    m_anim.SetBool("Jumping", false);
                    m_legHitBox.enabled = false;
                    GetComponent<Spike>().enabled = false;
                }
            }
            else if (!InRangeCheck() && m_isGrounded)   // Start patrolling if player is not in range
            {
                m_patrolling = true;
                m_jumping = false;
                m_anim.SetBool("Jumping", false);
                m_legHitBox.enabled = false;
            }

            // Patrol jumping
            if (m_patrolling && PatrolCooldown())
            {
                if (m_isGrounded && !m_jumping)
                {
                    m_jumping = true;
                    m_anim.SetBool("Jumping", true);
                }
                else if (m_isGrounded && m_jumping)
                {
                    m_jumping = false;
                    m_anim.SetBool("Jumping", false);
                }
            }
        }
        

    }

    // Check if player in range
    bool InRangeCheck()
    {
        if (m_distance < agroRange)       // In attack mode if in agro range.
        {
            return true;
        }
        return false;
    }

    // Time before next jump is allowed
    bool JumpCooldown()                // Check if hoppy can jump again.
    {

        if (m_coolDown <= 0)
        {
            m_coolDown = jumpCooldown;
            return true;
        }
        else
        {
            m_coolDown -= Time.deltaTime;

        }

        return false;
    }

    // Time before next jump while patrolling
    bool PatrolCooldown()                // Check if hoppy can jump again.
    {

        if (m_pCooldown <= 0)
        {
            m_pCooldown = m_patrolTimer;
            return true;
        }
        else
        {
            m_pCooldown -= Time.deltaTime;

        }

        return false;
    }

    // Flip sprite depending on the direction it's facing
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

    // Sprite will face the player while in range
    void lookAtPlayer()     // Flip the sprite to look in the player's direction.
    {
        if (transform.position.x > player.position.x && m_isFacingRight)
        {
            flip();
        }
        else if (transform.position.x < player.position.x && !m_isFacingRight)
        {
            flip();
        }

    }

    void CheckIfGrounded()
    {
        if (m_onGround.collider == true)
        {
            m_isGrounded = true;

        }
        else if (m_onGround.collider == false)
        {
            m_isGrounded = false;
        }

    }

    // Jump to player if in range or hop if patrolling
    void JumpToPlayer()
    {
        float distanceToPlayer = player.position.x - transform.position.x;
        float offset = Random.Range(1, 3);
        float jumpHeight = m_distance;

        if (m_distance > maxJumpHeight)
        {
            m_distance = maxJumpHeight;
        }

        if (m_patrolling)
        {
            float direction = Random.Range(0, 3);
            distanceToPlayer = Random.Range(minHopDist, maxHopDist);
            jumpHeight = hopJumpHeight;
            if(direction > 0 && m_isFacingRight)
            {
                distanceToPlayer *= -1;
                flip();
            }
            else if(direction == 0 && !m_isFacingRight)
            {
                flip();
            }

            
        }

        m_RB.AddForce(new Vector2(distanceToPlayer, jumpHeight), ForceMode2D.Impulse);          // Jump Attacking Enemy AI in Unity 2D Part 2: https://youtu.be/fviU0V6nivs
        PlayJumpSound();



    }

    // Play jump sound
    void PlayJumpSound()
    {
        // Play audio clip
        int randomIndex = UnityEngine.Random.Range(0, m_jumpSounds.Length);
        m_jumpAudioSource.PlayOneShot(m_jumpSounds[randomIndex]);
    }
    // Play attack sounds
    void PlayAttackSounds()
    {
        int randomIndex = UnityEngine.Random.Range(0, m_attackSounds.Length);
        m_attackAudioSource.PlayOneShot(m_attackSounds[randomIndex]);
    }

    // Take HP away from player if the collide with hoppy
    private void OnCollisonEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            GameHandler gh = FindObjectOfType<GameHandler>();
            gh.takeDamage();
        }
    }
}