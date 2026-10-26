using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSprintSpeed;
    [SerializeField] private Camera cam;
    [SerializeField] private float acceleration = 40f;   
    [SerializeField] private float deceleration = 12f;

    [Header("Stamina")]
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private CanvasGroup staminaGroup;
    [SerializeField] private float fadeSpeed = 4f;
    [SerializeField] private float maxStaminaAmount;
    [SerializeField] private float staminaDrainSpeed;
    [SerializeField] private float staminaRefillSpeed;

    private float staminaAmount;
    PlayerControls playerControls;
    PlayerCrouch playerCrouch;
    PlayerCam playerCam;
    Rigidbody rb;
    bool isSprintingHold;
    Vector2 direction;
    private float currentTargetFOV = -1f;
    private void Awake()
    {
        playerControls = GetComponent<PlayerControls>();
        playerCrouch = GetComponent<PlayerCrouch>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        staminaSlider.maxValue = maxStaminaAmount;
        staminaAmount = maxStaminaAmount;
        playerCam = cam.GetComponent<PlayerCam>();
    }

    private void OnEnable()
    {
        playerControls.OnMove += HandleDirection;
        playerControls.OnSprintPressed += HandleSprint;
        playerControls.OnSprintReleased += HandleSprint;
    }

    private void OnDisable()
    {
        playerControls.OnMove -= HandleDirection;
        playerControls.OnSprintPressed -= HandleSprint;
        playerControls.OnSprintReleased -= HandleSprint;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        HandleMovement();
        
    }

    private void Update()
    {
        HandleCameraFOV();
        UpdateStaminaUI();
        if (isSprintingHold && direction.sqrMagnitude > 0.01f)
        {
            StaminaDrain();
        }
        else
        {
            StaminaRefill();
        }
    }

    public void ResetMovementState()
    {
        direction.x = 0;
        direction.y = 0;
        rb.linearVelocity = Vector3.zero;
        isSprintingHold = false;
        playerCrouch.HandleCrouchReleased();
    }

    private void HandleSprint()
    {
        isSprintingHold = !isSprintingHold;
    }

    private void HandleDirection(Vector2 dir)
    {
        direction = dir;
    }

    private void HandleMovement()
    {
        Quaternion yawRotation = Quaternion.Euler(0f, PlayerCam.Instance.Yaw, 0f);

        bool canSprint = staminaAmount > 0.01f && isSprintingHold;

        float moveSpeed;
        if (!playerCrouch.IsCrouching)
        {
            moveSpeed = canSprint ? sprintSpeed : movementSpeed;
        }
        else
        {
            moveSpeed = canSprint ? crouchSprintSpeed : crouchSpeed;
        }

        if (!playerCrouch.IsSliding)
        {
            Vector3 inputDir = yawRotation * new Vector3(direction.x, 0f, direction.y);
            inputDir = Vector3.ClampMagnitude(inputDir, 1f);

            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            Vector3 targetVel = inputDir * moveSpeed;

            // Going faster than walk speed (e.g. just left a slide)? Slow down gently.
            float rate = flatVel.magnitude > moveSpeed ? deceleration : acceleration;

            flatVel = Vector3.MoveTowards(flatVel, targetVel, rate * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector3(flatVel.x, rb.linearVelocity.y, flatVel.z);
        }

        rb.MoveRotation(yawRotation);
    }

    private void HandleCameraFOV()
    {
        bool canSprint = staminaAmount > 0.01f && isSprintingHold;
        float targetFOV = canSprint ? 75f : 65f;

        if (Mathf.Approximately(targetFOV, currentTargetFOV)) return;

        currentTargetFOV = targetFOV;
        playerCam.DoFOV(targetFOV);
    }

    private void UpdateStaminaUI()
    {
        staminaSlider.value = staminaAmount;

        // Visible while stamina is below full (draining or refilling), hidden once it's full
        float targetAlpha = staminaAmount < maxStaminaAmount ? 1f : 0f;
        staminaGroup.alpha = Mathf.MoveTowards(staminaGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
    }

    private void StaminaDrain()
    {
        staminaAmount -= staminaDrainSpeed * Time.deltaTime;
        staminaAmount = Mathf.Clamp(staminaAmount, 0f, maxStaminaAmount);
    }

    private void StaminaRefill()
    {
        staminaAmount += staminaRefillSpeed * Time.deltaTime;
        staminaAmount = Mathf.Clamp(staminaAmount, 0f, maxStaminaAmount);
    }
}
