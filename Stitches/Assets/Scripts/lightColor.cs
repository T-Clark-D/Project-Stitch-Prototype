using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://stackoverflow.com/questions/56735003/unity-script-how-to-gradually-change-a-color-of-an-object-using-color-lerp
public class lightColor : MonoBehaviour
{
    private Light myLight;

    // Color Variables
    public bool changeColors;
    public float colorSpeed = 0.5f;
    public Color startColor;
    public Color endColor;
    private float tick;

    float startTime;

    void Start()
    {
        myLight = GetComponent<Light>();
        tick = 0;
    }



    void Update()
    { 

        if (changeColors)
        {

            tick += Time.deltaTime * colorSpeed;
            myLight.color = Color.Lerp(startColor, endColor, tick);

        }
    }

}
