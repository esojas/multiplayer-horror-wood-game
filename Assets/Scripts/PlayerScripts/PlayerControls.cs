using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public static PlayerControls Instance { get; private set; }

    public static InputActionAsset inputActions;

    public event Action<Vector2> OnMove;
    public event Action OnInteractPressed;
    public event Action OnJumpPressed;
    public event Action OnJumpReleased;
    public event Action OnPausePressed;
    public event Action OnUnPausedPressed;
    public event Action OnAttackPressed;
    public event Action OnAttackReleased;
    public event Action OnSprintPressed;
    public event Action OnSprintReleased;
    public event Action OnCrouchPressed;
    public event Action OnCrouchReleased;

    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction jumpAction;
    private InputAction pauseAction;
    private InputAction onUnPausedAction;
    private InputAction attackAction;
    private InputAction sprintAction;
    private InputAction crouchAction;

    private Action<InputAction.CallbackContext> onMovePerformed;
    private Action<InputAction.CallbackContext> onMoveCancelled;
    private Action<InputAction.CallbackContext> onInteractPerfomed;
    private Action<InputAction.CallbackContext> onJumpPerformed;
    private Action<InputAction.CallbackContext> onJumpCancelled;
    private Action<InputAction.CallbackContext> onPausePerformed;
    private Action<InputAction.CallbackContext> onUnPausedPerformed;
    private Action<InputAction.CallbackContext> onAttackPerformed;
    private Action<InputAction.CallbackContext> onAttackCancelled;
    private Action<InputAction.CallbackContext> onSprintPerformed;
    private Action<InputAction.CallbackContext> onSprintCancelled;
    private Action<InputAction.CallbackContext> onCrouchPerformed;
    private Action<InputAction.CallbackContext> onCrouchCancelled;

    private void Awake()
    {
        Instance = this;
        inputActions = InputSystem.actions;

        moveAction = InputSystem.actions.FindAction("Move");
        interactAction = InputSystem.actions.FindAction("Interact");
        jumpAction = InputSystem.actions.FindAction("Jump");
        pauseAction = InputSystem.actions.FindAction("PausedButton");
        attackAction = InputSystem.actions.FindAction("Attack");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        crouchAction = InputSystem.actions.FindAction("Crouch");

        onUnPausedAction = InputSystem.actions.FindAction("UnpausedButton");

        onMovePerformed = ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
        onMoveCancelled = ctx => OnMove?.Invoke(Vector2.zero);

        onJumpPerformed = ctx => OnJumpPressed?.Invoke();
        onJumpCancelled = ctx => OnJumpReleased?.Invoke();

        onInteractPerfomed = ctx => OnInteractPressed?.Invoke();

        onSprintPerformed = ctx => OnSprintPressed?.Invoke();
        onSprintCancelled = ctx => OnSprintReleased?.Invoke();

        onPausePerformed = ctx => OnPausePressed?.Invoke();

        onAttackPerformed = ctx => OnAttackPressed?.Invoke();
        onAttackCancelled = ctx => OnAttackReleased?.Invoke();

        onUnPausedPerformed = ctx => OnUnPausedPressed?.Invoke();

        onCrouchPerformed = ctx => OnCrouchPressed?.Invoke();
        onCrouchCancelled = ctx => OnCrouchReleased?.Invoke();
    }

    private void OnEnable()
    {
        moveAction.performed += onMovePerformed;
        moveAction.canceled += onMoveCancelled;
        interactAction.performed += onInteractPerfomed;
        jumpAction.performed += onJumpPerformed;
        jumpAction.canceled += onJumpCancelled;
        pauseAction.performed += onPausePerformed;
        attackAction.performed += onAttackPerformed;
        attackAction.canceled += onAttackCancelled;
        sprintAction.performed += onSprintPerformed;
        sprintAction.canceled += onSprintCancelled;
        crouchAction.performed += onCrouchPerformed;
        crouchAction.canceled += onCrouchCancelled;

        onUnPausedAction.performed += onUnPausedPerformed;

        inputActions.FindActionMap("Player").Enable();
        inputActions.FindActionMap("UI").Disable();
    }

    private void OnDisable()
    {
        moveAction.performed -= onMovePerformed;
        moveAction.canceled -= onMoveCancelled;
        interactAction.performed += onInteractPerfomed;
        jumpAction.performed -= onJumpPerformed;
        jumpAction.canceled -= onJumpCancelled;
        pauseAction.performed -= onPausePerformed;
        attackAction.performed -= onAttackPerformed;
        attackAction.canceled -= onAttackCancelled;
        sprintAction.performed -= onSprintPerformed;
        sprintAction.canceled -= onSprintCancelled;
        crouchAction.performed -= onCrouchPerformed;
        crouchAction.canceled -= onCrouchCancelled;

        onUnPausedAction.performed -= onUnPausedPerformed;

        inputActions.FindActionMap("Player").Disable();
        inputActions.FindActionMap("UI").Disable();
    }
}
