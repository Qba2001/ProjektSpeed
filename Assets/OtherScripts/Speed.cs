using UnityEngine;

public class Speed : MonoBehaviour
{
    public enum SpeedUnit
    {
        KPH,
        MPH
    }

    public SpeedUnit speedUnit = SpeedUnit.KPH;

    private Rigidbody carRigidbody; // Reference to the Rigidbody of the car

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        if (carRigidbody == null)
        {
            Debug.LogError("Rigidbody component not found on the vehicle.");
        }
    }

    // Method to calculate speed in the selected unit (KPH or MPH)
    public float GetSpeed()
    {
        if (carRigidbody != null)
        {
            float speed = carRigidbody.velocity.magnitude; // Speed in meters per second

            // Convert speed to KPH or MPH based on the selected unit
            if (speedUnit == SpeedUnit.KPH)
            {
                speed *= 3.6f; // Convert m/s to km/h
            }
            else
            {
                speed *= 2.23694f; // Convert m/s to mph
            }

            return speed;
        }
        else
        {
            return 0f; // Return 0 if Rigidbody is not found
        }
    }
}