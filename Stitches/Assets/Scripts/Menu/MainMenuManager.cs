using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    /// <summary>
    /// Determines whether the how to play menu, in the main menu, is shown or not.
    /// </summary>
    private bool m_helpMenuShown = false;
    /// <summary>
    /// Represents the parent game object of the How To Play menu.
    /// </summary>
    [SerializeField] private GameObject m_helpMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        GameManager.m_instance.Reset();
        SceneManager.LoadScene("MainLevel");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToggleHelpMenu()
    {
        if(m_helpMenuShown)
        {
            // Close the menu
            m_helpMenuShown = false;
        }
        else
        {
            // Open the menu
            m_helpMenuShown = true;
        }

        m_helpMenu.SetActive(m_helpMenuShown);
    }
}
