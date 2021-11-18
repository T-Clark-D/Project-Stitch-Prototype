using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform mTarget;
    [SerializeField] float mFollowSpeed;
    [SerializeField] float mFollowRange;

    void Update()
    {
        if (mTarget != null)
        {
            if ((Vector3.Distance(transform.position, mTarget.position)) <= mFollowRange)
            {
                transform.position = Vector2.MoveTowards(transform.position, mTarget.position, mFollowSpeed * Time.deltaTime);
            }
        }
    }

    public void SetTarget(Transform target)
    {
        mTarget = target;
    }
}