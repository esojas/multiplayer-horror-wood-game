using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject playerGameObject;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerControls playerControls;
    private bool isPaused = false;

    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button optionButton;

    private void Awake()
    {

    }

    private void Start()
    {
        playerControls = playerGameObject.GetComponent<PlayerControls>();
        playerMovement = playerGameObject.GetComponent<PlayerMovement>();

        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        closeButton.onClick.AddListener(Resume);
        optionButton.onClick.AddListener(DisplayOption);
    }

    private void OnEnable()
    {
        playerControls.OnPausePressed += Pause;
        playerControls.OnUnPausedPressed += Resume;
    }

    private void OnDisable()
    {
        playerControls.OnPausePressed -= Pause;
        playerControls.OnUnPausedPressed -= Resume;
    }

    private void Pause()
    {
        if (isPaused) return;
        SetPaused(true);
    }

    private void Resume()
    {
        if (!isPaused) return;
        SetPaused(false);
    }

    private void DisplayOption()
    {
        optionUI.SetActive(true);
    }

    private void SetPaused(bool paused)
    {
        isPaused = paused;

        if (paused)
        {
            playerMovement.ResetMovementState();
        }

        pauseCanvas.SetActive(isPaused);
        playerControls.enabled = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
