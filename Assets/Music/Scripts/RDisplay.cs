using UnityEngine;
using UnityEngine.UI;

public class RMusicText : MonoBehaviour
{
    public RaceMusicDisplay musicPlayer; // Referencja do RaceMusicDisplay
    public GameObject musicTitleObject; // Referencja do GameObject z tekstem

    private Text musicTitleText; // Referencja do komponentu Text wewn¹trz GameObject

    private void Start()
    {
        // Pobierz komponent Text z obiektu musicTitleObject
        if (musicTitleObject != null)
        {
            musicTitleText = musicTitleObject.GetComponent<Text>();
            if (musicTitleText == null)
            {
                Debug.LogError("GameObject musicTitleObject nie zawiera komponentu Text.");
            }
        }
        else
        {
            Debug.LogError("musicTitleObject nie jest przypisany.");
        }

        if (musicPlayer == null)
        {
            Debug.LogError("RaceMusicDisplay nie jest przypisany do RMusicText.");
        }

        UpdateSongTitle(); // Zaktualizuj tytu³ na starcie
    }

    private void Update()
    {
        UpdateSongTitle(); // Aktualizuj tytu³ w ka¿dej klatce (na wypadek zmiany utworu)
    }

    private void UpdateSongTitle()
    {
        if (musicPlayer != null && musicTitleText != null)
        {
            string currentTrackName = musicPlayer.GetCurrentTrackName(); // Pobierz nazwê aktualnego utworu
            musicTitleText.text = currentTrackName; // Ustaw nazwê utworu w komponencie Text
        }
    }
}