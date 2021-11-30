using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustController : MonoBehaviour
{
    public float m_timeBeforeDeath = 0.5f;

    private ParticleSystem m_particleSystem;

    private void Awake()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAndDestroy()
    {
        m_particleSystem.Play();

        Destroy(gameObject, m_timeBeforeDeath);
    }
}
