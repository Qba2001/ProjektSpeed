using UnityEngine;

public class MovementEnabler : MonoBehaviour
{
    public static MovementEnabler Instance { get; private set; }

    private Rigidbody playerRigidbody;
    private bool movementEnabled = true;

    void Awake()
    {
        // Ensure there is only one instance of MovementEnabler
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: If you want this to persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        // Assuming the player has a Rigidbody component attached to the same GameObject
        playerRigidbody = GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            Debug.LogError("No Rigidbody component found on the player object.");
        }
    }

    public void SetMovementEnabled(bool enabled)
    {
        movementEnabled = enabled;
        if (playerRigidbody != null)
        {
            if (enabled)
            {
                playerRigidbody.constraints = RigidbodyConstraints.None;
            }
            else
            {
                playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }

    void Update()
    {
        // Additional logic to control movement can be added here
        // if necessary
    }
}