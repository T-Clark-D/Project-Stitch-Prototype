using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject m_grapplingHookObject;
    private HookController m_grapplingHookController;

    // Start is called before the first frame update
    void Start()
    {
        m_grapplingHookController = m_grapplingHookObject.GetComponent<HookController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if(Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x - 4 * Time.deltaTime, transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x + 4 * Time.deltaTime, transform.position.y, transform.position.z);
        }
        if(Input.GetMouseButtonDown(0))
        {
            m_grapplingHookController.LaunchHook(Input.mousePosition);
        }
        if(Input.GetMouseButtonDown(1))
        {
            m_grapplingHookController.PullUp();
        }
    }

    
}
