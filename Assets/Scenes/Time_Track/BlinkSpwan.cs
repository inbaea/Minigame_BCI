using UnityEngine;

public class BlinkSpwan : MonoBehaviour
{
    public GameObject prefab;     // 생성할 프리팹
    public int count = 10;        // 총 생성 수
    public Vector2 startPos = Vector2.zero; // 시작 위치

    private float xMinSpacing = 10f;
    private float xMaxSpacing = 25f;
    private float yVariation = 20f;

    void Start()
    {
        SpawnPrefabs();
    }

    void SpawnPrefabs()
    {
        Vector2 currentPos = startPos;

        for (int i = 0; i < count; i++)
        {
            // X 간격 랜덤
            float xOffset = Random.Range(xMinSpacing, xMaxSpacing);
            currentPos.x += xOffset;

            // Y 위치 변화
            float yOffset = Random.Range(-yVariation, yVariation);
            Vector2 spawnPos = new Vector2(currentPos.x, startPos.y + yOffset);

            // 프리팹 생성 및 부모 설정
            GameObject newObj = Instantiate(prefab, spawnPos, Quaternion.identity);
            newObj.transform.SetParent(this.transform); // 부모를 현재 GameObject로 설정
        }
    }
}
