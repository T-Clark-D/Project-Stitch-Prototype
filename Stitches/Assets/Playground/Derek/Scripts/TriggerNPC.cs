using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerNPC : MonoBehaviour
{
    private Text dialogueText;
    string[] messageArray;
    int dialogueIndex = 0;
    bool dialogueOver = false;

    // for talking sound
    private AudioSource talkingAudioSource;

    void Awake()
    {
        dialogueText = transform.Find("Dialogues").Find("DialogueText").GetComponent<Text>();
        talkingAudioSource = transform.Find("TalkingSound").GetComponent<AudioSource>();
        messageArray = new string[]
      {
            "You",
            "You're finally awake.",
      };
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            Debug.Log("Dialogue starts");
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
