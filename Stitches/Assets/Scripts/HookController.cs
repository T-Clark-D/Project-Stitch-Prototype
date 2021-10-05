using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    public float m_grappleSpeed = 1;
    public float m_grapplingStartHeightOffset = 1;
    public Vector3 m_hookColliderSize = new Vector3(0.5f, 0.5f, 0.5f);

    private bool m_grapplingHookOut = false;
    private LineRenderer m_grapplingHookRenderer;
    private Vector3 m_clickPosition;
    private Vector3 m_currentPosition;
    private BoxCollider2D m_hookCollider;

    // Start is called before the first frame update
    void Start()
    {
        m_grapplingHookRenderer = GetComponent<LineRenderer>();
        m_hookCollider = GetComponent<BoxCollider2D>();
        m_hookCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleGrapplingHook();
    }

    public void LaunchHook(Vector3 pMouseClickPosition)
    {
        // Handling launch

        if (m_grapplingHookOut)
        {
            m_hookCollider.enabled = false;

            // Reset hook display
            m_grapplingHookRenderer.positionCount = 0;
            m_currentPosition = transform.position;
            m_clickPosition = new Vector3(0, 0, 0);

            // Retracting hook
            m_grapplingHookOut = false;
        }
        else
        {
            // Launching grappling hook

            // Changing Z value of mouse pos to a positive value, to avoid the object showing behind the near clip plane of the camera.
            pMouseClickPosition.z = Camera.main.nearClipPlane;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pMouseClickPosition);
            // Setting worldpos z to 1 so it doesnt draw behind the background.
            worldPosition.z = 0;

            m_clickPosition = worldPosition;
            m_currentPosition = transform.position + new Vector3(0, m_grapplingStartHeightOffset, 0);

            m_grapplingHookOut = true;

            m_hookCollider.offset = m_currentPosition;
            m_hookCollider.size = m_hookColliderSize;

            m_hookCollider.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            Debug.Log("Hit!");
        }
    }

    void HandleGrapplingHook()
    {
        // Handling display and collision
        if (m_grapplingHookOut)
        {
            // Making a unit vector
            Vector3 unitPosition = m_clickPosition / m_clickPosition.magnitude;

            // Making a line
            m_grapplingHookRenderer.positionCount = 2;

            // We add the vector 0, 0, 1 to make the points have a z position of 1, making them show up on our screen in front of the background.
            // We also add the vector 0, offset, 0 to make the grappling hook start above our character.
            m_grapplingHookRenderer.SetPosition(0, transform.position + new Vector3(0, m_grapplingStartHeightOffset, 1));

            // Increasing the length of the whole line
            m_currentPosition = m_currentPosition + (unitPosition * m_grappleSpeed * Time.deltaTime);

            m_grapplingHookRenderer.SetPosition(1, m_currentPosition + new Vector3(0, 0, 1));

            // Handle Hook Collision
            m_hookCollider.offset = m_currentPosition;
        }
    }
}
