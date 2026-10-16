using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class PlayerCam : MonoBehaviour
{
    public static PlayerCam Instance { get; private set; }
    public float turnSpeed = 4.0f;
    private Transform target;
    [SerializeField] private float setCameraDistance = 0.5f;
    [SerializeField] private float setCameraSmoothness = 1f;
    private Vector3 smoothedTargetPosition;
    private Transform cameraPosition;
    private float targetDistance;
    public float verticalOffset = 0f;
    public float minTurnAngle = -90.0f;
    public float maxTurnAngle = 0.0f;
    private float rotX;
    [SerializeField] private GameObject playerGameObject;
    [SerializeField] private Transform playerCameraPosition;
    private float rotY;                  // add this
    public float Yaw => rotY;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        turnSpeed = PlayerSettings.Sensitivity;
        PlayerSettings.OnSensitivityChanged += OnSensitivityChanged;
        SetCameraTarget(playerGameObject);

        // TEMPORARY
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //GameEventsManager.Instance.onChangeCameraTarget += SetCameraTarget;
        //GameEventsManager.Instance.onRestartLevel += OnRestartLevel;
    }

    private void OnSensitivityChanged(float newSensitivity)
    {
        turnSpeed = newSensitivity;
    }

    private void OnDestroy()
    {
        PlayerSettings.OnSensitivityChanged -= OnSensitivityChanged;

        //GameEventsManager.Instance.onChangeCameraTarget -= SetCameraTarget;
        //GameEventsManager.Instance.onRestartLevel -= OnRestartLevel;
    }


    public void SetCameraTarget(GameObject playerTarget)
    {
        target = playerTarget.transform;

        if (target != null)
        {
            cameraPosition = playerCameraPosition;
            targetDistance = Vector3.Distance(cameraPosition.position, target.position) * setCameraDistance;
            smoothedTargetPosition = target.position;
        }

        else
        {
            //Debug.LogWarning("CharacterData not found!");
        }
    }

    public void OnRestartLevel()
    {

    }


    void Update()
    {
        //if (PausedControl.isPaused) return;
        ControlCamera();

    }

    private void ControlCamera()
    {
        if (target == null || cameraPosition == null) return;

        rotY += Input.GetAxis("Mouse X") * turnSpeed;
        rotX += Input.GetAxis("Mouse Y") * turnSpeed;
        rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);

        transform.rotation = Quaternion.Euler(-rotX, rotY, 0f);

        smoothedTargetPosition = Vector3.Lerp(smoothedTargetPosition, target.position, setCameraSmoothness * Time.deltaTime);

        float heightOffset = cameraPosition.position.y - smoothedTargetPosition.y;
        transform.position = smoothedTargetPosition + new Vector3(0, heightOffset, 0) - (transform.forward * targetDistance);
    }

    public void DoFOV(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }
}
