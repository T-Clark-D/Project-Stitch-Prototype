using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HpHandler : MonoBehaviour
{
    // Start is called before the first frame update

    //TODO: Find an object to store a UI element like an image
    [SerializeField] private GameObject LifeMeter; 
    [SerializeField] private int maxLives;

    private Image[] lives;
    private int livesLeft;

    void Start()
    {
        LifeMeter = GameObject.FindGameObjectWithTag("LifeMeter");
        lives = LifeMeter.GetComponentsInChildren<Image>();
        livesLeft = maxLives;
        //intialize the UI object or get it
        for (int i = 0; i < maxLives; i++)
        {
            lives[i].enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //change the image on space spressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            loseLife(1);
        }

    }

    void loseLife(int amount)
    {
        if (lives[0].enabled == true)
        {
            for (int i = 0; i < amount; i++)
            {
                lives[livesLeft - 1].enabled = false;
                livesLeft--;
            }
        }

    }
}
//todo:
//iteration two: code the display of multiple images on the UI
//iteration three: code the creation of those images based on amount of life input
