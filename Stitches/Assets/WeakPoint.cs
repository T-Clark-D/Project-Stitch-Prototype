using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    [SerializeField] private Armordillo m_armordillo;

    private void Start()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "HookCollider")
        {
            m_armordillo.WeakPointHit();
        }
    }
}
