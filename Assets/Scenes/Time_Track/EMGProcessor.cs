using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class EMGProcessor : MonoBehaviour
{
    public string filePath = "Assets/EMG/raw_brainlinkpro6.csv";
    public float sampleRate = 512f;

    void Start()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다: " + filePath);
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        List<float> eeg = new List<float>();

        for (int i = 1; i < lines.Length; i++) // 헤더 제외
        {
            var split = lines[i].Split(',');
            if (split.Length > 2 && float.TryParse(split[2], out float val))
            {
                if (!float.IsNaN(val)) eeg.Add(val);
            }
        }

        if (eeg.Count == 0)
        {
            Debug.LogError("유효한 EEG 데이터가 없습니다.");
            return;
        }

        float[] eegArray = eeg.ToArray();
        float duration = eegArray.Length / sampleRate;

        // 2. Bandpass 필터 (20–100Hz)
        ButterworthFilter bandpass = new ButterworthFilter(sampleRate, 20f, 100f);
        float[] emg = bandpass.Apply(eegArray);

        // 3. Envelope 계산 (0.25초 이동 평균)
        int windowSize = Mathf.RoundToInt(0.25f * sampleRate);
        float[] envelope = MovingAverage(AbsArray(emg), windowSize);

        // 4. Threshold 설정
        float mean = envelope.Average();
        float std = Std(envelope);
        float threshold = mean + 1.5f * std;

        bool[] eventMask = envelope.Select(x => x > threshold).ToArray();

        // 5. 이벤트 라벨링 및 분류
        List<int[]> rawEvents = LabelEvents(eventMask);
        List<EventInfo> classified = ClassifyEvents(rawEvents, sampleRate);

        // 5-1. 병합 및 재분류
        List<EventInfo> merged = MergeEvents(classified, 0.6f);

        // 출력
        Debug.Log("<color=yellow>감지된 이벤트:</color>");
        foreach (var evt in merged)
        {
            Debug.Log($"[{evt.Type}] {evt.Time:F2}초에서 발생 ({evt.Duration:F2}초 지속)");
        }

        int blinkCount = classified.Count(e => e.Type == "Blink");
        Debug.Log($"\n총 눈 깜빡임 횟수: {blinkCount}회");
    }

    float[] AbsArray(float[] input) => input.Select(Mathf.Abs).ToArray();

    float[] MovingAverage(float[] data, int window)
    {
        float[] result = new float[data.Length];
        float sum = 0;
        Queue<float> q = new Queue<float>();

        for (int i = 0; i < data.Length; i++)
        {
            q.Enqueue(data[i]);
            sum += data[i];

            if (q.Count > window)
                sum -= q.Dequeue();

            result[i] = sum / q.Count;
        }
        return result;
    }

    float Std(float[] data)
    {
        float mean = data.Average();
        float sumSq = data.Select(val => (val - mean) * (val - mean)).Sum();
        return Mathf.Sqrt(sumSq / data.Length);
    }

    List<int[]> LabelEvents(bool[] mask)
    {
        List<int[]> events = new List<int[]>();
        int start = -1;
        for (int i = 0; i < mask.Length; i++)
        {
            if (mask[i])
            {
                if (start == -1) start = i;
            }
            else
            {
                if (start != -1)
                {
                    events.Add(new int[] { start, i - 1 });
                    start = -1;
                }
            }
        }
        if (start != -1) events.Add(new int[] { start, mask.Length - 1 });
        return events;
    }

    List<EventInfo> ClassifyEvents(List<int[]> rawEvents, float fs)
    {
        List<EventInfo> events = new List<EventInfo>();
        foreach (var range in rawEvents)
        {
            float start = range[0] / fs;
            float duration = (range[1] - range[0] + 1) / fs;
            string type = duration < 0.5f ? "Blink" : (duration < 0.9f ? "Clench" : "Yawn");
            events.Add(new EventInfo { Time = start, Duration = duration, Type = type });
        }
        return events;
    }

    List<EventInfo> MergeEvents(List<EventInfo> events, float mergeGap)
    {
        List<EventInfo> merged = new List<EventInfo>();
        int i = 0;
        while (i < events.Count)
        {
            float t0 = events[i].Time;
            float dur = events[i].Duration;
            int j = i + 1;

            while (j < events.Count && events[j].Time - (t0 + dur) < mergeGap)
            {
                dur = events[j].Time + events[j].Duration - t0;
                j++;
            }

            string type = dur < 0.5f ? "Blink" : (dur < 1.2f ? "Clench" : "Yawn");
            merged.Add(new EventInfo { Time = t0, Duration = dur, Type = type });

            i = j;
        }
        return merged;
    }

    public class EventInfo
    {
        public float Time;
        public float Duration;
        public string Type;
    }
}
