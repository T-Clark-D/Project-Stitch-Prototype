using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyController : MonoBehaviour
{
    public PlayerController m_player;
    public float m_dustOffsetMultiplier = 0.25f;
    public bool m_dustEnabled = true;

    private Rigidbody2D m_rigidBody;
    private SpriteRenderer m_spriteRenderer;
    private bool m_spawnDust = false;
    [SerializeField] private DustController m_referenceDustSystem;
    private DustController m_currentDustSystem;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Making the body's neck face towards the player's head.
        Vector3 moveDirection = m_player.transform.position - transform.position;

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Creating dust

        if (m_spawnDust && m_dustEnabled)
        {
            m_spawnDust = false;

            m_currentDustSystem.PlayAndDestroy();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Enemy") &&
            !collision.collider.CompareTag("Needle") &&
            !collision.collider.CompareTag("Player"))
        {
            m_spawnDust = true;

            // Move the particle system to the right position
            ContactPoint2D contact = collision.GetContact(0);
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 position = contact.point + contact.normal * m_dustOffsetMultiplier;

            m_currentDustSystem = Instantiate(m_referenceDustSystem, position, rotation);
            m_currentDustSystem.gameObject.SetActive(true);
        }
    }
}
