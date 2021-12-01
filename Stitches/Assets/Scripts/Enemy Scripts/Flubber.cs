using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flubber : Enemy
{
    Vector3 worldPoint;
    [SerializeField] public float interval;
    public float agroRadius;

    public float force = 15;

    /// <summary>
    /// Time the enemy will stay frozen after the player unhooking from it.
    /// </summary>

    /// <summary>
    /// Time the enemy will stay vulnerable after the player unhooking from it.
    /// </summary>
    /// 
    private float lastForce = 0;


    [SerializeField] Transform mTarget;
    [SerializeField] float mFollowSpeed;
    [SerializeField] float mFollowRange;
    [SerializeField] float mStopRange;
    [SerializeField] GameObject mGustPrefab;
    [SerializeField] float mGustRecoil;

    [SerializeField] float frequency;
    [SerializeField] float magnitude;
    [SerializeField] float distance;
    bool idleRight = false;
    Vector3 currentPos;
    bool idle = true;

    bool mInRange;
    bool gustShot;
    float mTime;
    float gustDuration = 0.75f;
    Rigidbody2D mRigidBody2D; // Temporary

    Vector2 mFacingDirection;
    bool mChasing;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        mRigidBody2D = GetComponent<Rigidbody2D>();
        worldPoint = transform.position;
        currentPos = transform.position;
        /*if (m_frozen)
            Freeze();*/
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!m_aiIsOff)
        {
            //FlubberOne();
            //FlubberTwo();
            //FlubberThree();

            //flys around world point until player comes in proximity
            FlubberFour();
        }
        if (mTarget != null)
        {
            FollowPlayer();
        }
        if (mInRange)
        {
            GustPlayer();
        }
        if (gustShot)
        {
            mTime += Time.deltaTime;
            if (mTime > gustDuration)
            {
                mRigidBody2D.velocity = mRigidBody2D.velocity * 0.001f * Time.deltaTime;
                if (mRigidBody2D.velocity.magnitude <= 0.0f)
                {
                    mRigidBody2D.velocity = Vector3.zero;
                    mRigidBody2D.Sleep();
                    gustShot = false;
                    mTime = 0.0f;
                }
            }
        }
        if (idle)
        {
            CheckDirection();
            IdleMove();
        }

        if (!mChasing && !idle)
        {
            transform.position = Vector2.MoveTowards(transform.position, worldPoint, mFollowSpeed * Time.deltaTime);
            if(transform.position.x == worldPoint.x && transform.position.y == worldPoint.y)
            {
                mTime += Time.deltaTime;
                if (mTime > 1.0f) {
                    idle = true;
                    mTime = 0.0f;
                }
            }
        }
    }

    private void FollowPlayer()
    {
        if ((Vector3.Distance(transform.position, mTarget.position)) <= mFollowRange && (Vector3.Distance(transform.position, mTarget.position)) >= mStopRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, mTarget.position, mFollowSpeed * Time.deltaTime);
            FlipDirection();
            idle = false;
            mChasing = true;
            mTime = 0.0f;
        }
        if ((Vector3.Distance(transform.position, mTarget.position)) <= mStopRange)
        {
            mInRange = true;
        }
        else
        {
            mChasing = false;
            mInRange = false;
        }
    }

    private void GustPlayer()
    {
        gustShot = true;
        Vector2 targetPoint = mTarget.position;
        GameObject gustObj = Instantiate(mGustPrefab, transform.position, transform.rotation);
        FlubberGust gustRef = gustObj.GetComponent<FlubberGust>();
        Vector2 shootDir = (targetPoint - (Vector2)this.transform.position).normalized;
        gustRef.SetDirection(shootDir);

        Vector3 force = -shootDir * mGustRecoil;
        //m_RB.velocity = -shootDir * mGustRecoil;
        //mRigidBody2D.AddForce(force, ForceMode2D.Force);
        mRigidBody2D.velocity = -shootDir * mGustRecoil;
        mTime = 0.0f;
    }

    private void FaceDirection(Vector2 direction)
    {
        mFacingDirection = direction;
        GetComponent<SpriteRenderer>().flipX = direction != Vector2.right;
    }

    private void FlipDirection()
    {

        if (transform.position.x > mTarget.position.x)
        {
            FaceDirection(Vector2.right);
        }
        else
        {
            FaceDirection(-Vector2.right);
        }

        /*if (m_RB.velocity.x > 0)
        {
            m_SR.flipX = true;
        }
        else
        {
            m_SR.flipX = false;
        }*/
    }

    private void CheckDirection()
    {
        if (transform.position.x - worldPoint.x <= -distance)
        {
            FaceDirection(-Vector2.right);
            idleRight = true;
        }
        else if (transform.position.x - worldPoint.x >= distance)
        {
            FaceDirection(Vector2.right);
            idleRight = false;
        }
    }

    private void IdleMove()
    {
        if (idleRight)
        {
            currentPos += transform.right * Time.deltaTime * mFollowSpeed;
            transform.position = currentPos + transform.up * Mathf.Sin(Time.time * frequency) * magnitude;
        }
        else
        {
            currentPos -= transform.right * Time.deltaTime * mFollowSpeed;
            transform.position = currentPos + transform.up * Mathf.Sin(Time.time * frequency) * magnitude;
        }
    }

    private void FlubberFour()
    {
        LayerMask mask = LayerMask.GetMask("Player");
        Collider2D player = Physics2D.OverlapCircle((Vector2)transform.position, agroRadius, mask);
        Vector2 targetPoint = new Vector2(worldPoint.x, worldPoint.y);
        if (player != null)
        {
            if ((player.transform.position - this.transform.position).magnitude < agroRadius)
            {
                targetPoint = player.transform.position;
            }
        }
        // If we are hooked by the player, we cant move.
        if (!m_isHooked)
        {
            if (Time.timeSinceLevelLoad - lastForce >= interval)
            {
                m_RB.AddForce((targetPoint - (Vector2)this.transform.position).normalized * force, ForceMode2D.Impulse);
                lastForce = Time.timeSinceLevelLoad;
            }
        }
    }





    //extra AIs 

    /*
 
    private void FlubberTwo()
    {
        if (Time.timeSinceLevelLoad - lastForce >= interval)
        {
            RB.AddForce((player.transform.position - this.transform.position).normalized * force, ForceMode2D.Impulse);
            lastForce = Time.timeSinceLevelLoad;
        }
    }
    private void FlubberOne()
    {
        if (Time.timeSinceLevelLoad - lastForce >= interval)
        {
            switch (Random.Range(0, 2))
            {
                case 0:
                    RB.AddForce(Vector3.up * force, ForceMode2D.Impulse);
                    break;
                case 1:
                    RB.AddForce(new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)).normalized * force, ForceMode2D.Impulse);
                    break;
            }
            RB.AddForce(Vector3.up * force, ForceMode2D.Impulse);
            lastForce = Time.timeSinceLevelLoad;
        }
    }*/
}