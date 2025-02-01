using UnityEngine;

public class CarCam : MonoBehaviour
{
    Transform rootNode;
    Transform carCam;
    Transform car;
    Rigidbody carPhysics;

    public float rotationThreshold = 1f;
    public float cameraStickiness = 10.0f;
    public float cameraRotationSpeed = 5.0f;
    public float maxShakeAmount = 0.1f;
    public float maxSpeed = 100f;
    public float maxSwayAmount = 0.5f;
    public float swaySensitivity = 0.1f;
    public float maxDistance = 5f;
    public float distanceSensitivity = 0.1f;
    public float spinThreshold = 360f;
    public float spinSpeedThreshold = 90f;

    public float lookReturnSpeed = 5f; // Speed to return to the default position
    public float sideViewDistance = 5.0f; // Distance for the side views
    public float sideViewHeight = 2.0f; // Height for the side views
    public float backViewDistance = 5.0f; // Distance for the back view
    public float backViewHeight = 2.0f; // Height for the back view

    private float accumulatedYRotation = 0f;
    private Vector3 previousForward;

    private bool isLookingBack;
    private bool isLookingLeft;
    private bool isLookingRight;

    private bool isPaused = false;

    void Awake()
    {
        carCam = Camera.main.transform;
        rootNode = transform;
        car = rootNode.parent;
        carPhysics = car.GetComponent<Rigidbody>();
    }

    void Start()
    {
        rootNode.parent = null;
        previousForward = car.forward;
    }

    void FixedUpdate()
    {
        if (!isPaused)
        {
            Quaternion look;

            // Follow the car's position
            rootNode.position = Vector3.Lerp(rootNode.position, car.position, cameraStickiness * Time.fixedDeltaTime);

            // Determine the direction to look
            if (carPhysics.velocity.magnitude < rotationThreshold)
                look = Quaternion.LookRotation(car.forward);
            else
                look = Quaternion.LookRotation(carPhysics.velocity.normalized);

            // Rotate towards the direction
            look = Quaternion.Slerp(rootNode.rotation, look, cameraRotationSpeed * Time.fixedDeltaTime);
            rootNode.rotation = look;

            // Apply camera effects
            AddCameraShake();
            AddCameraSway();
            AdjustCameraDistance();
            HandleLookAround();
        }
    }

    void AddCameraShake()
    {
        float speedFactor = Mathf.Clamp01(carPhysics.velocity.magnitude / maxSpeed);
        float shakeAmount = maxShakeAmount * speedFactor;

        Vector3 shakeOffset = new Vector3(
            Random.Range(-shakeAmount, shakeAmount),
            Random.Range(-shakeAmount, shakeAmount),
            Random.Range(-shakeAmount, shakeAmount)
        );

        rootNode.position += shakeOffset;
    }

    void AddCameraSway()
    {
        float turnAmount = Vector3.Dot(carPhysics.velocity.normalized, car.right);
        float swayAmount = Mathf.Clamp(turnAmount * swaySensitivity, -maxSwayAmount, maxSwayAmount);

        rootNode.position += car.right * swayAmount;
    }

    void AdjustCameraDistance()
    {
        Vector3 currentForward = car.forward;
        float angleDifference = Vector3.SignedAngle(previousForward, currentForward, Vector3.up);
        accumulatedYRotation += angleDifference;

        if (Mathf.Abs(accumulatedYRotation) >= spinThreshold && Mathf.Abs(angleDifference) > spinSpeedThreshold)
        {
            float distanceFactor = Mathf.Clamp01(Mathf.Abs(accumulatedYRotation) * distanceSensitivity);
            float desiredDistance = Mathf.Lerp(0f, maxDistance, distanceFactor);

            Vector3 backOffset = -car.forward * desiredDistance;
            rootNode.position += backOffset;

            accumulatedYRotation = 0f;
        }

        previousForward = currentForward;
    }

    void HandleLookAround()
    {
        isLookingBack = Input.GetKey(KeyCode.Keypad2);
        isLookingLeft = Input.GetKey(KeyCode.Keypad4);
        isLookingRight = Input.GetKey(KeyCode.Keypad6);

        Vector3 targetPosition;
        Quaternion targetRotation;

        if (isLookingBack)
        {
            targetPosition = car.position + car.forward * backViewDistance + Vector3.up * backViewHeight;
            targetRotation = Quaternion.LookRotation(-car.forward);
        }
        else if (isLookingLeft)
        {
            targetPosition = car.position - car.right * sideViewDistance + Vector3.up * sideViewHeight;
            targetRotation = Quaternion.LookRotation(car.position - targetPosition);
        }
        else if (isLookingRight)
        {
            targetPosition = car.position + car.right * sideViewDistance + Vector3.up * sideViewHeight;
            targetRotation = Quaternion.LookRotation(car.position - targetPosition);
        }
        else
        {
            // Default position and rotation behind the car
            targetPosition = car.position - car.forward * 5.0f + Vector3.up * 2.0f;
            targetRotation = Quaternion.LookRotation(car.forward);
        }

        carCam.position = Vector3.Lerp(carCam.position, targetPosition, lookReturnSpeed * Time.fixedDeltaTime);
        carCam.rotation = Quaternion.Slerp(carCam.rotation, targetRotation, lookReturnSpeed * Time.fixedDeltaTime);
    }

    public void SetPause(bool paused)
    {
        isPaused = paused;
    }
}