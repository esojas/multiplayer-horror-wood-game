using UnityEngine;

public class PlayerScreamDetect : MonoBehaviour
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ScaleFromMicrophone.displayed > .75f) // ensure its the same value as the one in scalefrommicrophone
        {
            MonsterAIScript.hearNoise = true;
        }
        else
        {
            MonsterAIScript.hearNoise = false;
        }
    }

}
