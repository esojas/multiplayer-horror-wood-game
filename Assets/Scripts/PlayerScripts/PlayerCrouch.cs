using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    [SerializeField] private float slideStartSpeed = 12f;
    [SerializeField] private float slideEndSpeed = 5f;
    [SerializeField] private float slideDamping = 1.5f;
    [SerializeField] private float slideYScale = 0.5f;

    public bool IsSliding { get; private set; }
    public bool IsCrouching { get; private set; }
    float startYScale;
    Vector2 direction;
    PlayerControls playerControls;
    Rigidbody rb;

    private void Awake()
    {
        playerControls = GetComponent<PlayerControls>();
        rb = GetComponent<Rigidbody>();
        startYScale = transform.localScale.y;
    }

    private void OnEnable()
    {
        playerControls.OnMove += HandleDirection;
        playerControls.OnCrouchPressed += HandleCrouchPressed;
        playerControls.OnCrouchReleased += HandleCrouchReleased;
    }

    private void OnDisable()
    {
        playerControls.OnMove -= HandleDirection;
        playerControls.OnCrouchPressed -= HandleCrouchPressed;
        playerControls.OnCrouchReleased -= HandleCrouchReleased;
    }

    private void HandleDirection(Vector2 dir) => direction = dir;

    private void HandleCrouchPressed()
    {
        SetYScale(slideYScale);
        IsCrouching = true;
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        if (direction.sqrMagnitude > 0.01f)
        {
            Vector3 slideDir = (orientation.forward * direction.y + orientation.right * direction.x).normalized;
            Vector3 v = slideDir * slideStartSpeed;
            rb.linearDamping = slideDamping;
            IsSliding = true;
        }
    }

    private void HandleCrouchReleased()
    {
        IsCrouching = false;
        IsSliding = false;
        rb.linearDamping = 0f;
        SetYScale(startYScale);
    }

    private void FixedUpdate()
    {
        if (!IsSliding) return;

        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVel.magnitude <= slideEndSpeed)
            IsSliding = false;
            rb.linearDamping = 0f;
    }

    private void SetYScale(float y)
    {
        Vector3 s = transform.localScale;
        transform.localScale = new Vector3(s.x, y, s.z);
    }
}
