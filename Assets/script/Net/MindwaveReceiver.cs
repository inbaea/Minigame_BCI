using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using Newtonsoft.Json.Linq;  // JSON ������ �Ľ��� ���� �ʿ� (Newtonsoft.Json ��Ű�� ���)

public class MindwaveReceiver : MonoBehaviour
{
    private TcpClient client;
    private StreamReader reader;
    public int Theta;
    public int Alpha;
    public int Beta;
    public int Gamma;

    void Start()
    {
        try
        {
            // ThinkGear Connector�� TCP ���� (��Ʈ 13854)
            client = new TcpClient("127.0.0.1", 13854);
            reader = new StreamReader(client.GetStream());
            Debug.Log("Connected to ThinkGear Connector!");

            //JSON ��� Ȱ��ȭ ��û
            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.Write("{\"enableRawOutput\": false, \"format\": \"Json\"}\n");
            writer.Flush();
        }
        catch (Exception e)
        {
            Debug.LogError("Connection error: " + e.Message);
        }
    }

    void Update()
    {
        if (client != null && client.Connected)
        {
            try
            {
                //ThinkGear Connector���� EEG ������ ����
                string data = reader.ReadLine();
                if (!string.IsNullOrEmpty(data))
                {
                    JObject json = JObject.Parse(data);  // JSON �Ľ�

                    //���ļ� �뿪�� ���� ������ ����
                    if (json["eegPower"] != null)
                    {
                        int delta = json["eegPower"]["delta"]?.ToObject<int>() ?? 0;
                        int theta = json["eegPower"]["theta"]?.ToObject<int>() ?? 0;
                        int lowAlpha = json["eegPower"]["lowAlpha"]?.ToObject<int>() ?? 0;
                        int highAlpha = json["eegPower"]["highAlpha"]?.ToObject<int>() ?? 0;
                        int lowBeta = json["eegPower"]["lowBeta"]?.ToObject<int>() ?? 0;
                        int highBeta = json["eegPower"]["highBeta"]?.ToObject<int>() ?? 0;
                        int lowGamma = json["eegPower"]["lowGamma"]?.ToObject<int>() ?? 0;
                        int midGamma = json["eegPower"]["midGamma"]?.ToObject<int>() ?? 0;

                        Alpha = lowAlpha + highAlpha;
                        Beta = highBeta + lowBeta;
                        Gamma = lowGamma + midGamma;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Read error: " + e.Message);
            }
        }
    }

    void OnApplicationQuit()
    {
        if (client != null) client.Close();
    }
}