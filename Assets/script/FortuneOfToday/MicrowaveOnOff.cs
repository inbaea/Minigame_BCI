using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MicrowaveOnOff : MonoBehaviour
{
    public Sprite front;
    public Sprite back;
    public Image thisImage;
    public bool isFlipped = true;
    bool CanFlip = true;

    private void Start()
    {
        thisImage = GetComponent<Image>();
        back = GameObject.Find("Microwave_on").GetComponent<Image>().sprite;
        front = GameObject.Find("Microwave_off").GetComponent<Image>().sprite;
        StartCoroutine(FlipCard());
        StartCoroutine(StopFlip());
    }
    IEnumerator FlipCard()
    { // 카드 뒤집는 메서드
        Vector3 originalScale = transform.localScale; // 맨 처음 가지고 있던 x,y,z 값을 불러와 저장한다.
        Vector3 targetScale = new Vector3(0f, originalScale.y, originalScale.z); // x값을 0으로 변경하고자 하는 스케일 값
        if (CanFlip)
        {
            transform.DOScale(targetScale, 0.5f).OnComplete(() =>
            {
                isFlipped = !isFlipped; // 카드의 상태를 변경

                if (isFlipped)
                {
                    thisImage.sprite = front; // 현재 카드 생태를 앞면의 동물카드로 변경
                }
                else
                {
                    thisImage.sprite = back; // 현재 카드 생태를 뒷면의 배경카드로 변경
                }

                transform.DOScale(originalScale, 0.5f);
            }); // 현재의 스케일 값을 0.2초동안 targetScale로 값을 변경한다.
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(FlipCard());
    }

    IEnumerator StopFlip()
    {
        yield return new WaitForSeconds(10.5f);
        CanFlip = false;
    }
}
