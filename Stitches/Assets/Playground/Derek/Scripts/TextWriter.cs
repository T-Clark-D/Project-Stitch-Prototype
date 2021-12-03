using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour
{
    private Text uiText;
    private string text;
    private int charIndex;
    private float timePerChara;
    private float mTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (uiText != null)
        {
            mTime += Time.deltaTime;
            while(mTime <= 0f)
            {
                // display next character
                mTime += timePerChara;
                charIndex++;
                uiText.text = text.Substring(0, charIndex);

                if(charIndex >= text.Length)
                {
                    text = null;
                    return;
                }
            }
        }
    }
    
    public void AddWriter(Text posText, string textToWrite, float timePerChar)
    {
        uiText = posText;
        text = textToWrite;
        timePerChara = timePerChar;
        charIndex = 0;
    }
}
