using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    public AudioClip[] m_fireLoopSounds;
    public AudioSource m_fireAudioSource;

    [SerializeField] GameObject fire;
    Vector3 worldPoint;
    bool lit;
    GameHandler GH;

    // Start is called before the first frame update
    void Start()
    {
        lit = false;
        worldPoint = transform.position;
        GH = GameObject.Find("GameHud").GetComponent<GameHandler>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(!lit && col.tag == "Player")
        {
            fire.SetActive(true);
            GH.setRespawnPoint(new Vector3(worldPoint.x, worldPoint.y, 0));
            lit = true;

            // Play audio clip
            int randomIndex = UnityEngine.Random.Range(0, m_fireLoopSounds.Length);
            m_fireAudioSource.clip = m_fireLoopSounds[randomIndex];
            m_fireAudioSource.loop = true;
            m_fireAudioSource.Play();
        }
    }

}
