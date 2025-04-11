using System;
using System.IO.Ports;
using UnityEngine;
using System.Numerics;  // FFT 연산을 위한 System.Numerics 사용

public class MindwaveSerialReader : MonoBehaviour
{
    SerialPort serialPort;
    public string portName = "COM3";  // MindWave의 COM 포트
    public int baudRate = 57600;      // ThinkGear 기본 통신 속도
    private byte[] buffer = new byte[256];
    private int bufferIndex = 0;

    public FFTProcessor fftProcessor;  // FFT 처리기
    public int MSB;
    public int LSB;
    public int rawEEG;

    void Start()
    {
        //블루투스 Serial Port 연결 시도
        try
        {
            serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
            serialPort.Open();
            Debug.Log("MindWave 연결 성공: " + portName);
        }
        //연결 오류시
        catch (Exception e)
        {
            Debug.LogError("시리얼 포트 오류: " + e.Message);
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

                    // 데이터 처리
                    ProcessMindWaveData();
                }
            }
            //데이터 오류시
            catch (Exception e)
            {
                Debug.LogError("데이터 읽기 오류: " + e.Message);
            }
        }
    }

    void ProcessMindWaveData()
    {
        for (int i = 0; i < bufferIndex - 3; i++)
        {
            if (buffer[i] == 0xAA && buffer[i + 1] == 0xAA)  // 패킷 헤더 확인
            {
                int payloadLength = buffer[i + 2];  // 페이로드 크기
                if (i + 3 + payloadLength < bufferIndex)
                {
                    int dataCode = buffer[i + 3];  // 데이터 코드 확인
                    if (dataCode == 0x80)  // Raw EEG 데이터 (0x80)
                    {
                        MSB = buffer[i + 4];
                        LSB = buffer[i + 5];

                        // 16비트 signed integer 변환
                        rawEEG = (MSB << 8) | LSB;
                        if (rawEEG >= 32768) rawEEG -= 65536;

                        // FFTProcessor로 전달
                        fftProcessor.ProcessRawEEG(rawEEG);

                        // 데이터 버퍼 초기화
                        bufferIndex = 0;
                        return;
                    }
                }
            }
        }
    }


    void OnApplicationQuit()
    {
        //연결 종료시
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("MindWave 연결 종료");
        }
    }
}
