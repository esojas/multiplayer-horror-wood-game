using UnityEngine;

public class AudioLoudnessDetection : MonoBehaviour
{
    public int sampleWindow = 64;
    private AudioClip microphoneClip;

    string microphoneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MicrophoneToAudioClip(0);
    }

    private void OnEnable()
    {
        MicrophoneSelector.OnMicrophoneChoiceChange += ChangeMicrophoneSource;
    }

    private void OnDisable()
    {
        MicrophoneSelector.OnMicrophoneChoiceChange -= ChangeMicrophoneSource;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeMicrophoneSource(int deviceIndex)
    {
        MicrophoneToAudioClip(deviceIndex);
    }

    public void MicrophoneToAudioClip(int microphoneIndex)
    {
        microphoneName = Microphone.devices[microphoneIndex];
        microphoneClip = Microphone.Start(microphoneName, true, 20,AudioSettings.outputSampleRate);

    }

    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(microphoneName), microphoneClip);
    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;

        if(startPosition < 0 ) return 0;

        float[] waveData = new float[sampleWindow];

        clip.GetData(waveData, startPosition);

        // Compute the loudness.

        float totalLoudness = 0;
        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / sampleWindow; // Get mean value of the array
    }


}
