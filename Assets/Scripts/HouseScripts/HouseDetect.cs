using UnityEngine;

public class HouseDetect : MonoBehaviour
{
    private HouseProgress houseProgressScript;

    private void Awake()
    {
        houseProgressScript = GetComponent<HouseProgress>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 6) return;

        if(ScaleFromMicrophone.displayed > .75f) // Ensure its the same as scalefrommicrophone
        {
            houseProgressScript.InitiateFilling();
        }
    }
}
