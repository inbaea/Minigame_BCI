using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;
using LSL4Unity.Utils;

public class InletOutlet : MonoBehaviour
{
    #region LSL4Unity_inlet
    public string StreamName;// OpenViBE에서 StreamName과 동일해야 함
    public string StreamType;
    ContinuousResolver resolver;
    double max_chunk_duration = 0.5;//OpenViBE에서 Epoch Interval과 동일
    private StreamInlet inlet;

    private float[,] data_buffer;//EEG data를 저장하기 위한 버퍼
    private double[] timestamp_buffer;
    float EEGpow;//EEG power를 계산하기 위한 변수
    #endregion

    #region LSL4Unity_outlet
    int ChannelCount = 1;
    private StreamOutlet stimulationOutlet; // 
    private StreamOutlet signalOutlet; // 
    #endregion

    void Awake()
    {
        if (!StreamName.Equals(""))
            resolver = new ContinuousResolver("name", StreamName);
        else
        {
            Debug.LogError("Object must specify a name for resolver to lookup a stream.");
            this.enabled = false;
            return;
        }
        StartCoroutine(ResolveExpectedStream());
    }

    IEnumerator ResolveExpectedStream()
    {
        var results = resolver.results();
        while (results.Length == 0)
        {
            yield return new WaitForSeconds(.1f);
            results = resolver.results();
        }
        inlet = new StreamInlet(results[0]);
        var streamInfo = inlet.info();
        StreamType = streamInfo.type();
        int buf_samples = (int)Mathf.Ceil((float)(streamInfo.nominal_srate() * max_chunk_duration));
        int n_channels = streamInfo.channel_count();
        data_buffer = new float[buf_samples, n_channels];
        timestamp_buffer = new double[buf_samples];
    }

    void Start()
    {
        SetupStimulationOutlet(); // Setup for stimulation outlet
        SetupSignalOutlet(); // Setup for signal outlet
    }

    void FixedUpdate()
    {
        if (inlet != null)
        {
            int samples_returned = inlet.pull_chunk(data_buffer, timestamp_buffer);
            if (samples_returned > 0)
            {
                float x = data_buffer[samples_returned - 1, 0];
                EEGpow = x;
                Debug.Log(EEGpow);
            }
        }
        SendSignalData(ChannelCount);
    }
    
    void SetupStimulationOutlet()
    {
        string stimulationStreamName = StreamName + "_Stimulations";
        string stimulationStreamType = "Markers";
        string uniqueSourceId = gameObject.GetInstanceID().ToString();

        StreamInfo streamInfo_stimulation = new StreamInfo(stimulationStreamName, stimulationStreamType, ChannelCount, LSL.LSL.IRREGULAR_RATE, channel_format_t.cf_int32, uniqueSourceId);
        stimulationOutlet = new StreamOutlet(streamInfo_stimulation);
    }

    void SetupSignalOutlet()
    {
        float signalSamplingRate = 50; // Example: 250 Hz, adjust as needed
        string signalStreamName = StreamName + "_Signal";
        string signalStreamType = "EEG"; // Adjust if streaming different data type
        string uniqueSourceId = gameObject.GetInstanceID().ToString();
        StreamInfo streamInfo_signal = new StreamInfo(signalStreamName, signalStreamType, ChannelCount, signalSamplingRate, channel_format_t.cf_float32, uniqueSourceId);
        signalOutlet = new StreamOutlet(streamInfo_signal);
    }

    public void SendSignalData(int n_channels)
    {
        float[] signalData = new float[n_channels];
        signalData[0] = 1f;

        if (signalOutlet != null)
        {
            double timestamp = LSL.LSL.local_clock();
            signalOutlet.push_sample(signalData, timestamp);
        }
    }
    
    public void SendStimulation(int markerValue)
    {
        int[] marker = new int[1] { markerValue };
        if (stimulationOutlet != null)
        {
            stimulationOutlet.push_sample(marker);
            Debug.Log("Sent stimulation marker: " + markerValue);
        }
    }

    


}
