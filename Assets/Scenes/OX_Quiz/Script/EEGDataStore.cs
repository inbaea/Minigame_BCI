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
        public bool currCorrected; // ������ ���߸� O, Ʋ�ȴٸ� X.
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
            timer = 0f; // Ÿ�̸� �ʱ�ȭ

            // �� �����Ӹ��� ���ο� EEG_Store ����
            EEG_Store newEntry = new EEG_Store()
            {
                eegType = gameObject.name,
                eegPower = gameObject.GetComponent<InletOutlet2>().EEGpow, // ���� EEG �Ŀ�
                topic = OX.GetComponent<OXManager>().topicText.text,
                questionText = OX.GetComponent<OXManager>().questionText.text,
                currCorrected = OX.GetComponent<OXManager>().currAns,
                level = OX.GetComponent<OXManager>().levelText.text // 1 ~ 10 ���� ����
            };

            storedData.Add(newEntry);
        }
    }
}
