using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
// Lies Under MicDropDown
public class MicrophoneSelector : MonoBehaviour
{
    private TMP_Dropdown sourceDropdown;
    public int chosenDeviceIndex = 0;

    public static UnityAction<int> OnMicrophoneChoiceChange;

    private void Awake()
    {
        sourceDropdown = GetComponent<TMP_Dropdown>();
    }

    void Start()
    {
        PopulateSourceDropDown();
    }

    private void PopulateSourceDropDown()
    {
        var options = new List<TMP_Dropdown.OptionData>();

        foreach(var micIn in Microphone.devices)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(micIn);

            options.Add(optionData);
        }

        sourceDropdown.options = options;
    }

    public void ChooseMicrophone(int optionIndex)
    {
        chosenDeviceIndex = optionIndex;
        OnMicrophoneChoiceChange?.Invoke(chosenDeviceIndex);
    }
}
