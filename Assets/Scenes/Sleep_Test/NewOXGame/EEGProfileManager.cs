using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class EEGProfileManager : MonoBehaviourPunCallbacks
{
    public Transform uiParent; // 프로필 UI를 배치할 부모 오브젝트
    public GameObject profileUIPrefab; // EEG 데이터를 표시할 UI 프리팹

    private Dictionary<GameObject, GameObject> eegToUIMap = new Dictionary<GameObject, GameObject>();

    void Update()
    {
        GameObject[] eegObjects = GameObject.FindGameObjectsWithTag("UserEEG"); // 태그를 꼭 설정해야 함
        foreach (GameObject eeg in eegObjects)
        {
            if (!eegToUIMap.ContainsKey(eeg))
            {
                AddProfileUI(eeg);
            }
        }
    }

    void AddProfileUI(GameObject eegObject)
    {
        GameObject ui = Instantiate(profileUIPrefab, uiParent);
        eegToUIMap[eegObject] = ui;

        EEGDataReceiver receiver = eegObject.GetComponent<EEGDataReceiver>();
        EEGProfileUI uiScript = ui.GetComponent<EEGProfileUI>();
        uiScript.targetReceiver = receiver;
    }
}
