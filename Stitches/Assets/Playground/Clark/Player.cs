using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 screenPos;
    private float direction = 1;
    [SerializeField]
    private Camera cam;

    //private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            screenPos = cam.ScreenToWorldPoint(Input.mousePosition);
            /*
            if (gameObject.transform.position.x - screenPos.x > 0)
            {
                direction = -1;
            }
            else direction = 1;*/
            print(screenPos);
            gameObject.GetComponent<Rigidbody2D>().AddForce(((Vector2)screenPos-(Vector2)gameObject.transform.position).normalized*5);
        }
       
    }
}
