using UnityEngine;
using System.Diagnostics;
using System.IO;

public class PythonAutoRunner : MonoBehaviour
{
    private Process pythonProcess;

    void Start()
    {
        string pythonExePath = "python"; // 또는 python3 (운영체제에 따라)
        string scriptPath = Path.Combine(Application.dataPath, "../mindwave_reader.py"); // EEG 파이썬 파일 경로

        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = pythonExePath;
        start.Arguments = $"\"{scriptPath}\"";
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;
        start.RedirectStandardError = true;
        start.CreateNoWindow = true;

        try
        {
            pythonProcess = new Process();
            pythonProcess.StartInfo = start;
            pythonProcess.OutputDataReceived += (sender, e) => {
                if (!string.IsNullOrEmpty(e.Data))
                    UnityEngine.Debug.Log("PY: " + e.Data);
            };
            pythonProcess.ErrorDataReceived += (sender, e) => {
                if (!string.IsNullOrEmpty(e.Data))
                    UnityEngine.Debug.LogWarning("PY ERR: " + e.Data);
            };

            pythonProcess.Start();
            pythonProcess.BeginOutputReadLine();
            pythonProcess.BeginErrorReadLine();

            UnityEngine.Debug.Log("파이썬 EEG 스크립트 자동 실행 완료!");
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError("파이썬 실행 실패: " + ex.Message);
        }
    }

    void OnApplicationQuit()
    {
        if (pythonProcess != null && !pythonProcess.HasExited)
        {
            pythonProcess.Kill();
            pythonProcess.Dispose();
            UnityEngine.Debug.Log("파이썬 프로세스 종료됨");
        }
    }
}
