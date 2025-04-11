using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollowMouse : MonoBehaviour
{
    private RectTransform rectTransform;
    public Vector2 offset = new Vector2(65f, -65f); // 마우스보다 살짝 오른쪽 아래
    public List<Sprite> spriteList; // 변경할 스프라이트 리스트
    public GameObject EEGPow;
    public GameObject AlphaEEG;
    public string MouseColor = "Red";
    public int Clicked = 0;

    bool CanChange = true;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Cursor.visible = false; // 기본 마우스 커서 숨기기
    }

    void Update()
    {
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            Input.mousePosition,
            null,
            out mousePosition);

        rectTransform.anchoredPosition = mousePosition + offset;

        if (Clicked == 45)
        {
            EEGPow.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        }

        if (AlphaEEG.GetComponent<InletOutlet2>().EEGpow > 1000)
        {
            if (MouseColor == "Red")
            {
                if (CanChange)
                {
                    CanChange = false;
                    MouseColor = "Orange";
                    ChangeColor("Orange");
                    StartCoroutine("CanChanged");
                }    
            }
                
            if (MouseColor == "Orange")
            {
                if (CanChange)
                {
                    CanChange = false;
                    MouseColor = "Yellow";
                    ChangeColor("Yellow");
                    StartCoroutine("CanChanged");
                }
            }
                
            if (MouseColor == "Yellow")
            {
                if (CanChange)
                {
                    CanChange = false;
                    MouseColor = "Green";
                    ChangeColor("Green");
                    StartCoroutine("CanChanged");
                }
            }
            
            if (MouseColor == "Green")
            {
                if (CanChange)
                {
                    CanChange = false;
                    MouseColor = "Sky";
                    ChangeColor("Sky");
                    StartCoroutine("CanChanged");
                }
            }
                
            if (MouseColor == "Sky")
            {
                if (CanChange)
                {
                    CanChange = false;
                    MouseColor = "Blue";
                    ChangeColor("Blue");
                    StartCoroutine("CanChanged");
                }    
            }
                
            if (MouseColor == "Blue")
            {
                if (CanChange)
                {
                    CanChange = false;
                    MouseColor = "Purple";
                    ChangeColor("Purple");
                    StartCoroutine("CanChanged");
                }
            }

            if (MouseColor == "Purple")
            {
                if (CanChange)
                {
                    CanChange = false;
                    MouseColor = "Red";
                    ChangeColor("Red");
                    StartCoroutine("CanChanged");
                }
            }
        }
    }
    IEnumerator CanChanged()
    {
        yield return new WaitForSeconds(2f);
        CanChange = true;
    }

    public void ChangeColor(string colored)
    {
        if (colored == "Red")
        {
            gameObject.GetComponent<Image>().sprite = spriteList[0];
        }
        if (colored == "Orange")
        {
            gameObject.GetComponent<Image>().sprite = spriteList[1];
        }
        if (colored == "Yellow")
        {
            gameObject.GetComponent<Image>().sprite = spriteList[2];
        }
        if (colored == "Green")
        {
            gameObject.GetComponent<Image>().sprite = spriteList[3];
        }
        if (colored == "Sky")
        {
            gameObject.GetComponent<Image>().sprite = spriteList[4];
        }
        if (colored == "Blue")
        {
            gameObject.GetComponent<Image>().sprite = spriteList[5];
        }
        if (colored == "Purple")
        {
            gameObject.GetComponent<Image>().sprite = spriteList[6];
        }
    }
}