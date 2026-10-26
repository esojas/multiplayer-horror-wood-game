using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [SerializeField] private Button closeOptionButton;
    [SerializeField] private GameObject optionUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closeOptionButton.onClick.AddListener(HideOptionUI);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HideOptionUI()
    {
        optionUI.SetActive(false);
    }

}
