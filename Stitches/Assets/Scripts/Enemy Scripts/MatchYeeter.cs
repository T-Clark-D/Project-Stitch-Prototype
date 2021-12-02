using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchYeeter : Enemy
{
    // Start is called before the first frame update
    public GameObject match;
    public PlayerController player;

    public float interval;

    public AudioClip[] m_throwingMatchSounds;
    public AudioSource m_throwingMatchAudioSource;
    public AudioClip[] m_lightingMatchSounds;
    public AudioSource m_lightingMatchAudioSource;
    public AudioClip[] m_idleFireSounds;
    public AudioSource m_idleFireAudioSource;
    public float m_throwSoundDelay = 0.5f;

    private float lastYeet;
    private int throwForce = 20;
    private int mod;

    bool lookingLeft;

    protected override void Start()
    {
        base.Start();
        player = FindObjectOfType<PlayerController>();
        interval = 1.0f;
        lookingLeft = true;

        int randomIndex = UnityEngine.Random.Range(0, m_idleFireSounds.Length);

        m_idleFireAudioSource.clip = m_idleFireSounds[randomIndex];
        m_idleFireAudioSource.loop = true;
        m_idleFireAudioSource.Play();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (lookingLeft)
        {
            mod = -1;
        }
        else
        {
            mod = 1;
        }

        if (Time.timeSinceLevelLoad - lastYeet >= interval)
        {
            GameObject m_match = Instantiate(match,this.transform.position ,Quaternion.identity);
            m_match.GetComponent<Rigidbody2D>().AddForce(new Vector2(mod*1,1)* throwForce, ForceMode2D.Impulse);
            m_match.GetComponent<Rigidbody2D>().AddForceAtPosition(Vector2.right, new Vector2(-2.77f, -3.51f), ForceMode2D.Impulse);
            lastYeet = Time.timeSinceLevelLoad;
            m_match.GetComponent<MatchController>().PassPlayerObject(player);
            StartCoroutine(DestroyMatch(m_match));

            // Play the lighting clip.
            int randomIndex = UnityEngine.Random.Range(0, m_lightingMatchSounds.Length);
            m_lightingMatchAudioSource.PlayOneShot(m_lightingMatchSounds[randomIndex]);

            // Play the Throwing clip with a slight delay
            randomIndex = UnityEngine.Random.Range(0, m_throwingMatchSounds.Length);

            m_throwingMatchAudioSource.clip = m_throwingMatchSounds[randomIndex];
            m_throwingMatchAudioSource.PlayDelayed(m_throwSoundDelay);
        }
        FlipDirection();
       
    }

    IEnumerator DestroyMatch(GameObject m_match)
    {
        yield return new WaitForSeconds(5);
        Destroy(m_match);
    }
    private void FlipDirection()
    {
        if (player.transform.position.x - this.transform.position.x > 0 && lookingLeft)
        {
              m_SR.flipX = true;
            lookingLeft = false;
            Transform[] fires = GetComponentsInChildren<Transform>();
            foreach (Transform f in fires)
            {
                if(!f.gameObject.Equals(this.gameObject))
                f.localPosition = new Vector2(-f.localPosition.x, f.localPosition.y);
            }
        }
        else if (player.transform.position.x - this.transform.position.x < 0 && !lookingLeft)
        {
             m_SR.flipX = false;
            lookingLeft = true;
            Transform[] fires = GetComponentsInChildren<Transform>();
            foreach (Transform f in fires)
            {
                if (!f.gameObject.Equals(this.gameObject))
                    f.localPosition = new Vector2(-f.localPosition.x, f.localPosition.y);
            }
        }
    }
}
