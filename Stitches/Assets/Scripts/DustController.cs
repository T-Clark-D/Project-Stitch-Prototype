using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustController : MonoBehaviour
{
    public float m_timeBeforeDeath = 0.5f;

    private float initializationTime;

    private ParticleSystem m_particleSystem;

    private void Awake()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        initializationTime = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad - initializationTime > 5) Destroy(this.gameObject);
    }

    public void PlayAndDestroy()
    {
        m_particleSystem.Play();

        Destroy(gameObject, m_timeBeforeDeath);
    }
}
