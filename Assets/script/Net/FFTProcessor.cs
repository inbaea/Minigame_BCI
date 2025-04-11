using System;
using UnityEngine;
using System.Numerics;  // 복소수 연산을 위한 System.Numerics 사용

public class FFTProcessor : MonoBehaviour
{
    private const int sampleSize = 256;  // FFT 샘플 크기
    private int bufferIndex = 0;

    // 정규화를 위한 최소/최대값 (필요시 업데이트 가능)
    private float minEEG = float.MaxValue;
    private float maxEEG = float.MinValue;

    public float thetaPower;
    public float alphaPower;
    public float betaPower;
    public float gammaPower;
    private Complex[] signal;

    [SerializeField] public float[] eegBuffer = new float[sampleSize];
    public float[] processedSignal;

    void Update()
    {
        // EEG 데이터가 충분히 쌓이면 FFT 변환 실행
        if (bufferIndex >= sampleSize)
        {
            NormalizeData();  // 데이터 정규화 수행
            PerformFFT(eegBuffer);
            bufferIndex = 0;
        }
    }

    public void ProcessRawEEG(float rawEEG)
    {
        // 최소/최대값 업데이트
        if (rawEEG < minEEG) minEEG = rawEEG;
        if (rawEEG > maxEEG) maxEEG = rawEEG;

        // EEG 데이터를 버퍼에 저장
        eegBuffer[bufferIndex] = rawEEG;
        bufferIndex++;
    }

    void NormalizeData()
    {
        // 최소-최대 정규화 (0~1 범위로 변환)
        for (int i = 0; i < eegBuffer.Length; i++)
        {
            eegBuffer[i] = (eegBuffer[i] - minEEG) / (maxEEG - minEEG);
        }
    }

    void PerformFFT(float[] real)
    {
        int n = real.Length;
        signal = new Complex[n];
        processedSignal = new float[n]; // Inspector에서 볼 수 있도록 Vector2 배열 사용

        for (int i = 0; i < n; i++)
        {
            signal[i] = new Complex(real[i], 0);
        }

        FFT(signal); // FFT 수행

        for (int i = 0; i < n; i++)
        {
            processedSignal[i] = (float)signal[i].Magnitude;
        }

        // 주파수 대역별 파워 분석
        thetaPower = GetPower(signal, 4, 8);
        alphaPower = GetPower(signal, 8, 13);
        betaPower = GetPower(signal, 14, 30);
        gammaPower = GetPower(signal, 30, 50);
        Debug.Log($"Theta: {thetaPower}, Alpha: {alphaPower}, Beta: {betaPower}, Gamma: {gammaPower}");
    }

    void FFT(Complex[] buffer)
    {
        int n = buffer.Length;
        if (n <= 1) return;

        // 배열을 반으로 나누기
        Complex[] even = new Complex[n / 2];
        Complex[] odd = new Complex[n / 2];

        for (int i = 0; i < n / 2; i++)
        {
            even[i] = buffer[i * 2];
            odd[i] = buffer[i * 2 + 1];
        }

        // 재귀 호출
        FFT(even);
        FFT(odd);

        for (int k = 0; k < n / 2; k++)
        {
            Complex t = Complex.Exp(new Complex(0, -2 * Mathf.PI * k / n)) * odd[k];
            buffer[k] = even[k] + t;
            buffer[k + n / 2] = even[k] - t;
        }
    }

    float GetPower(Complex[] signal, int minFreq, int maxFreq)
    {
        float power = 0;
        int n = signal.Length;

        // 특정 주파수 대역 파워 계산
        for (int i = minFreq; i <= maxFreq && i < n / 2; i++)
        {
            power += (float)(signal[i].Magnitude * signal[i].Magnitude);
        }
        return power;
    }
}
