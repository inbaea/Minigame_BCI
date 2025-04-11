using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDetector : MonoBehaviour
{
    public LayerMask uiLayer; // UI 콜라이더가 있는 레이어만 감지

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭
        {
            // 마우스 위치를 월드 좌표로 변환
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePosition = new Vector2(worldMousePos.x, worldMousePos.y);

            // 콜라이더만 감지
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0.1f, uiLayer);

            if (hit.collider != null)
            {
                Debug.Log("클릭된 UI 콜라이더: " + hit.collider.gameObject.name);
            }
            else
            {
                Debug.Log("UI 콜라이더 없음");
            }
        }
    }
}
