using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Button : MonoBehaviour
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        print("***In ONPOINTENTER***");
        this.GetComponentInChildren<Text>().color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.GetComponentInChildren<Text>().color = Color.white;
    }
}
