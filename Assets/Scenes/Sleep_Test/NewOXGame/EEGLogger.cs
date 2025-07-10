using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EEGLogger : MonoBehaviour
{
    public string fileName = "eeg_log";

    private string fullPath;
    private StreamWriter writer;
    private int blinkCount = 0;
    private int logCount = 0;

    void Start()
    {
        string folderPath = Application.dataPath + "/BCILog";
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        fullPath = Path.Combine(folderPath, fileName + "_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv");

        writer = new StreamWriter(fullPath, false);
        writer.WriteLine("Time,Attention,Meditation,BlinkCount,Delta,Theta,LowAlpha,HighAlpha,LowBeta,HighBeta,LowGamma,HighGamma");
    }

    public void LogEEG(
        int attention,
        int meditation,
        int blinkStrength,
        int delta,
        int theta,
        int lowAlpha,
        int highAlpha,
        int lowBeta,
        int highBeta,
        int lowGamma,
        int highGamma)
    {
        // 블링크 값이 0이 아니면 카운트 증가
        if (blinkStrength > 0)
        {
            blinkCount++;
            return;
        }

        string timestamp = System.DateTime.Now.ToString("HH:mm:ss.fff");
        string line = $"{timestamp},{attention},{meditation},{blinkCount},{delta},{theta},{lowAlpha},{highAlpha},{lowBeta},{highBeta},{lowGamma},{highGamma}";
        writer.WriteLine(line);
        writer.Flush();

        logCount++;

        if (logCount >= 20)
        {
            Debug.Log("로그가 20회 기록되어 에디터 실행을 중지합니다.");

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
    }

    void OnApplicationQuit()
    {
        writer?.Close();
    }
}
