using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// https://youtu.be/Oadq-IrOazg
public class LevelChanger : MonoBehaviour
{
    public Animator anim;
    public bool changeLevel;
    private int levelToLoad;
    void Update()
    {
        if(changeLevel)
        {
            FadeToLevel(1);
        }
    }

    public void FadeToLevel(int index)
    {
        levelToLoad = index;
        anim.SetTrigger("FadeOut");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            changeLevel = true;
        }
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }

}
