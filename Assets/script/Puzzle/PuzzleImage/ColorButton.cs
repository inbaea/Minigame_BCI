using UnityEngine;

public class ColorButton : MonoBehaviour
{
    public GameObject MouseColored;
    public void ColorClick()
    {
        MouseColored.GetComponent<UIFollowMouse1>().MouseColor = gameObject.name;
        MouseColored.GetComponent<UIFollowMouse1>().ChangeColor(gameObject.name);
    }
}
