using UnityEngine;
using UnityEngine.UI;

public class ScaleFromMicrophone : MonoBehaviour
{
    public Image audioOptionBar;
    public Slider sensitivitySlider;
    public AudioLoudnessDetection detector;
    public GameObject loudnessDetectedText;
    public GameObject loudnessInGameDetectedText;

    [Header("Sensitivity (higher = more sensitive)")]
    public float minSensibility = 5f;
    public float maxSensibility = 100f;
    [SerializeField] private float currentLoudnessSensibility = 30f;

    [Header("Tuning")]
    [Tooltip("Raw mic loudness below this counts as silence")]
    public float noiseGate = 0.005f;
    [Tooltip("Higher = bar reacts faster")]
    public float smoothing = 12f;

    public static float displayed { get; private set; }

    void Start()
    {
        if (sensitivitySlider == null) return;

        sensitivitySlider.value = .5f;
        SetLoudnessSensibility(sensitivitySlider.value);
    }

    void Update()
    {
        float raw = detector.GetLoudnessFromMicrophone();

        if (raw < noiseGate) raw = 0f;

        float target = Mathf.Clamp01(raw * currentLoudnessSensibility);

        displayed = Mathf.Lerp(displayed, target, 1f - Mathf.Exp(-smoothing * Time.unscaledDeltaTime));

        if (displayed > .75f)
        {
            loudnessDetectedText.SetActive(true);
            loudnessInGameDetectedText.SetActive(true);
        }
        else
        {
            loudnessDetectedText.SetActive(false);
            loudnessInGameDetectedText.SetActive(false);
        }
        audioOptionBar.fillAmount = displayed;
        
    }

    public void SetLoudnessSensibility(float t)
    {
        currentLoudnessSensibility = Mathf.Lerp(minSensibility, maxSensibility, t);
    }
}