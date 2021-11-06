using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuPanel : MonoBehaviour
{

    public void quit()
    {
        print("In here");
        Application.Quit();
    }

    public void play()
    {
        print("In here");
        SceneManager.LoadScene("Main Scene");
    }

    public void controls()
    {
        print("In here");
        SceneManager.LoadScene("ControlsUI");
    }
}
