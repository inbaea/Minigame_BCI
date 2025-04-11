using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static QuestionData;

public class EEGDataStore : MonoBehaviour
{
    [System.Serializable]
    public class EEG_Store
    {
        public string eegType;
        public float eegPower;
        public string topic;
        public string questionText;
        public bool currCorrected; // 문제를 맞추면 O, 틀렸다면 X.
        public string level; // 1 ~ 10
    }
    public List<EEG_Store> storedData = new List<EEG_Store>();

    public GameObject OX;

    private float timer = 0f;
    public float interval = 0.75f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f; // 타이머 초기화

            // 매 프레임마다 새로운 EEG_Store 생성
            EEG_Store newEntry = new EEG_Store()
            {
                eegType = gameObject.name,
                eegPower = gameObject.GetComponent<InletOutlet2>().EEGpow, // 랜덤 EEG 파워
                topic = OX.GetComponent<OXManager>().topicText.text,
                questionText = OX.GetComponent<OXManager>().questionText.text,
                currCorrected = OX.GetComponent<OXManager>().currAns,
                level = OX.GetComponent<OXManager>().levelText.text // 1 ~ 10 레벨 랜덤
            };

            storedData.Add(newEntry);
        }
    }
}
