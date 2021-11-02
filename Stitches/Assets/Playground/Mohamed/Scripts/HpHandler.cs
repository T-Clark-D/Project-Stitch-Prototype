using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HpHandler : MonoBehaviour
{
    // Start is called before the first frame update

    //TODO: Find an object to store a UI element like an image
    [SerializeField] private Image Life;
    [SerializeField] private int maxLives;
    void Start()
    {
        //intialize the UI object or get it
        Image[] lives = GameObject.FindGameObjectWithTag("LifeMeter").GetComponentsInChildren<Image>();
        for (int i = 0; i <= maxLives; i++)
        {
            lives[i].enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //change the image on space spressed

    }
}
//todo:
//iteration two: code the display of multiple images on the UI
//iteration three: code the creation of those images based on amount of life input
