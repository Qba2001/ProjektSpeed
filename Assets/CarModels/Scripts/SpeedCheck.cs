using UnityEngine;

public class SpeedCheck : MonoBehaviour
{
    private CarController carController;

    void Start()
    {
        carController = FindObjectOfType<CarController>();

        if (carController == null)
        {
            Debug.LogError("Nie znaleziono komponentu CarController.");
        }
    }

    void Update()
    {
        if (carController != null)
        {
            float speed = carController.CurrentSpeed;
            Debug.Log("Current Speed: " + speed);
        }
    }
}