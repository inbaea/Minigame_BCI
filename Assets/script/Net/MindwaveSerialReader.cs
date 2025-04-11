using System;
using System.IO.Ports;
using UnityEngine;
using System.Numerics;  // FFT ������ ���� System.Numerics ���

public class MindwaveSerialReader : MonoBehaviour
{
    SerialPort serialPort;
    public string portName = "COM3";  // MindWave�� COM ��Ʈ
    public int baudRate = 57600;      // ThinkGear �⺻ ��� �ӵ�
    private byte[] buffer = new byte[256];
    private int bufferIndex = 0;

    public FFTProcessor fftProcessor;  // FFT ó����
    public int MSB;
    public int LSB;
    public int rawEEG;

    void Start()
    {
        //������� Serial Port ���� �õ�
        try
        {
            serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
            serialPort.Open();
            Debug.Log("MindWave ���� ����: " + portName);
        }
        //���� ������
        catch (Exception e)
        {
            Debug.LogError("�ø��� ��Ʈ ����: " + e.Message);
        }
    }

    void Update()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                while (serialPort.BytesToRead > 0)
                {
                    int bytesRead = serialPort.Read(buffer, bufferIndex, buffer.Length - bufferIndex);
                    bufferIndex += bytesRead;

                    // ������ ó��
                    ProcessMindWaveData();
                }
            }
            //������ ������
            catch (Exception e)
            {
                Debug.LogError("������ �б� ����: " + e.Message);
            }
        }
    }

    void ProcessMindWaveData()
    {
        for (int i = 0; i < bufferIndex - 3; i++)
        {
            if (buffer[i] == 0xAA && buffer[i + 1] == 0xAA)  // ��Ŷ ��� Ȯ��
            {
                int payloadLength = buffer[i + 2];  // ���̷ε� ũ��
                if (i + 3 + payloadLength < bufferIndex)
                {
                    int dataCode = buffer[i + 3];  // ������ �ڵ� Ȯ��
                    if (dataCode == 0x80)  // Raw EEG ������ (0x80)
                    {
                        MSB = buffer[i + 4];
                        LSB = buffer[i + 5];

                        // 16��Ʈ signed integer ��ȯ
                        rawEEG = (MSB << 8) | LSB;
                        if (rawEEG >= 32768) rawEEG -= 65536;

                        // FFTProcessor�� ����
                        fftProcessor.ProcessRawEEG(rawEEG);

                        // ������ ���� �ʱ�ȭ
                        bufferIndex = 0;
                        return;
                    }
                }
            }
        }
    }


    void OnApplicationQuit()
    {
        //���� �����
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("MindWave ���� ����");
        }
    }
}
