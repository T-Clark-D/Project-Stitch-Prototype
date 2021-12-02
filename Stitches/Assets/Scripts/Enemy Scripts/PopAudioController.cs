using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopAudioController : MonoBehaviour
{
    private AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
