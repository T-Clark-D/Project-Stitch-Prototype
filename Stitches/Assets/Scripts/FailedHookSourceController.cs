using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailedHookSourceController : MonoBehaviour
{
    public GameObject m_hookCollider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = m_hookCollider.transform.position;
    }
}
