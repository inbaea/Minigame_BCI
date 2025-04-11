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
    { // ī�� ������ �޼���
        Vector3 originalScale = transform.localScale; // �� ó�� ������ �ִ� x,y,z ���� �ҷ��� �����Ѵ�.
        Vector3 targetScale = new Vector3(0f, originalScale.y, originalScale.z); // x���� 0���� �����ϰ��� �ϴ� ������ ��
        if (CanFlip)
        {
            transform.DOScale(targetScale, 0.5f).OnComplete(() =>
            {
                isFlipped = !isFlipped; // ī���� ���¸� ����

                if (isFlipped)
                {
                    thisImage.sprite = front; // ���� ī�� ���¸� �ո��� ����ī��� ����
                }
                else
                {
                    thisImage.sprite = back; // ���� ī�� ���¸� �޸��� ���ī��� ����
                }

                transform.DOScale(originalScale, 0.5f);
            }); // ������ ������ ���� 0.2�ʵ��� targetScale�� ���� �����Ѵ�.
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
