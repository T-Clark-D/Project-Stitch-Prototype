using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightTrigger : MonoBehaviour
{
    
    private lightColor m_lightCol;
    [SerializeField] private Color m_switchColor;
    [SerializeField] private bool m_ignoreColor = false;
    [SerializeField] private bool m_ignorePoint = false;
    [SerializeField] private Animator m_lightAnim;

    private bool m_firstPass = true;

    void Start()
    {
        m_lightCol = FindObjectOfType<lightColor>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Can trigger color change when the player enters the trigger box.
        if(col.tag == "Player")
        {
            if (m_switchColor != null && !m_ignoreColor) m_lightCol.SwitchColor(m_switchColor);
            if (!m_ignorePoint)
            {
                if (m_firstPass) 
                {
                    m_lightAnim.SetInteger("lightState", m_lightAnim.GetInteger("lightState") + 1);
                } 
                else
                {
                    m_lightAnim.SetInteger("lightState", m_lightAnim.GetInteger("lightState") - 1);
                }
                m_firstPass = !m_firstPass;
            }
            
        }
        
    }
}
