using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer m_mainMixer;
    public void SetLevel(float pSliderValue)
    {
        // We use Log10 because we are using decibels.
        // This takes our value, and sets it to a value between -80 and 0, on a Logarithmic scale. This is how the mixer works.
        m_mainMixer.SetFloat("MainVolume", Mathf.Log10(pSliderValue) * 20);
        GameManager.m_instance.m_volumeSliderValue = pSliderValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Fectching current volume.
        this.GetComponent<Slider>().value = GameManager.m_instance.m_volumeSliderValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
