using UnityEngine;
using UnityEngine.UI;
public class ImageButton : MonoBehaviour
{
    public GameObject MouseColored;
    public void ImageClick()
    {
        if (MouseColored.GetComponent<UIFollowMouse1>().MouseColor == "Red")
        {
            MouseColored.GetComponent<UIFollowMouse1>().Clicked++;
            gameObject.GetComponent<Image>().color = Color.red;
            gameObject.GetComponent<Image>().raycastTarget = false; // 레이캐스트 타겟 비활성화
        }
        if (MouseColored.GetComponent<UIFollowMouse1>().MouseColor == "Orange")
        {
            MouseColored.GetComponent<UIFollowMouse1>().Clicked++;
            gameObject.GetComponent<Image>().color = new Color(1f, 0.5f, 0f, 1f);
            gameObject.GetComponent<Image>().raycastTarget = false; // 레이캐스트 타겟 비활성화
        }
        if (MouseColored.GetComponent<UIFollowMouse1>().MouseColor == "Yellow")
        {
            MouseColored.GetComponent<UIFollowMouse1>().Clicked++;
            gameObject.GetComponent<Image>().color = Color.yellow;
            gameObject.GetComponent<Image>().raycastTarget = false; // 레이캐스트 타겟 비활성화
        }
        if (MouseColored.GetComponent<UIFollowMouse1>().MouseColor == "Green")
        {
            MouseColored.GetComponent<UIFollowMouse1>().Clicked++;
            gameObject.GetComponent<Image>().color = Color.green;
            gameObject.GetComponent<Image>().raycastTarget = false; // 레이캐스트 타겟 비활성화
        }
        if (MouseColored.GetComponent<UIFollowMouse1>().MouseColor == "Sky")
        {
            MouseColored.GetComponent<UIFollowMouse1>().Clicked++;
            gameObject.GetComponent<Image>().color = new Color(0.53f, 0.81f, 0.92f, 1f);
            gameObject.GetComponent<Image>().raycastTarget = false; // 레이캐스트 타겟 비활성화
        }
        if (MouseColored.GetComponent<UIFollowMouse1>().MouseColor == "Blue")
        {
            MouseColored.GetComponent<UIFollowMouse1>().Clicked++;
            gameObject.GetComponent<Image>().color = Color.blue;
            gameObject.GetComponent<Image>().raycastTarget = false; // 레이캐스트 타겟 비활성화
        }
        if (MouseColored.GetComponent<UIFollowMouse1>().MouseColor == "Purple")
        {
            MouseColored.GetComponent<UIFollowMouse1>().Clicked++;
            gameObject.GetComponent<Image>().color = new Color(0.5f, 0f, 0.5f, 1f);
            gameObject.GetComponent<Image>().raycastTarget = false; // 레이캐스트 타겟 비활성화
        }
    }
}