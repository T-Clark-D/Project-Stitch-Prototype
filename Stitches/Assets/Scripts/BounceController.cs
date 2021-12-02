using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceController : MonoBehaviour
{
    public AudioClip[] m_bounceSounds;
    public AudioSource m_bounceAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Play audio clip
        int randomIndex = UnityEngine.Random.Range(0, m_bounceSounds.Length);
        m_bounceAudioSource.PlayOneShot(m_bounceSounds[randomIndex]);
    }
}
