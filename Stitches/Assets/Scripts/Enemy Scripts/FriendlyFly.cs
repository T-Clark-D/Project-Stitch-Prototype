using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyFly : Enemy
{
    public AudioClip[] m_flyingSounds;
    public AudioSource m_flyingAudioSource;
    public float flySpeed;
    public float flyRadius;

    private Vector2 m_startPosition;
    private float circleFlyAngle = 360;
    private ParticleSystem m_flyerExplosion;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_flyerExplosion = GameObject.Find("Flyer_Explosion").GetComponent<ParticleSystem>();
        m_startPosition = transform.position;
        print(m_startPosition);

        // Play audio clip
        int randomIndex = UnityEngine.Random.Range(0, m_flyingSounds.Length);
        m_flyingAudioSource.clip = m_flyingSounds[randomIndex];
        m_flyingAudioSource.loop = true;
        m_flyingAudioSource.Play();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(!m_isHooked)
        {
            /*
            transform.position = m_startPosition + new Vector2(Mathf.Sin(circleFlyAngle), Mathf.Cos(circleFlyAngle)) * flyRadius;
            circleFlyAngle -= Time.deltaTime * flySpeed;
            if (circleFlyAngle <= 0) circleFlyAngle = 360;*/
        }
      
        
    }
}
