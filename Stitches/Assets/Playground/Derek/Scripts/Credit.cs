using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credit : MonoBehaviour
{
    public void Click()
    {
        Debug.Log("Load Main Menu scene");
        SceneManager.LoadScene(0);
    }
}
