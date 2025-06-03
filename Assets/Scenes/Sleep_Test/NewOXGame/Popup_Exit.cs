using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_Exit : MonoBehaviour
{
    public RectTransform buttonPos;

    public void Exit_Popup()
    {
        buttonPos.anchoredPosition = new Vector2(-2500, -250);
    }
}