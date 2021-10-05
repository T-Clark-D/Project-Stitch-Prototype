using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappler : MonoBehaviour
{
    public Camera mainCamera;
    public LineRenderer lineRend;
    public DistanceJoint2D distJoint;
    public Rigidbody2D rigidBod;

    bool pullUp = false;
    bool tethered = false;
    Vector2 forceDirection = new Vector2(0, 0);
    Vector2 mousePos = new Vector2(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        distJoint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            distJoint.enabled = false;
            lineRend.enabled = false;
            tethered = true;

            mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
            print(forceDirection);

            lineRend.SetPosition(0, mousePos);
            lineRend.SetPosition(1, transform.position);

            distJoint.connectedAnchor = mousePos;
            distJoint.enabled = true;
            lineRend.enabled = true;

        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && tethered)
        {
            pullUp = true;
        }
        if(pullUp)
        {
            rigidBod.velocity = new Vector2(0,0);
            forceDirection = (mousePos - (Vector2)transform.position).normalized;
            distJoint.enabled = false;
            lineRend.enabled = false;
            rigidBod.AddForce(forceDirection * 1000);
            pullUp = false;
        }
        if (distJoint.enabled)
        {
            lineRend.SetPosition(1, transform.position);
        }
    }
}
