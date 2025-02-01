using UnityEngine;

public class RotateCarModel : MonoBehaviour
{
    public float rotationSpeed = 90.0f;
    private bool shouldRotate = true; // Flaga okreœlaj¹ca, czy obiekt powinien siê obracaæ

    private static RotateCarModel instance; // Statyczna instancja, aby upewniæ siê, ¿e jest tylko jedna instancja

    void Start()
    {
        // Zapewniamy, ¿e obiekt nie zostanie zniszczony przy przejœciu miêdzy scenami
        if (instance == null)
        {
            instance = this; // Przypisujemy obiekt jako jedyn¹ instancjê
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Jeœli istnieje ju¿ instancja, niszczymy ten obiekt (unikamy duplikatów)
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Sprawdzamy, czy obiekt powinien siê obracaæ
        if (shouldRotate)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    public void StartRotating()
    {
        // W³¹cza obracanie
        shouldRotate = true;
    }

    public void StopRotating()
    {
        // Zatrzymuje obracanie
        shouldRotate = false;
    }

    // Metoda do obs³ugi zdarzenia, które uruchamia ponowne obracanie siê samochodu
    public void OnResumeFromMenu()
    {
        // Uruchamia obracanie samochodu po powrocie z menu
        StartRotating();
    }

    public void OnExitRace()
    {
        // Zatrzymuje obracanie po wyjœciu z wyœcigu
        StopRotating();
    }
}