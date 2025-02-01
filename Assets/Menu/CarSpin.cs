using UnityEngine;

public class RotateCarModel : MonoBehaviour
{
    public float rotationSpeed = 90.0f;
    private bool shouldRotate = true; // Flaga okre�laj�ca, czy obiekt powinien si� obraca�

    private static RotateCarModel instance; // Statyczna instancja, aby upewni� si�, �e jest tylko jedna instancja

    void Start()
    {
        // Zapewniamy, �e obiekt nie zostanie zniszczony przy przej�ciu mi�dzy scenami
        if (instance == null)
        {
            instance = this; // Przypisujemy obiekt jako jedyn� instancj�
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Je�li istnieje ju� instancja, niszczymy ten obiekt (unikamy duplikat�w)
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Sprawdzamy, czy obiekt powinien si� obraca�
        if (shouldRotate)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    public void StartRotating()
    {
        // W��cza obracanie
        shouldRotate = true;
    }

    public void StopRotating()
    {
        // Zatrzymuje obracanie
        shouldRotate = false;
    }

    // Metoda do obs�ugi zdarzenia, kt�re uruchamia ponowne obracanie si� samochodu
    public void OnResumeFromMenu()
    {
        // Uruchamia obracanie samochodu po powrocie z menu
        StartRotating();
    }

    public void OnExitRace()
    {
        // Zatrzymuje obracanie po wyj�ciu z wy�cigu
        StopRotating();
    }
}