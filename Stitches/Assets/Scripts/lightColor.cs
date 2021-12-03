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
    public bool switcharoo = false;

    float startTime;

    void Start()
    {
        myLight = GetComponent<Light>();
        tick = 0;
    }



    void Update()
    {
        
        // Change color when player enters trigger event.
        if (changeColors)
        {
            SwitchColor();
            tick += Time.deltaTime * colorSpeed;
            myLight.color = Color.Lerp(startColor, endColor, tick);

        }
    }

    // Switch the start and end colors on exit event.
    public void SwitchColor()
    {
        // Check if the light has completely changed color before making the switch and that player has exited the trigger.
        if(myLight.color == endColor && switcharoo)
        {
            tick = 0;
            Color temp = startColor;
            startColor = endColor;
            endColor = temp;
            switcharoo = false;
        }
    }

}
