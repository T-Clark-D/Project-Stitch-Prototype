using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBubble : MonoBehaviour
{
    private TextMeshPro textMeshPro;
    private AudioSource talkingAudioSource;

    void Awake()
    {
        //backgroundSpriteRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    void Start()
    {
        Setup("Testing. You, you're finally awake.");
    }

    private void Setup(string text)
    {
        textMeshPro.SetText(text);
        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues(false);

        /*  Vector2 padding = new Vector2(4f, 2f);
            backgroundSpriteRenderer.size = textSize + padding;
            Vector2 offset = new Vector2(-2f, 0f);
            backgroundSpriteRenderer.transfom.localPosition = new Vectpr3(backgroundSpriteRenderer.size.x / 2f, 0f) + offset;*/
        //StartTalkingSound();
        //TextWriter.AddWriter_Static(textMeshPro, text, 0.05f, StopTalkingSound);
    }

}
