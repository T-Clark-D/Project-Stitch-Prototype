using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Static, global instance of the Game Manager.
    /// </summary>
    public static GameManager m_instance;
    /// <summary>
    /// Represents whether or not the player has lost.
    /// </summary>
    public bool m_gameOver = false;
    /// <summary>
    /// True if the game is currently on pause.
    /// </summary>
    public bool m_isPaused = false;
    /// <summary>
    /// Represents the pause menu
    /// </summary>
    public PauseController m_pausePanel;

    /// <summary>
    /// Global volume level, so that it stays consistent across menus.
    /// </summary>
    public float m_volumeSliderValue = 1f;

    /// <summary>
    /// Reference to the hud manager.
    /// </summary>
    //private HUDManager m_hudManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        // Checking for instance
        if (m_instance == null)
        {
            // This is the first game manager created. We initialize it.
            // Setting the global instance.
            m_instance = this;

            ReFetch();
        }
        else if (m_instance != this)
        {
            // This is not the current game manager.
            // We changed scene. We need to do a refetch on the manager.
            m_instance.ReFetch();

            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Reinitializes the game manager with components it has to find in the current scene.
    /// Essentially, refetches all cached references.
    /// </summary>
    public void ReFetch()
    {
        // Fetches the hud manager again.
        //m_hudManager = FindObjectOfType<HUDManager>();

        //m_hudManager.ResetHUD();

        m_pausePanel = FindObjectOfType<PauseController>(true);
    }

    /// <summary>
    /// Resets every value in the game manager. Starts fresh.
    /// </summary>
    public void Reset()
    {
        // Resetting all variables.
        m_isPaused = false;
        Time.timeScale = 1f;

        // Unpause the game.
        if (m_pausePanel != null)
            m_pausePanel.gameObject.SetActive(false);

        // Reset the hud
        //if (m_hudManager != null)
        //    m_hudManager.ResetHUD();
    }

    /// <summary>
    /// Signals the game's end.
    /// </summary>
    public void GameOver()
    {
        m_gameOver = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameOver");
    }

    /// <summary>
    /// Returns to main menu.
    /// </summary>
    public void MainMenu()
    {
        Reset();
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        // Check what to do depending on if the menu is opened already or not.
        if (m_isPaused)
        {
            // Unpause
            Time.timeScale = 1f;
            m_isPaused = false;

            // Hiding UI
            if (m_pausePanel != null)
            {
                m_pausePanel.gameObject.SetActive(false);
            }
        }
        else
        {
            // Pause the game
            Time.timeScale = 0f;
            m_isPaused = true;

            // Showing UI
            if (m_pausePanel != null)
            {
                m_pausePanel.gameObject.SetActive(true);
            }
        }
    }
}
