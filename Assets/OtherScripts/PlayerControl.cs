using UnityEngine;
using TMPro; // Dodajemy przestrzeñ nazw TextMeshPro
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class CountdownAndPlayerLockScript : MonoBehaviour
{
    public TextMeshProUGUI countdownText; 
    private int countdown = 3;
    private bool isCountdownActive = true;
    private CarController carController;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        carController = GetComponent<CarController>(); // Pobieramy CarController z tego samego obiektu
        if (carController == null)
        {
            Debug.LogError("CarController component not found!");
        }

        if (countdownText == null)
        {
            Debug.LogError("CountdownText is not assigned in the Inspector!");
        }

        StartCoroutine(StartCountdown());
    }

    void Update()
    {
        if (!isCountdownActive && carController != null)
        {
            // Jeœli odliczanie siê zakoñczy³o, w³¹czamy sterowanie przez CarController
            carController.enabled = true;
        }
    }

    IEnumerator StartCountdown()
    {
        while (countdown > 0)
        {
            if (countdownText != null)
            {
                countdownText.text = countdown.ToString(); // Wyœwietlamy odliczanie
            }
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        isCountdownActive = false;

        // Po zakoñczeniu odliczania, ustawiamy odpowiednie ograniczenia na Rigidbody
        rb.constraints = RigidbodyConstraints.None;

        if (countdownText != null)
        {
            countdownText.text = ""; // Czyœcimy tekst po odliczaniu
        }
    }
}