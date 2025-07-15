using System.Collections;
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
    private bool hasStartedShutdown = false;

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
        }

        // 항상 EEG 로그 기록
        string timestamp = System.DateTime.Now.ToString("HH:mm:ss.fff");
        string line = $"{timestamp},{attention},{meditation},{blinkCount},{delta},{theta},{lowAlpha},{highAlpha},{lowBeta},{highBeta},{lowGamma},{highGamma}";
        writer.WriteLine(line);
        writer.Flush();

        // 최초 한 번만 코루틴 실행
        if (!hasStartedShutdown)
        {
            hasStartedShutdown = true;

            StartCoroutine(ShutdownAfterDelay(30f));
            StartCoroutine(Delay_A(1, 10f));
            StartCoroutine(Delay_B(1, 15f));
            StartCoroutine(Delay_A(2, 20f));
            StartCoroutine(Delay_B(2, 25f));
        }
    }

    private IEnumerator ShutdownAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

#if UNITY_EDITOR
        Debug.Log("30초가 지나 에디터 실행을 중지합니다.");
        EditorApplication.isPlaying = false;
#endif
    }

    private IEnumerator Delay_A(int A, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log($"{A}번째 실행! 5초간 행동 실행!");
    }

    private IEnumerator Delay_B(int B, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log($"{B}번째 휴식! 5초간 휴식!");
    }

    void OnApplicationQuit()
    {
        writer?.Close();
    }
}
