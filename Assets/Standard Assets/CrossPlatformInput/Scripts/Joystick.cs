using UnityEngine;

public class CarControls : MonoBehaviour
{
    public enum SpeedUnit { KPH, MPH }
    public SpeedUnit speedUnit = SpeedUnit.KPH;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Metoda do zwracania prêdkoœci w odpowiednich jednostkach
    public float GetSpeed()
    {
        float speed = rb.velocity.magnitude; // Prêdkoœæ w metrach na sekundê
        if (speedUnit == SpeedUnit.KPH)
        {
            return speed * 3.6f; // Konwersja na km/h
        }
        else
        {
            return speed * 2.237f; // Konwersja na mph
        }
    }
}