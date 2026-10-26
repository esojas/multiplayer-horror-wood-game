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

    [SerializeField] private LayerMask hidingObject;
    public static bool IsHidden { get; private set; }
    //[SerializeField] private float upRayDistance;
    [SerializeField] private float upRayRadius;
    [SerializeField] private Transform headPos;
    bool crouchButtonReleased;

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

    private void Update()
    {
        
        if (!PlayerIsHidding() && crouchButtonReleased)
        {
            CrouchReleased();
        }
    }

    private void FixedUpdate()
    {
        if (!IsSliding) return;

        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVel.magnitude <= slideEndSpeed)
            IsSliding = false;
            rb.linearDamping = 0f;
    }

    private bool PlayerIsHidding()
    {
        if(Physics.SphereCast(headPos.position, upRayRadius, headPos.up, out RaycastHit hit, hidingObject))
        {
            IsHidden = true;
            return true;
        }
        else
        {
            IsHidden = false;
            return false;
        }
    }

    private void HandleDirection(Vector2 dir) => direction = dir;

    private void HandleCrouchPressed()
    {
        SetYScale(slideYScale);
        IsCrouching = true;
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        crouchButtonReleased = false;
        if (direction.sqrMagnitude > 0.01f)
        {
            Vector3 slideDir = (orientation.forward * direction.y + orientation.right * direction.x).normalized;
            Vector3 v = slideDir * slideStartSpeed;
            rb.linearDamping = slideDamping;
            IsSliding = true;
        }
    }

    public void HandleCrouchReleased()
    {
        crouchButtonReleased = true; // Button is released
        if (PlayerIsHidding()) return;
        CrouchReleased();
    }

    private void CrouchReleased()
    {
        IsCrouching = false;
        IsSliding = false;
        rb.linearDamping = 0f;
        SetYScale(startYScale);
    }


    private void SetYScale(float y)
    {
        Vector3 s = transform.localScale;
        transform.localScale = new Vector3(s.x, y, s.z);
    }


    private void OnDrawGizmos()
    {
        if (headPos == null) return;

        Gizmos.color = Color.red;
        Vector3 start = headPos.position;

        Gizmos.DrawWireSphere(start, upRayRadius);
    }
}
