using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class NPC : MonoBehaviour
{
    private Text dialogueText;
    string[] messageArray;
    int dialogueIndex = 0;
    bool dialogueOver = false;

    private TextWriter.SingleTextWriter singleTextWriter;

    // for talking sound
    //private AudioSource talkingAudioSource;

    void Awake()
    {
        dialogueText = transform.Find("Dialogues").Find("DialogueText").GetComponent<Text>();
        //talkingAudioSource = transform.Find("TalkingSound").GetComponent<AudioSource>();
        messageArray = new string[]
      {
            "Oh?",
            "A guest?",
            "Rare to see folks around these parts.",
            "Where are we?",
            "Who knows...",
            "I've long forgotten how I first got here.",
            "But never mind that, how about taking a seat and humor an old sticth for a bit.",
            "Years has it been since I've spoken to a soul.",
            "...",
            "Quiet type eh?",
            "Well, no matter. How about a little tale then?",
            "One of the few things this old coot can still do...",
            "A tale of a stitch in the dark...",
      };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Move to Game Scene");
        }
    }

    public void Click()
    {
        string dialogue = messageArray[dialogueIndex];

        if (singleTextWriter != null && singleTextWriter.isActive())
        {
            singleTextWriter.WriteAllAndDestroy();
        }
        else
        {
            //StartTalkingSound();
            singleTextWriter = TextWriter.AddWriter_Static(dialogueText, dialogue, 0.03f); //StopTalkingSound()
            if (dialogueIndex == messageArray.Length - 1 && !dialogueOver)
            {
                dialogueOver = true;

            }
            else if (dialogueOver)
            {
                Debug.Log("Move to Game Scene");
            }
            else
            {
                dialogueIndex++;
            }
        }
    }

   /* private void StartTalkingSound()
    {
        talkingAudioSource.Play();
    }*/

   /* private void StopTalkingSound()
    {
        talkingAudioSource.Stop();
    }*/
}
