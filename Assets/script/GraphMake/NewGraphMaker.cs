using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor.Experimental.GraphView;	//이건 뭔지 모르겠다.  언제부턴가 추가돼있었음

public class NewGraphMaker : MonoBehaviour
{
    public GameObject DotPrefab;    // 그래프 점 프리팹
    public GameObject LinePrefab;   // 그래프 선 프리팹
    public GameObject EEGpow;    // 뇌파 값 받는 스크립트
    public Transform DotGroup;      // 생성한 그래프 점을 자식으로 가질 부모 오브젝트
    public Transform LineGroup;     // 생성한 선을 자식으로 둘 부모
    public RectTransform GraphArea; // 그래프 영역의RectTransform

    private float graph_Width;
    private float graph_Height;                            // 그래프 영역의 길이, 높이 저장할 변수
    private const int ArraySize = 100;                     // 가로 점 갯수
    private float[] EEGValue = new float[ArraySize];     // 심전도값 저장할 배열 ArraySize개 만들기 > 값 0으로 통일

    private GameObject[] dots = new GameObject[ArraySize];              // 생성된 점들을 저장할 배열
    private GameObject[] lines = new GameObject[ArraySize]; // 생성된 선들을 저장할 배열

    private Vector2 prevDotPos = Vector2.zero;          // 이전 점과 현재 점을 이어야하므로 이전 점의 위치를 저장한다.
                                                        // Vector2 형식 (0,0) 으로 초기화

    private int currentIndex = 0;
    private float timePassed = 0f;          // 경과한 시간을 추적
    private const float updateTime = 0.1f;  // 그래프를 업데이트할 시간 간격(0.1초)
    //const : 1. 반드시 선언과 동시에 값 초기화 2. 한번 값이 할당되면 이후 변경 불가능 3.자동으로 static

    private const int MaxValue = 1000;    //그래프 최댓값

    void Start() //시작할때 실행하는 함수
    {
        graph_Width = GraphArea.rect.width;
        graph_Height = GraphArea.rect.height;
    }

    void Update() //프레임마다 실행하는 함수
    {
        timePassed += Time.deltaTime; // Unity 엔진에서 프레임 간 경과 시간을 나타내는 변수
                                      // 각 프레임 사이의 시간 간격

        if (timePassed >= updateTime)
        {
            timePassed = 0f;

            float EEG = EEGpow.GetComponent<InletOutlet2>().EEGpow;

            EEGValue[currentIndex] = EEG;
            DrawGraph(EEGValue[currentIndex], currentIndex);

            if (currentIndex + 1 == ArraySize)
            {     // 가로 다채우면 모든 값 0으로 바꾸기
                for (int i = 0; i < ArraySize; i++) EEGValue[i] = MaxValue / 2;
                for (int i = 0; i < ArraySize; i++) DrawGraph(EEGValue[i], i);
            }

            currentIndex = (currentIndex + 1) % ArraySize; // 다음 인덱스로 이동
        }
    }

    private void DrawGraph(float value, int index)
    {
        float startPosition = -graph_Width / 2;     // 그래프 영역의 길이 / 2 에 -를 붙이면 시작위치
        float maxYPosition = graph_Height * 2 / 3;    // 그래프 영역의 높이 * 2/3 => 점을 찍을 최대 높이

        if (dots[index] == null)
        {    // 이전에 생성된 점이 없는 경우 점 오브젝트 생성 및 부모 설정
            dots[index] = Instantiate(DotPrefab, DotGroup, true);
        }


        // -- 1. 점 찍기

        //각 컴포넌트 가져오기
        RectTransform dotRT = dots[index].GetComponent<RectTransform>();
        Image dotImage = dots[index].GetComponent<Image>();

        float yPosOffset = value / (float)MaxValue; // 그래프의 최댓값으로 나눠, 값을 0과 1 사이로 정규화

        dotRT.anchoredPosition = new Vector2(startPosition + (graph_Width / (ArraySize - 1) * index), maxYPosition * yPosOffset - graph_Height / 3);
        // 가로는 startPosition부터 일정간격으로 찍히게 하였고
        // 세로는 값에 따라 점이 찍힐 최대 높이에서 비율에 맞게 찍히게 하였다.


        // -- 2. 선 찍기

        if (index != 0) // 최초 점을 찍을 땐 연결할 선이 없으므로 넘긴다.
        {
            if (lines[index] == null)
            {    // 이전에 생성된 선이 없는 경우 선 오브젝트 생성 및 부모 설정
                lines[index] = Instantiate(LinePrefab, LineGroup, true);
            }

            RectTransform lineRT = lines[index].GetComponent<RectTransform>();
            Image lineImage = lines[index].GetComponent<Image>();

            float lineWidth = Vector2.Distance(prevDotPos, dotRT.anchoredPosition); // 선의 길이 = 이전 점과 현재 점 사이의 거리
            float xPos = (prevDotPos.x + dotRT.anchoredPosition.x) / 2;             // 선의 x축 위치 (두 점의 중간)
            float yPos = (prevDotPos.y + dotRT.anchoredPosition.y) / 2;             // 선의 y축 위치 (두 점의 중간)

            Vector2 dir = (dotRT.anchoredPosition - prevDotPos).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            lineRT.anchoredPosition = new Vector2(xPos, yPos);
            lineRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lineWidth);
            lineRT.localRotation = Quaternion.Euler(0f, 0f, angle);
            // 두 점 사이의 각도를 tan를 이용하여 구한다.
            // atan를 이용해 라디안 값을 구하고 Rad2Deg를 이용해 라디안을 각도로 변환해준다.
            // 이 부분 이해를 완벽하게는 못 해서 언젠가 다시 보기

        }

        prevDotPos = dotRT.anchoredPosition;    // 이전 점 좌표 업데이트
    }
}