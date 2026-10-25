using System.Linq.Expressions;
using UnityEngine;

public class PlayerMic : MonoBehaviour
{
    public float loadnessThreshold = .1f; // mic sensitivty


    private string playermicName;
    private AudioClip clipRecord;
    private int sampleNumber = 128;
    private float[] waveData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waveData = new float[sampleNumber];

        if(Microphone.devices.Length > 0)
        {
            playermicName = Microphone.devices[0];
            clipRecord = Microphone.Start(playermicName, true, 999, 44100);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
