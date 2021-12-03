using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    [SerializeField] Light light;
    [SerializeField] float lightStep;
    [SerializeField] float minRange;
    [SerializeField] float maxRange;
    bool flickerUp;

    // Update is called once per frame
    void Update()
    {
        CheckFlicker();
        if(flickerUp)
        {
            light.range += lightStep * Time.deltaTime;
        }
        else if(!flickerUp)
        {
            light.range -= lightStep * Time.deltaTime;
        }
    }

    private void CheckFlicker()
    {
        if(light.range <= minRange)
        {
            flickerUp = true;
        }
        if(light.range >= maxRange)
        {
            flickerUp = false;
        }
    }

   
}
