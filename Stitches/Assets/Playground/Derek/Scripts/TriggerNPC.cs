using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TriggerNPC : MonoBehaviour
{
    [SerializeField] float dialogueTimer;
    [SerializeField] float dialogueSpeed;
    [SerializeField] string[] messageArray;
    private Text dialogueText;
    bool dialogueStart = false;
    bool dialogueOver = false;
    int dialogueIndex = 0;
    float mTime;

    private TextWriter.SingleTextWriter singleTextWriter;

    // for talking sound
    private AudioSource talkingAudioSource;

    void Awake()
    {
        dialogueText = transform.Find("DialogueCanvas").Find("Text").GetComponent<Text>();
        talkingAudioSource = transform.Find("DialogueCanvas").Find("TalkingSound").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (dialogueStart && !dialogueOver)
        {
            mTime += Time.deltaTime;
            if (mTime >= dialogueTimer && dialogueIndex < messageArray.Length)
            {
                if (dialogueIndex == messageArray.Length - 1)
                {
                    dialogueText.text = " ";
                    dialogueOver = true;
                }
                else
                {
                    dialogueIndex++;
                    string dialogue = messageArray[dialogueIndex];

                    if (singleTextWriter != null && singleTextWriter.isActive())
                    {
                        singleTextWriter.WriteAllAndDestroy();
                    }
                    else
                    {
                        StartTalkingSound();
                        singleTextWriter = TextWriter.AddWriter_Static(dialogueText, dialogue, dialogueSpeed, StopTalkingSound); //StopTalkingSound()
                        mTime = 0.0f;
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && !dialogueStart)
        {
            dialogueStart = true;
            string dialogue = messageArray[dialogueIndex];

            if (singleTextWriter != null && singleTextWriter.isActive())
            {
                singleTextWriter.WriteAllAndDestroy();
            }
            else
            {
                StartTalkingSound();
                singleTextWriter = TextWriter.AddWriter_Static(dialogueText, dialogue, 0.03f, StopTalkingSound);
            }
        }
    }

    private void StartTalkingSound()
    {
        talkingAudioSource.Play();
    }

    private void StopTalkingSound()
    {
        talkingAudioSource.Stop();
    }

}
