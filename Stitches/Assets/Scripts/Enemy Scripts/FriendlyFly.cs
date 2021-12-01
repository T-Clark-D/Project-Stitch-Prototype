using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyFly : Enemy
{
    private Vector2 m_startPosition;
    private float circleFlyAngle = 360;
    private ParticleSystem m_flyerExplosion;
    public float flySpeed;
    public float flyRadius;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        m_flyerExplosion = GameObject.Find("Flyer_Explosion").GetComponent<ParticleSystem>();
        m_startPosition = transform.position;
        print(m_startPosition);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = m_startPosition + new Vector2(Mathf.Sin(circleFlyAngle), Mathf.Cos(circleFlyAngle)) * flyRadius;
        circleFlyAngle -= Time.deltaTime * flySpeed;
        if (circleFlyAngle <= 0) circleFlyAngle = 360;
    }
}
