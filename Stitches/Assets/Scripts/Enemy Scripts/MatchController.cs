using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : MonoBehaviour
{
    public AudioClip[] m_whooshSounds;
    public AudioSource m_whooshAudioSource;

    public AudioClip[] m_fireSounds;
    public AudioSource m_fireAudioSource;

    public float m_whooshRange;
    private bool m_whooshHasPlayed = false;

    private PlayerController m_player;

    private void Awake()
    {
        int randomIndex = UnityEngine.Random.Range(0, m_fireSounds.Length);

        m_fireAudioSource.clip = m_fireSounds[randomIndex];
        m_fireAudioSource.loop = true;
        m_fireAudioSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If the player has been set...
        if(m_player != null)
        {
            // ... and the player is in range...
            if((m_player.transform.position - transform.position).magnitude <= m_whooshRange)
            {
                // ... and we haven't played the whoosh...
                if(!m_whooshHasPlayed)
                {
                    // Play the sound.
                    int randomIndex = UnityEngine.Random.Range(0, m_whooshSounds.Length);
                    m_whooshAudioSource.PlayOneShot(m_whooshSounds[randomIndex]);

                    m_whooshHasPlayed = true;
                }
            }
        }
    }

    public void PassPlayerObject(PlayerController pPlayer)
    {
        m_player = pPlayer;
    }
}
