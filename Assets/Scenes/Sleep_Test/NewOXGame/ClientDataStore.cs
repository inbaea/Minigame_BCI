using System.Collections.Generic;
using UnityEngine;
using static QuestionData;

public class ClientDataStore : MonoBehaviour
{
    [System.Serializable]
    public class EEG_Store
    {
        public string eegType;
        public float eegPower;
        public string topic;
        public string questionText;
        public bool currCorrected;
        public string level;
    }

    public List<EEG_Store> storedData = new List<EEG_Store>();
    public GameObject OX;
    public GameObject receiver;

    private float timer = 0f;
    public float interval = 0.75f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            float selectedEEGPower = 0f;

            switch (gameObject.name)
            {
                case "delta":
                    selectedEEGPower = receiver.GetComponent<EEGData>().delta;
                    break;
                case "theta":
                    selectedEEGPower = receiver.GetComponent<EEGData>().theta;
                    break;
                case "lowAlpha":
                    selectedEEGPower = receiver.GetComponent<EEGData>().lowAlpha;
                    break;
                case "highAlpha":
                    selectedEEGPower = receiver.GetComponent<EEGData>().highAlpha;
                    break;
                case "lowBeta":
                    selectedEEGPower = receiver.GetComponent<EEGData>().lowBeta;
                    break;
                case "highBeta":
                    selectedEEGPower = receiver.GetComponent<EEGData>().highBeta;
                    break;
                case "lowGamma":
                    selectedEEGPower = receiver.GetComponent<EEGData>().lowGamma;
                    break;
                case "highGamma":
                    selectedEEGPower = receiver.GetComponent<EEGData>().highGamma;
                    break;
                default:
                    Debug.LogWarning($"알 수 없는 eegType: {gameObject.name}");
                    break;
            }

            EEG_Store newEntry = new EEG_Store()
            {
                eegType = gameObject.name,
                eegPower = selectedEEGPower,
                topic = OX.GetComponent<ClientOX>().topicText.text,
                questionText = OX.GetComponent<ClientOX>().questionText.text,
                currCorrected = OX.GetComponent<ClientOX>().currAns,
                level = OX.GetComponent<ClientOX>().levelText.text
            };

            storedData.Add(newEntry);
        }
    }
}
