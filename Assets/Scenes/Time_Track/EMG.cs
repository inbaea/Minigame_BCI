using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public class EMG : MonoBehaviour
{
    public float fs = 512f;
    private float timer = 0f;
    private List<float> eegBuffer = new List<float>();
    private Queue<float> realTimeBuffer = new Queue<float>();

    private float threshold = 0f;
    private bool thresholdSet = false;

    private float envWindow = 0.25f; // 0.25초
    private float minBlinkDuration = 0.05f; // 최소 50ms
    private float maxBlinkDuration = 0.5f;  // 최대 500ms

    private float currentEventDuration = 0f;
    private bool isAboveThreshold = false;

    public GameObject inlet;
    public GameObject blinkYN;

    void Update()
    {
        // EEG 값을 inlet 등에서 실시간으로 받는다
        float newSample = inlet.GetComponent<InletOutlet2>().EEGpow;  // 직접 연결된 코드로 대체해야 함

        realTimeBuffer.Enqueue(Mathf.Abs(newSample)); // 절댓값 처리

        // 0.25초 평균 이동평균을 위한 버퍼 유지
        int maxBufferSize = Mathf.RoundToInt(envWindow * fs);
        if (realTimeBuffer.Count > maxBufferSize)
            realTimeBuffer.Dequeue();

        float envelope = 0f;
        foreach (float val in realTimeBuffer)
            envelope += val;
        envelope /= realTimeBuffer.Count;

        if (!thresholdSet)
        {
            // 시작 후 10초 동안 데이터 누적
            timer += Time.deltaTime;
            eegBuffer.Add(envelope);

            if (timer >= 10f)
            {
                float mean = 0f;
                foreach (var val in eegBuffer) mean += val;
                mean /= eegBuffer.Count;

                float std = 0f;
                foreach (var val in eegBuffer) std += Mathf.Pow(val - mean, 2);
                std = Mathf.Sqrt(std / eegBuffer.Count);

                threshold = mean + 1.5f * std;
                thresholdSet = true;

                Debug.Log($"[임계값 설정 완료] 평균: {mean:F4}, 표준편차: {std:F4}, 임계값: {threshold:F4}");
            }
        }
        else
        {
            // 실시간 Blink 감지
            if (envelope > threshold)
            {
                if (!isAboveThreshold)
                {
                    isAboveThreshold = true;
                    currentEventDuration = 0f;
                    blinkYN.GetComponent<BlinkYesNo>().blink = true;
                }

                currentEventDuration += Time.deltaTime;
            }
            else
            {
                if (isAboveThreshold)
                {
                    // 이벤트가 종료되었을 때 조건 확인
                    if (currentEventDuration >= minBlinkDuration && currentEventDuration <= maxBlinkDuration)
                    {
                        Debug.Log($"[Blink 감지] {Time.time:F2}초에 감지됨 (지속: {currentEventDuration:F2}초)");
                    }
                    blinkYN.GetComponent<BlinkYesNo>().blink = false;

                    isAboveThreshold = false;
                    currentEventDuration = 0f;
                }
            }
        }
    }
}
