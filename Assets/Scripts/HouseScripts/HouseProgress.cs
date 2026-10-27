using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI.Table;

public class HouseProgress : MonoBehaviour
{
    public static event System.Action<HouseProgress> OnHouseCompleted;

    [SerializeField] private Camera camPlayer;
    [SerializeField] private Transform progressCanvas;
    [SerializeField] private Image progressBar;
    [SerializeField] private float fillSpeed = 0.25f;

    private float currentProgress;
    private bool isComplete;

    private void Update()
    {
        Vector3 dir = progressCanvas.position - camPlayer.transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.0001f) 
            progressCanvas.rotation = Quaternion.LookRotation(dir);
    }

    private void Start()
    {
        progressBar.fillAmount = 0f;
    }

    public void InitiateFilling()
    {
        if (isComplete) return;

        currentProgress = Mathf.Clamp01(currentProgress + fillSpeed * Time.deltaTime);
        progressBar.fillAmount = currentProgress;

        if (currentProgress >= 1f)
        {
            isComplete = true;
            OnHouseCompleted?.Invoke(this);
        }
    }

}
