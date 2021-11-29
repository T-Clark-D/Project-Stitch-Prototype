using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlubberGust : MonoBehaviour
{
    [SerializeField] float mSpeed;
    [SerializeField] float mExpirationTime;
    Rigidbody2D mRigidBody2D;
    float mTimer;

    void Awake()
    {
        mRigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        mTimer += Time.deltaTime;
        if (mTimer >= mExpirationTime)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector2 direction)
    {
        mRigidBody2D.velocity = direction * mSpeed;
    }
}
