using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    public float m_grappleSpeed = 1;
    public float m_grapplingStartHeightOffset = 1;
    public float m_hookColliderSize = 0.05f;
    public bool m_tethered = false;
    public bool m_pullingUp = false;
    public bool m_isHookedToAnEnemy = false;
    public AudioClip[] m_platformHitSounds;
    public AudioClip[] m_ungrappleAbleHitSounds;
    public AudioClip[] m_needleThrowSounds;
    public AudioClip[] m_pullUpSounds;
    public AudioSource m_needleHitAudioSource;
    public AudioSource m_throwAudioSource;
    public AudioSource m_pullUpAudioSource;

    private bool m_grapplingHookOut = false;
    private LineRenderer m_grapplingHookRenderer;
    private Vector3 m_clickPosition;
    private Vector3 m_currentPosition;
    private Vector3 m_directionUnitVector;
    private CircleCollider2D m_hookCollider;
    private Vector3 m_enemyHitLocationOffset;

    [SerializeField] private PlayerController m_player;
    private Rigidbody2D m_playerRigidBody;
    private DistanceJoint2D m_distJoint;
    private Enemy m_enemy;
    private Enemy m_lastEnemyHooked;
    private SpriteRenderer m_needleSpriteRenderer;
    private Collider2D m_lastCollision;
    private Vector3 m_platformHitLocationOffset;
    private GameObject m_lastPlatformHit;

    // Start is called before the first frame update
    void Start()
    {
        m_grapplingHookRenderer = GetComponent<LineRenderer>();
        m_hookCollider = GetComponentInChildren<CircleCollider2D>();
        m_distJoint = m_player.GetComponent<DistanceJoint2D>();
        m_playerRigidBody = m_player.GetComponent<Rigidbody2D>();
        m_needleSpriteRenderer = m_hookCollider.gameObject.GetComponentInChildren<SpriteRenderer>();

        m_hookCollider.enabled = false;
        m_distJoint.enabled = false;
        m_grapplingHookOut = false;
        m_tethered = false;
        m_needleSpriteRenderer.enabled = false;

        m_currentPosition = new Vector3(0, 0, 0);
        m_clickPosition = new Vector3(0, 0, 0);
        m_directionUnitVector = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        HandleGrapplingHook();
        RefreshHookPosition();
    }

    public void LaunchHook(Vector3 pMouseClickPosition)
    {
        // Handling launch

        if (m_grapplingHookOut)
        {
            RetractHook();
        }
        else
        {
            // Launching grappling hook

            // Changing Z value of mouse pos to a positive value, to avoid the object showing behind the near clip plane of the camera.
            pMouseClickPosition.z = 1;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pMouseClickPosition);

            // Setting worldpos z to 1 so it doesnt draw behind the background.
            worldPosition.z = 0;

            // Calculating direction vector
            Vector3 direction = worldPosition - (m_player.transform.position + new Vector3(0, m_grapplingStartHeightOffset, 0));
            m_directionUnitVector = direction / direction.magnitude;

            m_clickPosition = worldPosition;
            m_currentPosition = m_player.transform.position + new Vector3(0, m_grapplingStartHeightOffset, 0);

            m_grapplingHookOut = true;

            Vector3 position = m_player.transform.position + new Vector3(0, m_grapplingStartHeightOffset, 0);
            m_hookCollider.gameObject.transform.position = new Vector3(position.x, position.y, 0);
            m_hookCollider.radius = m_hookColliderSize;

            m_hookCollider.enabled = true;
            m_pullingUp = false;
            StopPullUpSounds();

            m_needleSpriteRenderer.enabled = true;


            // Angling the needle
            var rotation = Quaternion.FromToRotation(transform.up, m_directionUnitVector).eulerAngles;
            rotation.x = 0f;
            rotation.y = 0f;
            m_needleSpriteRenderer.gameObject.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);

            // Play audio clip
            int randomIndex = UnityEngine.Random.Range(0, m_needleThrowSounds.Length);
            m_throwAudioSource.PlayOneShot(m_needleThrowSounds[randomIndex]);
        }
    }

    public void RetractHook()
    {
        m_hookCollider.enabled = false;

        // Reset hook display
        m_grapplingHookRenderer.positionCount = 0;
        m_currentPosition = new Vector3(0, 0, 0);
        m_clickPosition = new Vector3(0, 0, 0);

        // Retracting hook
        m_grapplingHookOut = false;

        Vector3 position = m_player.transform.position + new Vector3(0, m_grapplingStartHeightOffset, 0);
        m_hookCollider.gameObject.transform.position = new Vector3(position.x, position.y, 0);

        m_tethered = false;
        m_distJoint.enabled = false;

        m_pullingUp = false;

        m_player.ResetGravity();

        if(m_enemy != null)
        {
            // If we unhook the enemy, we flag it as unhooked.
            m_enemy.ToggleHook();
            m_enemy = null;
        }

        m_enemyHitLocationOffset = new Vector3();
        m_isHookedToAnEnemy = false;

        m_needleSpriteRenderer.enabled = false;

        // Unfreeze Collider
        m_hookCollider.attachedRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        //Physics2D.IgnoreCollision(m_hookCollider, m_lastCollision, false);

        // Stop pull up sounds
        StopPullUpSounds();
    }

    public void PullUp()
    {
        if(m_tethered)
        {
            // We have to be tethered to pull up.
            Vector3 direction = m_clickPosition - (m_player.transform.position + new Vector3(0, m_grapplingStartHeightOffset, 0));
            direction = direction / direction.magnitude;

            m_playerRigidBody.velocity = new Vector2(0, 0); // Resetting velocity

            m_pullingUp = true;
        }
    }

    public void HandleCollision(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            m_lastCollision = collision.collider;
            //Debug.Log("Hit!");
            m_tethered = true;

            m_distJoint.enabled = true;
            m_distJoint.connectedAnchor = m_hookCollider.transform.position;

            m_distJoint.distance = GetHookDirection(false).magnitude;

            // Freeze position.
            m_hookCollider.attachedRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

            // Prevent pushing
            //Physics2D.IgnoreCollision(m_hookCollider, m_lastCollision, true);

            // Play audio clip
            int randomIndex = UnityEngine.Random.Range(0, m_platformHitSounds.Length);
            m_needleHitAudioSource.PlayOneShot(m_platformHitSounds[randomIndex]);

            Vector2 point = collision.GetContact(0).point;
            Vector3 collisionPoint = new Vector3(point.x, point.y, 0);
            m_platformHitLocationOffset = collisionPoint - collision.gameObject.transform.position;
            m_lastPlatformHit = collision.gameObject;
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("Hit an enemy!");
            m_tethered = true;

            m_distJoint.enabled = true;
            m_distJoint.connectedAnchor = m_hookCollider.transform.position;

            m_distJoint.distance = GetHookDirection(false).magnitude;

            m_enemy = collision.gameObject.GetComponent<Enemy>();
            m_lastEnemyHooked = m_enemy;

            Vector3 enemyHitLocation = m_hookCollider.transform.position;
            m_enemyHitLocationOffset = enemyHitLocation - collision.gameObject.transform.position;

            m_enemy.ToggleHook();

            m_isHookedToAnEnemy = true;
        }
        else if (collision.gameObject.CompareTag("Ungrapplable") || collision.gameObject.CompareTag("Boss"))
        {
            Debug.Log("Hit an ungrapplable target!");
            RetractHook();
            FindObjectOfType<PlayerController>().m_ungrapplableBuffer = true;

            // Play audio clip.
            if (collision.gameObject.CompareTag("Ungrapplable"))
            {
                int randomIndex = UnityEngine.Random.Range(0, m_ungrappleAbleHitSounds.Length);
                m_needleHitAudioSource.PlayOneShot(m_ungrappleAbleHitSounds[randomIndex]);
            }
            
        }
    }
    void HandleGrapplingHook()
    {        
        // Handling display and collision
        if (m_grapplingHookOut && !m_tethered)
        {
            // Making a line
            m_grapplingHookRenderer.positionCount = 2;

            RefreshHookPosition();

            // Increasing the length of the whole line
            m_currentPosition = m_currentPosition + (new Vector3(m_directionUnitVector.x, m_directionUnitVector.y, 0) * m_grappleSpeed * Time.fixedDeltaTime);

            // Handle Hook Collider
            Vector3 position = new Vector3(m_currentPosition.x, m_currentPosition.y, 0);
            m_hookCollider.attachedRigidbody.MovePosition(position);

            Vector3 secondPointPosition = position;
            m_grapplingHookRenderer.SetPosition(1, secondPointPosition);

            // Angling the needle
            Vector3 firstPointPosition = m_player.transform.position + new Vector3(0, m_grapplingStartHeightOffset, 0);
            var rotation = Quaternion.FromToRotation(transform.up, m_currentPosition - firstPointPosition).eulerAngles;
            rotation.x = 0f;
            rotation.y = 0f;
            m_needleSpriteRenderer.gameObject.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);

            //m_needleSpriteRenderer.transform.position = secondPointPosition;
        }
    }

    void RefreshHookPosition()
    {
        if(m_grapplingHookOut)
        {
            // We also add the vector 0, offset, 0 to make the grappling hook start above our character.

            Vector3 firstPointPosition = m_player.transform.position + new Vector3(0, m_grapplingStartHeightOffset, 0);
            firstPointPosition.z = 0;
            m_grapplingHookRenderer.SetPosition(0, firstPointPosition);

            if(m_enemy != null)
            {
                Vector3 enemyPos = m_enemy.transform.position + m_enemyHitLocationOffset;

                m_hookCollider.transform.position = enemyPos;

                Vector3 secondPointPosition = enemyPos;
                secondPointPosition.z = 0;
                m_grapplingHookRenderer.SetPosition(1, secondPointPosition);

                m_distJoint.connectedAnchor = enemyPos;
            }
            else if(m_tethered)
            {
                if( m_lastPlatformHit != null && 
                    m_lastPlatformHit.CompareTag("Platform") && 
                    m_lastPlatformHit.GetComponent<Rigidbody2D>() != null)
                {
                    Vector3 newRotatedOffset = m_lastPlatformHit.transform.rotation * m_platformHitLocationOffset;
                    Vector3 newPosition = m_lastPlatformHit.transform.position + newRotatedOffset;

                    // We have hit a moving platform. Updating hook position constantly.
                    m_hookCollider.transform.position = newPosition;
                    m_distJoint.connectedAnchor = newPosition;
                }

                Vector3 secondPointPosition = m_hookCollider.transform.position;
                secondPointPosition.z = 0;
                m_grapplingHookRenderer.SetPosition(1, secondPointPosition);
            }
        }
    }

    /// <summary>
    /// Returns the vector from the player to the hook collider.
    /// </summary>
    /// <param name="pWithOffset">If true, will return the vector with the offset on the player. If false, will return vector from the center of the player to the hook.</param>
    /// <returns></returns>
    public Vector3 GetHookDirection(bool pWithOffset = false)
    {
        Vector3 direction;
        if (pWithOffset)
        {
            if(m_enemy != null)
            {
                direction = (m_enemy.transform.position + m_enemyHitLocationOffset) - (m_player.transform.position + new Vector3(0, m_grapplingStartHeightOffset, 0));
            }
            else
            {
                direction = m_hookCollider.gameObject.transform.position - (m_player.transform.position + new Vector3(0, m_grapplingStartHeightOffset, 0));
            }
            
        }
        else
        {
            // If there is an enemy, we always apply offset.
            if (m_enemy != null)
            {
                direction =  (m_enemy.transform.position + m_enemyHitLocationOffset) - (m_player.transform.position);
            }
            else
            {
                direction = m_hookCollider.gameObject.transform.position - (m_player.transform.position);
            }
        }

        return direction;
    }

    public void StartVulnerabilityTimer()
    {
        if(m_lastEnemyHooked != null)
        {
            m_lastEnemyHooked.StartVulnerabilityTimer();
        }
    }

    public void StartPullUpSounds()
    {
        // Start playing audio clip
        m_pullUpAudioSource.loop = true;

        int randomIndex = UnityEngine.Random.Range(0, m_pullUpSounds.Length);

        m_pullUpAudioSource.clip = m_pullUpSounds[randomIndex];
        m_pullUpAudioSource.Play();
    }

    public void StopPullUpSounds()
    {
        m_pullUpAudioSource.Stop();
    }
}
