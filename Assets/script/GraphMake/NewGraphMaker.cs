using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor.Experimental.GraphView;	//�̰� ���� �𸣰ڴ�.  �������ϰ� �߰����־���

public class NewGraphMaker : MonoBehaviour
{
    public GameObject DotPrefab;    // �׷��� �� ������
    public GameObject LinePrefab;   // �׷��� �� ������
    public GameObject EEGpow;    // ���� �� �޴� ��ũ��Ʈ
    public Transform DotGroup;      // ������ �׷��� ���� �ڽ����� ���� �θ� ������Ʈ
    public Transform LineGroup;     // ������ ���� �ڽ����� �� �θ�
    public RectTransform GraphArea; // �׷��� ������RectTransform

    private float graph_Width;
    private float graph_Height;                            // �׷��� ������ ����, ���� ������ ����
    private const int ArraySize = 100;                     // ���� �� ����
    private float[] EEGValue = new float[ArraySize];     // �������� ������ �迭 ArraySize�� ����� > �� 0���� ����

    private GameObject[] dots = new GameObject[ArraySize];              // ������ ������ ������ �迭
    private GameObject[] lines = new GameObject[ArraySize]; // ������ ������ ������ �迭

    private Vector2 prevDotPos = Vector2.zero;          // ���� ���� ���� ���� �̾���ϹǷ� ���� ���� ��ġ�� �����Ѵ�.
                                                        // Vector2 ���� (0,0) ���� �ʱ�ȭ

    private int currentIndex = 0;
    private float timePassed = 0f;          // ����� �ð��� ����
    private const float updateTime = 0.1f;  // �׷����� ������Ʈ�� �ð� ����(0.1��)
    //const : 1. �ݵ�� ����� ���ÿ� �� �ʱ�ȭ 2. �ѹ� ���� �Ҵ�Ǹ� ���� ���� �Ұ��� 3.�ڵ����� static

    private const int MaxValue = 1000;    //�׷��� �ִ�

    void Start() //�����Ҷ� �����ϴ� �Լ�
    {
        graph_Width = GraphArea.rect.width;
        graph_Height = GraphArea.rect.height;
    }

    void Update() //�����Ӹ��� �����ϴ� �Լ�
    {
        timePassed += Time.deltaTime; // Unity �������� ������ �� ��� �ð��� ��Ÿ���� ����
                                      // �� ������ ������ �ð� ����

        if (timePassed >= updateTime)
        {
            timePassed = 0f;

            float EEG = EEGpow.GetComponent<InletOutlet2>().EEGpow;

            EEGValue[currentIndex] = EEG;
            DrawGraph(EEGValue[currentIndex], currentIndex);

            if (currentIndex + 1 == ArraySize)
            {     // ���� ��ä��� ��� �� 0���� �ٲٱ�
                for (int i = 0; i < ArraySize; i++) EEGValue[i] = MaxValue / 2;
                for (int i = 0; i < ArraySize; i++) DrawGraph(EEGValue[i], i);
            }

            currentIndex = (currentIndex + 1) % ArraySize; // ���� �ε����� �̵�
        }
    }

    private void DrawGraph(float value, int index)
    {
        float startPosition = -graph_Width / 2;     // �׷��� ������ ���� / 2 �� -�� ���̸� ������ġ
        float maxYPosition = graph_Height * 2 / 3;    // �׷��� ������ ���� * 2/3 => ���� ���� �ִ� ����

        if (dots[index] == null)
        {    // ������ ������ ���� ���� ��� �� ������Ʈ ���� �� �θ� ����
            dots[index] = Instantiate(DotPrefab, DotGroup, true);
        }


        // -- 1. �� ���

        //�� ������Ʈ ��������
        RectTransform dotRT = dots[index].GetComponent<RectTransform>();
        Image dotImage = dots[index].GetComponent<Image>();

        float yPosOffset = value / (float)MaxValue; // �׷����� �ִ����� ����, ���� 0�� 1 ���̷� ����ȭ

        dotRT.anchoredPosition = new Vector2(startPosition + (graph_Width / (ArraySize - 1) * index), maxYPosition * yPosOffset - graph_Height / 3);
        // ���δ� startPosition���� ������������ ������ �Ͽ���
        // ���δ� ���� ���� ���� ���� �ִ� ���̿��� ������ �°� ������ �Ͽ���.


        // -- 2. �� ���

        if (index != 0) // ���� ���� ���� �� ������ ���� �����Ƿ� �ѱ��.
        {
            if (lines[index] == null)
            {    // ������ ������ ���� ���� ��� �� ������Ʈ ���� �� �θ� ����
                lines[index] = Instantiate(LinePrefab, LineGroup, true);
            }

            RectTransform lineRT = lines[index].GetComponent<RectTransform>();
            Image lineImage = lines[index].GetComponent<Image>();

            float lineWidth = Vector2.Distance(prevDotPos, dotRT.anchoredPosition); // ���� ���� = ���� ���� ���� �� ������ �Ÿ�
            float xPos = (prevDotPos.x + dotRT.anchoredPosition.x) / 2;             // ���� x�� ��ġ (�� ���� �߰�)
            float yPos = (prevDotPos.y + dotRT.anchoredPosition.y) / 2;             // ���� y�� ��ġ (�� ���� �߰�)

            Vector2 dir = (dotRT.anchoredPosition - prevDotPos).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            lineRT.anchoredPosition = new Vector2(xPos, yPos);
            lineRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lineWidth);
            lineRT.localRotation = Quaternion.Euler(0f, 0f, angle);
            // �� �� ������ ������ tan�� �̿��Ͽ� ���Ѵ�.
            // atan�� �̿��� ���� ���� ���ϰ� Rad2Deg�� �̿��� ������ ������ ��ȯ���ش�.
            // �� �κ� ���ظ� �Ϻ��ϰԴ� �� �ؼ� ������ �ٽ� ����

        }

        prevDotPos = dotRT.anchoredPosition;    // ���� �� ��ǥ ������Ʈ
    }
}