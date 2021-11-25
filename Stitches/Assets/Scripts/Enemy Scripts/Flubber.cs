using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flubber : Enemy
{
    [SerializeField] public Vector2 worldPoint;
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

    bool mInRange;
    bool gustShot;
    float mTime;
    float gustDuration = 0.75f;
    Rigidbody2D mRigidBody2D; // Temporary

    Vector2 mFacingDirection;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        mRigidBody2D = GetComponent<Rigidbody2D>();
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
        FlipDirection();
        if (mTarget != null)
        {
            FollowPlayer();
        }
        if (mInRange)
        {
            mTime += Time.deltaTime;
            GustPlayer();
            
        }
        if(gustShot)
        {
            mTime += Time.deltaTime;
            if (mTime > gustDuration)
            {
                mRigidBody2D.velocity = Vector3.zero;
                mRigidBody2D.Sleep();
                gustShot = false;
            }
        }

    }

    private void FollowPlayer()
    {
        if ((Vector3.Distance(transform.position, mTarget.position)) <= mFollowRange && (Vector3.Distance(transform.position, mTarget.position)) >= mStopRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, mTarget.position, mFollowSpeed * Time.deltaTime);
            
        }
        if ((Vector3.Distance(transform.position, mTarget.position)) <= mStopRange)
        {
            mInRange = true;
        }
        else
        {
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

        //m_RB.velocity = -shootDir * mGustRecoil;
        mRigidBody2D.velocity = -shootDir * mGustRecoil;
        mTime = 0.0f;
    }

    private void FaceDirection(Vector2 direction)
    {
        mFacingDirection = direction;
        GetComponent<SpriteRenderer>().flipX = direction != Vector2.right;
    }

    private void FlubberFour()
    {
        LayerMask mask = LayerMask.GetMask("Player");
        Collider2D player = Physics2D.OverlapCircle((Vector2)transform.position, agroRadius, mask);
        Vector2 targetPoint = worldPoint;
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