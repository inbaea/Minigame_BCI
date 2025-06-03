using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_Open : MonoBehaviour
{
    public RectTransform buttonPos;

    public void Open_Popup()
    {
        // 부모 RectTransform을 안전하게 참조
        RectTransform parent = buttonPos.parent as RectTransform;

        if (parent == null)
        {
            Debug.LogWarning("부모 RectTransform이 존재하지 않습니다.");
            return;
        }

        float parentX = parent.anchoredPosition.x;
        float parentY = parent.anchoredPosition.y;
        buttonPos.anchoredPosition = new Vector2(-parentX, -parentY);
    }
}
