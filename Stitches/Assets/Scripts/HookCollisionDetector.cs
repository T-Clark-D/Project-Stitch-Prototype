using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCollisionDetector : MonoBehaviour
{
    [SerializeField] private HookController m_hookController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_hookController.HandleCollision(collision);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
