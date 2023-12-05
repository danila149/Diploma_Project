using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLounge : MonoBehaviour
{
    public int sampleWindow = 128;
    private AudioClip microphoneClip;
    void Start()
    {
        MicrophoneToAudioClip();
    }

    void Update()
    {
        
    }

    public void MicrophoneToAudioClip()
    {
        microphoneClip = Microphone.Start(null, true, 10, AudioSettings.outputSampleRate);
    }


    public float GetLoudnessFromMicrophone()
    {
        return GetLoundnessFromAudioClip(Microphone.GetPosition(null), microphoneClip);
    }

    public float GetLoundnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;

        if(startPosition < 0)
            return 0;

        float[] waveDate = new float[startPosition];
        clip.GetData(waveDate, startPosition);

        float totalLoundness = 0;

        for(int i = 0; i < sampleWindow; i++)
        {
            totalLoundness += Mathf.Abs(waveDate[i]);
        }

        return totalLoundness / sampleWindow;
    }
}
