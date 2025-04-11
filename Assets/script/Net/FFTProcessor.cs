using System;
using UnityEngine;
using System.Numerics;  // ���Ҽ� ������ ���� System.Numerics ���

public class FFTProcessor : MonoBehaviour
{
    private const int sampleSize = 256;  // FFT ���� ũ��
    private int bufferIndex = 0;

    // ����ȭ�� ���� �ּ�/�ִ밪 (�ʿ�� ������Ʈ ����)
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
        // EEG �����Ͱ� ����� ���̸� FFT ��ȯ ����
        if (bufferIndex >= sampleSize)
        {
            NormalizeData();  // ������ ����ȭ ����
            PerformFFT(eegBuffer);
            bufferIndex = 0;
        }
    }

    public void ProcessRawEEG(float rawEEG)
    {
        // �ּ�/�ִ밪 ������Ʈ
        if (rawEEG < minEEG) minEEG = rawEEG;
        if (rawEEG > maxEEG) maxEEG = rawEEG;

        // EEG �����͸� ���ۿ� ����
        eegBuffer[bufferIndex] = rawEEG;
        bufferIndex++;
    }

    void NormalizeData()
    {
        // �ּ�-�ִ� ����ȭ (0~1 ������ ��ȯ)
        for (int i = 0; i < eegBuffer.Length; i++)
        {
            eegBuffer[i] = (eegBuffer[i] - minEEG) / (maxEEG - minEEG);
        }
    }

    void PerformFFT(float[] real)
    {
        int n = real.Length;
        signal = new Complex[n];
        processedSignal = new float[n]; // Inspector���� �� �� �ֵ��� Vector2 �迭 ���

        for (int i = 0; i < n; i++)
        {
            signal[i] = new Complex(real[i], 0);
        }

        FFT(signal); // FFT ����

        for (int i = 0; i < n; i++)
        {
            processedSignal[i] = (float)signal[i].Magnitude;
        }

        // ���ļ� �뿪�� �Ŀ� �м�
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

        // �迭�� ������ ������
        Complex[] even = new Complex[n / 2];
        Complex[] odd = new Complex[n / 2];

        for (int i = 0; i < n / 2; i++)
        {
            even[i] = buffer[i * 2];
            odd[i] = buffer[i * 2 + 1];
        }

        // ��� ȣ��
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

        // Ư�� ���ļ� �뿪 �Ŀ� ���
        for (int i = minFreq; i <= maxFreq && i < n / 2; i++)
        {
            power += (float)(signal[i].Magnitude * signal[i].Magnitude);
        }
        return power;
    }
}
