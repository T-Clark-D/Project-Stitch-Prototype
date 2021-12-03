using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    /// <summary>
    /// Represents the TextMeshPro object that contains the High score
    /// </summary>
    public TextMeshProUGUI m_scoreText;
    /// <summary>
    /// Represents the TextMeshPro object that contains the time alive
    /// </summary>
    public TextMeshProUGUI m_timeAliveText;

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
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
