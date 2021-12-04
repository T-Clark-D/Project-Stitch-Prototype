using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenu()
    {
        GameManager.m_instance.MainMenu();
    }
    public void Quit()
    {
        GameManager.m_instance.Quit();
    }
}
