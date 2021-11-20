using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueController : MonoBehaviour
{
    public float m_tongueSpeed = 1;
    public float m_tongueStartHeightOffset = 1;
    public bool m_tongueOut = false;
    public float m_tongueMaxLength = 2f;

    private LineRenderer m_tongueRenderer;
    private Vector3 m_currentPosition;
    private Vector3 m_directionUnitVector;
    private Collider2D m_tipCollider;
    private Rigidbody2D m_tipRigidBody;

    [SerializeField] private WallCrawler m_crawler;

    private float m_tongueHitTimeElapsed = 0f;
    private bool m_tongueHasHit = false;

    // Start is called before the first frame update
    void Start()
    {
        m_tongueRenderer = GetComponent<LineRenderer>();
        m_tipCollider = GetComponent<BoxCollider2D>();
        m_tipRigidBody = GetComponent<Rigidbody2D>();

        m_tipCollider.enabled = false;
        m_tipRigidBody.isKinematic = true;
        m_tongueOut = false;

        m_currentPosition = new Vector3(0, 0, 0);
        m_directionUnitVector = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_tongueHasHit)
            m_tongueHitTimeElapsed += Time.deltaTime;

        if (m_tongueHasHit && m_tongueHitTimeElapsed >= m_crawler.m_attackPauseTimer)
        {
            RetractTongue();
        }
    }

    private void FixedUpdate()
    {
        HandleTongueTip();
        RefreshTonguePosition();
    }

    public void LaunchTongue(Vector3 pTargetPosition)
    {
        if (m_tongueOut)
            return;

        // Launching tongue
        // Setting target pos z to 0 so it doesnt draw behind the background.
        pTargetPosition.z = 0;

        // Calculating direction vector
        Vector3 direction = pTargetPosition - (m_crawler.transform.position + new Vector3(0, m_tongueStartHeightOffset, 0));
        m_directionUnitVector = direction / direction.magnitude;

        m_currentPosition = m_crawler.transform.position + new Vector3(0, m_tongueStartHeightOffset, 0);

        m_tongueOut = true;

        Vector3 position = m_crawler.transform.position + new Vector3(0, m_tongueStartHeightOffset, 0);
        m_tipCollider.gameObject.transform.position = new Vector3(position.x, position.y, 0);

        m_tipCollider.enabled = true;
        m_tipRigidBody.isKinematic = false;

        // Setting up the line renderer
        m_tongueRenderer.positionCount = 2;
        Vector3 firstPointPosition = m_crawler.transform.position + new Vector3(0, m_tongueStartHeightOffset, 0);
        firstPointPosition.z = 0;
        m_tongueRenderer.SetPosition(0, firstPointPosition);

        Vector3 secondPointPosition = m_currentPosition;
        secondPointPosition.z = 0;
        m_tongueRenderer.SetPosition(1, secondPointPosition);
    }

    public void RetractTongue()
    {
        m_tipCollider.enabled = false;
        m_tipRigidBody.isKinematic = true;

        // Reset tongue display
        m_tongueRenderer.positionCount = 0;
        m_currentPosition = new Vector3(0, 0, 0);

        // Retracting tongue
        m_tongueOut = false;
        m_tongueHasHit = false;
        m_tongueHitTimeElapsed = 0f;

        Vector3 position = m_crawler.transform.position + new Vector3(0, m_tongueStartHeightOffset, 0);
        m_tipCollider.gameObject.transform.position = new Vector3(position.x, position.y, 0);

        //Debug.Log("Finished Retracting tongue.");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Hit the player with the tongue!");
            m_tongueHasHit = true;
        }
        else
        {
           // Debug.Log("Hit something else than the player!");
            m_tongueHasHit = true;
        }

        if(!m_tongueHasHit)
            RetractTongue();
    }

    void HandleTongueTip()
    {
        // Handling display and collision
        if (m_tongueOut && !m_tongueHasHit)
        {
            // Making a line
            m_tongueRenderer.positionCount = 2;

            RefreshTonguePosition();

            // Increasing the length of the whole line
            m_currentPosition = m_currentPosition + (new Vector3(m_directionUnitVector.x, m_directionUnitVector.y, 0) * m_tongueSpeed * Time.fixedDeltaTime);

            Vector3 secondPointPosition = m_currentPosition;
            secondPointPosition.z = 0;
            m_tongueRenderer.SetPosition(1, secondPointPosition);

            // Handle tongue Collider
            Vector3 position = new Vector3(secondPointPosition.x, secondPointPosition.y, 0);
            m_tipCollider.attachedRigidbody.MovePosition(position);
        }

        Vector3 crawlerPos = m_crawler.transform.position + new Vector3(0, m_tongueStartHeightOffset, 0);

        if (m_tongueOut && (m_currentPosition - crawlerPos).magnitude >= m_tongueMaxLength)
        {
            m_tongueHasHit = true;
        }
    }

    void RefreshTonguePosition()
    {
        if (m_tongueOut)
        {
            // We also add the vector 0, offset, 0 to make the tongue tip start above our character.

            Vector3 firstPointPosition = m_crawler.transform.position + new Vector3(0, m_tongueStartHeightOffset, 0);
            firstPointPosition.z = 0;
            m_tongueRenderer.SetPosition(0, firstPointPosition);
        }
    }

    /// <summary>
    /// Returns the vector from the crawler to the tip collider.
    /// </summary>
    /// <param name="pWithOffset">If true, will return the vector with the offset on the crawler. If false, will return vector from the center of the crawler to the tongue tip.</param>
    /// <returns></returns>
    public Vector3 GetTongueDirection(bool pWithOffset = false)
    {
        Vector3 direction;
        if (pWithOffset)
        {
            direction = m_tipCollider.gameObject.transform.position - (m_crawler.transform.position + new Vector3(0, m_tongueStartHeightOffset, 0));
        }
        else
        {
            direction = m_tipCollider.gameObject.transform.position - (m_crawler.transform.position);
        }

        return direction;
    }
}
