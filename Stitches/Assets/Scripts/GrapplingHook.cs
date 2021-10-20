using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based on Don Haul's game dev unity tutorial on grappling hooks: https://youtu.be/sHhzWlrTgJo & https://youtu.be/DTFgQIs5iMY

public class GrapplingHook : MonoBehaviour
{
    public LineRenderer line;
    DistanceJoint2D joint;
    Vector3 mousPos;
    RaycastHit2D hit;
    public float maxRopeLength = 10f;       // Max rope length
    public LayerMask mask;                  // Put the player on a different layer using a mask as to not grapple yourself and control which objects you can grapple.

    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;                      // Turn off distance joint on start. Only active when you point to an object that can be grappled to.
        line.enabled = false;                       // Turned off when grappling hook is not used.
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {

            mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);      // Get mouse position based on world coordinates
            mousPos.z = 0;

            hit = Physics2D.Raycast(transform.position, mousPos - transform.position, maxRopeLength, mask);

            // Only grapple objects that have a Rogidbody2D component 
            if(hit.collider != null && hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                joint.enabled = true;
                joint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();
                joint.connectedAnchor = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);      // Line will grapple to the surface the raycast hits rather than anchor to the center of the object.
                // joint.distance = Vector2.Distance(transform.position, hit.point);        // Uncomment if you want the player to remain in the same position when they grapple something but this would require implementing actual controls to the player like jumping and moving.
                                                                                            

                line.enabled = true;
                line.SetPosition(0, transform.position);        // Set the initial point of the line.
                line.SetPosition(1, hit.point);                 // Set the end point of the line.
            }
        }


        // Update the initial point of the line whenever you click mouse 0.
        if(Input.GetMouseButton(0))
        {
            line.SetPosition(0, transform.position);
        }


        // Stop grappling when you release mouse 0
        if(Input.GetMouseButtonUp(0))
        {
            joint.enabled = false;
            line.enabled = false;
        }

    }
}
