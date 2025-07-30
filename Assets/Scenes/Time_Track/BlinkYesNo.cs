using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkYesNo : MonoBehaviour
{
    public Transform textParent;        // TextMeshProUGUI들이 있는 부모 오브젝트
    public GameObject yesPrefab;
    public GameObject noPrefab;
    public bool blink = false;

    private List<Transform> textSlots = new List<Transform>();
    private int currentIndex = 0;

    void Start()
    {
        // 자식 중 TextMeshProUGUI가 붙어 있는 오브젝트만 수집
        foreach (TextMeshProUGUI tmp in textParent.GetComponentsInChildren<TextMeshProUGUI>())
        {
            textSlots.Add(tmp.transform);
        }

        Debug.Log("Text Slot Count: " + textSlots.Count);

        StartCoroutine(SpawnBlinkMarkers());
    }

    IEnumerator SpawnBlinkMarkers()
    {
        while (currentIndex < textSlots.Count)
        {
            GameObject prefabToSpawn = blink ? yesPrefab : noPrefab;
            Transform parentText = textSlots[currentIndex];

            // 생성 → 부모 지정 → localPosition = Vector3.zero
            GameObject instance = Instantiate(prefabToSpawn, parentText);
            instance.transform.localPosition = Vector3.zero;

            currentIndex++;
            yield return new WaitForSeconds(1f);
        }
    }
}
