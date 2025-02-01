using UnityEngine;
using TMPro;

public class MText : MonoBehaviour
{
    public RandomMenuMusicPlayer musicPlayer; // Referencja do RandomMenuMusicPlayer
    public TextMeshProUGUI textMeshProText; // Referencja do TextMeshPro Text

    void Start()
    {
        if (musicPlayer == null)
        {
            Debug.LogError("RandomMenuMusicPlayer nie jest przypisany do MText.");
        }

        if (textMeshProText == null)
        {
            Debug.LogError("TextMeshPro Text nie jest przypisany do MText.");
        }

        UpdateSongTitle(); // Zaktualizuj tytu³ na starcie
    }

    void Update()
    {
        UpdateSongTitle(); // Aktualizuj tytu³ w ka¿dej klatce (na wypadek zmiany utworu)
    }

    void UpdateSongTitle()
    {
        if (musicPlayer != null && textMeshProText != null)
        {
            string currentTrackName = musicPlayer.GetCurrentTrackName(); // Pobierz nazwê aktualnego utworu
            textMeshProText.text = currentTrackName; // Ustaw nazwê utworu w TextMeshPro Text
        }
    }
}