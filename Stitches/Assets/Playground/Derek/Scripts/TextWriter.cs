//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour
{
    private static TextWriter instance;
    private List<SingleTextWriter> singleTextWriterList;

    void Awake()
    {
        instance = this;
        singleTextWriterList = new List<SingleTextWriter>();
    }

    public SingleTextWriter AddWriter(Text posText, string textToWrite, float timePerChar) //Action onComplete
    {
        SingleTextWriter singleTextWriter = new SingleTextWriter(posText, textToWrite, timePerChar); 
        singleTextWriterList.Add(singleTextWriter);
        return singleTextWriter;
    }

    public static SingleTextWriter AddWriter_Static(Text posText, string textToWrite, float timePerChar ) //Action onComplete
    {
        instance.RemoveWriter(posText);
        return instance.AddWriter(posText, textToWrite, timePerChar);
    }

    private void RemoveWriter(Text uiText)
    {
        for (int i = 0; i < singleTextWriterList.Count; i++)
        {
            if(singleTextWriterList[i].getUiText() == uiText)
            {
                singleTextWriterList.RemoveAt(i);
                i--;
            }
        }

    }

    public static void RemoveWriter_Static(Text uiText)
    {
        instance.RemoveWriter(uiText);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < singleTextWriterList.Count; i++)
        {
            bool destroyInstance = singleTextWriterList[i].Update();
            if (destroyInstance)
            {
                singleTextWriterList.RemoveAt(i);
                i--;
            }
        }
    }

    public class SingleTextWriter
    {
        private Text uiText;
        private string textToWrite;
        private int charIndex;
        private float timePerChara;
        private float mTime;
        //private Action onComplete;

        public SingleTextWriter(Text posText, string textToWrite, float timePerChar) //Action onComplete
        {
            uiText = posText;
            this.textToWrite = textToWrite;
            timePerChara = timePerChar;
            charIndex = 0;
            //this.onComplete = onComplete;
        }

        public bool Update()
        {

            mTime -= Time.deltaTime;
            while (mTime <= 0f)
            {
                // display next character
                mTime += timePerChara;
                charIndex++;
                string text = textToWrite.Substring(0, charIndex);
                text += "<color=#00000000>" + textToWrite.Substring(charIndex) + "</color>";
                uiText.text = text;

                if (charIndex >= textToWrite.Length)
                {
                    // Entire string is displayed
                    //if(onComplete != null) onComplete();
                    return true;
                }
            }
            return false;
        }

        public Text getUiText()
        {
            return uiText;
        }

        public bool isActive()
        {
            return charIndex < textToWrite.Length;
        }

        public void WriteAllAndDestroy()
        {
            uiText.text = textToWrite;
            charIndex = textToWrite.Length;
            //if(onComplete != null) onComplete();
            TextWriter.RemoveWriter_Static(uiText);
        }
    }
}
