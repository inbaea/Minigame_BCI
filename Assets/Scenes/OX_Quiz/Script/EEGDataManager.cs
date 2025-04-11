using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class EEGDataManager : MonoBehaviour
{
    public List<float> floatValues = new List<float>();
    public GameObject EEGData;
    public int maxPoints = 200;
    public float xSpacing = 10f;
    public float graphHeight = 500f;
    public float yMaxValue = 1000f;

    public GameObject pointPrefab;   // �߰�
    public GameObject textPrefab;    // �߰�
    public Transform pointContainer; // �߰�

    private LineRenderer lineRenderer;
    private float timer = 0f;
    public float interval = 0.5f;

    public Vector2 startOffset = new Vector2(-2450, -500);

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;

            AddFloatValue(EEGData.GetComponent<InletOutlet2>().EEGpow);
        }
    }

    public void AddFloatValue(float value)
    {
        floatValues.Add(value);

        if (floatValues.Count > maxPoints)
        {
            floatValues.RemoveAt(0);
        }

        DrawGraph();
    }

    void DrawGraph()
    {
        lineRenderer.positionCount = floatValues.Count;

        for (int i = 0; i < floatValues.Count; i++)
        {
            float x = i * xSpacing + startOffset.x;
            float y = (floatValues[i] / yMaxValue) * graphHeight + startOffset.y;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    public void RecordEEG()
    {
        // 1. �� ����
        AddFloatValue(EEGData.GetComponent<InletOutlet2>().EEGpow);

        // 2. ���� ���� ���
        CreatePointWithLabel(EEGData.GetComponent<InletOutlet2>().EEGpow);
    }

    void CreatePointWithLabel(float value)
    {
        if (pointPrefab == null || textPrefab == null || pointContainer == null)
        {
            Debug.LogWarning("PointPrefab, TextPrefab, PointContainer�� �������� �ʾҽ��ϴ�.");
            return;
        }

        if (floatValues.Count == 0) return;

        // ���� ������ ����Ʈ ��ġ ���ϱ�
        int lastIndex = floatValues.Count - 1;
        Vector3 pointPosition = lineRenderer.GetPosition(lastIndex);

        // 1. �� ����
        GameObject point = Instantiate(pointPrefab, pointContainer);
        RectTransform pointRect = point.GetComponent<RectTransform>();
        pointRect.anchoredPosition = new Vector2(pointPosition.x, pointPosition.y);

        // 2. �ؽ�Ʈ ����
        GameObject label = Instantiate(textPrefab, pointContainer);
        RectTransform labelRect = label.GetComponent<RectTransform>();
        labelRect.anchoredPosition = new Vector2(pointPosition.x, pointPosition.y + 30f); // �� ���� ���� ǥ��

        TMP_Text tmp = label.GetComponent<TMP_Text>();
        tmp.text = value.ToString("F1"); // �Ҽ��� 1�ڸ�
    }
}
