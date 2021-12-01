using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustController : MonoBehaviour
{
    public float m_timeBeforeDeath = 0.5f;
    public AudioClip[] m_dustSounds;

    private AudioSource m_audioSource;
    private ParticleSystem m_particleSystem;

    private void Awake()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
        m_audioSource = GetComponent<AudioSource>();
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
        int randomIndex = UnityEngine.Random.Range(0, m_dustSounds.Length);
        m_audioSource.PlayOneShot(m_dustSounds[randomIndex]);

        Destroy(gameObject, m_timeBeforeDeath);
    }
}
