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

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f; // 타이머 초기화

            // interval에서 설정한 시간마다 새로운 EEG_Store 생성
            EEG_Store newEntry = new EEG_Store()
            {
                eegType = gameObject.name,
                eegPower = gameObject.GetComponent<InletOutlet2>().EEGpow,  // 뇌파 타입은 InletOutlet2 코드에서 설정
                topic = OX.GetComponent<OXManager>().topicText.text,        // OXManager에서 기타 정보 추출
                questionText = OX.GetComponent<OXManager>().questionText.text,
                currCorrected = OX.GetComponent<OXManager>().currAns,
                level = OX.GetComponent<OXManager>().levelText.text
            };

            storedData.Add(newEntry);
        }
    }
}
