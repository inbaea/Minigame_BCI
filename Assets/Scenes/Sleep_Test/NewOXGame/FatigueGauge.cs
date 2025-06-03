using UnityEngine;
using UnityEngine.UI;

public class FatigueGauge : MonoBehaviour
{
    public GameObject dataReceiver; // EEG 데이터 받아오는 컴포넌트
    public Image fatigueFillImage;       // 게이지 UI (Image type with Fill Amount)
    
    // 값 튐 방지를 위한 부드러운 변화
    [Range(0f, 1f)] public float smoothFactor = 0.1f;
    private float currentFatigueNormalized = 0f;

    void Update()
    {
        if (dataReceiver == null || fatigueFillImage == null)
            return;

        // 1. 데이터 수집
        int attention = dataReceiver.GetComponent<EEGProfileUI>().targetReceiver.attention;
        int lowAlpha = dataReceiver.GetComponent<EEGProfileUI>().targetReceiver.lowAlpha;
        int highAlpha = dataReceiver.GetComponent<EEGProfileUI>().targetReceiver.highAlpha;

        // 2. 피로도 계산
        float rawFatigue = (lowAlpha + highAlpha) / Mathf.Max(1f, attention + 1f);  // 방어 코드 포함

        // 3. 정규화: 평균적으로 5000 ~ 10000 사이 → 0~1로 보정
        float normalizedFatigue = Mathf.Clamp01(rawFatigue / 10000f);  // 튜닝 가능

        // 4. 부드럽게 보간
        currentFatigueNormalized = Mathf.Lerp(currentFatigueNormalized, normalizedFatigue, smoothFactor);

        // 5. UI 반영
        fatigueFillImage.fillAmount = currentFatigueNormalized;
        fatigueFillImage.color = Color.Lerp(Color.blue, Color.red, currentFatigueNormalized);
    }
}
