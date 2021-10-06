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
    private Vector3 m_directionUnitVector;
    private BoxCollider2D m_hookCollider;

    [SerializeField] private GameObject m_player;

    // Start is called before the first frame update
    void Start()
    {
        m_grapplingHookRenderer = GetComponent<LineRenderer>();
        m_hookCollider = GetComponentInChildren<BoxCollider2D>();
        m_hookCollider.enabled = false;

        m_currentPosition = new Vector3(0, 0, 0);
        m_clickPosition = new Vector3(0, 0, 0);
        m_directionUnitVector = new Vector3(0, 0, 0);
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
            m_currentPosition = new Vector3(0, 0, 0);
            m_clickPosition = new Vector3(0, 0, 0);

            // Retracting hook
            m_grapplingHookOut = false;

            Vector3 position = m_player.transform.position + new Vector3(0, m_grapplingStartHeightOffset, 0);
            m_hookCollider.gameObject.transform.position = new Vector3(position.x, position.y, 0);
        }
        else
        {
            // Launching grappling hook

            // Changing Z value of mouse pos to a positive value, to avoid the object showing behind the near clip plane of the camera.
            pMouseClickPosition.z = 1;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pMouseClickPosition);

            // Setting worldpos z to 1 so it doesnt draw behind the background.
            worldPosition.z = 1;

            // Calculating direction vector
            Vector3 direction = worldPosition - (m_player.transform.position + new Vector3(0, m_grapplingStartHeightOffset, 0));
            m_directionUnitVector = direction / direction.magnitude;

            m_clickPosition = worldPosition;
            m_currentPosition = m_player.transform.position + new Vector3(0, m_grapplingStartHeightOffset, 0);

            m_grapplingHookOut = true;

            Vector3 position = m_player.transform.position + new Vector3(0, m_grapplingStartHeightOffset, 0);
            m_hookCollider.gameObject.transform.position = new Vector3(position.x, position.y, 0);
            m_hookCollider.size = m_hookColliderSize;

            m_hookCollider.enabled = true;
        }
    }

    public void HandleCollision(Collision2D collision)
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
            // Making a line
            m_grapplingHookRenderer.positionCount = 2;

            // We make the z position be 1 to make the points have a z position of 1, making them show up on our screen in front of the background.
            // We also add the vector 0, offset, 0 to make the grappling hook start above our character.
            Vector3 firstPointPosition = m_player.transform.position + new Vector3(0, m_grapplingStartHeightOffset, 0);
            firstPointPosition.z = 1;
            m_grapplingHookRenderer.SetPosition(0,  firstPointPosition);

            // Increasing the length of the whole line
            m_currentPosition = m_currentPosition + (new Vector3(m_directionUnitVector.x, m_directionUnitVector.y, 0) * m_grappleSpeed * Time.deltaTime);

            Vector3 secondPointPosition = m_currentPosition;
            secondPointPosition.z = 1;
            m_grapplingHookRenderer.SetPosition(1, secondPointPosition);

            // Handle Hook Collision
            Vector3 position = new Vector3(secondPointPosition.x, secondPointPosition.y, 0);
            m_hookCollider.transform.position = position;
        }
    }
}
