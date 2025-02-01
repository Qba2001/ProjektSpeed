using UnityEngine;

public class CarController : MonoBehaviour
{
    private Transform cameraTransform;
    public float movementSpeed = 50.0f;
    public float rotationSpeed = 100.0f;
    public float cameraRotationSpeed = 100.0f;

    public float CurrentSpeed { get; private set; }

    private void Awake()
    {
        cameraTransform = Camera.main.transform;

        if (cameraTransform == null)
        {
            Debug.LogError("Nie znaleziono kamery g³ównej.");
        }
    }

    private void Update()
    {
        // Get input
        float steer = Input.GetAxis("Horizontal");
        float throttle = Input.GetAxis("Vertical");
        float cameraHorizontal = Input.GetAxis("Mouse X");
        float cameraVertical = Input.GetAxis("Mouse Y");

        // Move the car
        transform.Translate(Vector3.forward * throttle * movementSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up, steer * rotationSpeed * Time.deltaTime);

        // Rotate the camera around the car
        cameraTransform.RotateAround(transform.position, Vector3.up, cameraHorizontal * cameraRotationSpeed * Time.deltaTime);
        cameraTransform.RotateAround(transform.position, cameraTransform.right, -cameraVertical * cameraRotationSpeed * Time.deltaTime);

        // Calculate current speed (based on local movement, for example)
        CurrentSpeed = throttle * movementSpeed;
    }
}