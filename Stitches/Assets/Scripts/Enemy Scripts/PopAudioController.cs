using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopAudioController : MonoBehaviour
{
    private AudioSource m_audioSource;
    private bool m_hasPlayedOnce = false;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_audioSource.isPlaying && m_hasPlayedOnce)
        {
            Destroy(gameObject);
        }
    }

    public void Play()
    {
        m_audioSource.Play();
        m_hasPlayedOnce = true;
    }
}
