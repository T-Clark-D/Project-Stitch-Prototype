using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Buttons : MonoBehaviour
{
    [SerializeField] private Transform m_PlayerTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(m_PlayerTransform, Vector3.up);
    }
}
