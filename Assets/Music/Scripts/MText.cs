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

        UpdateSongTitle(); // Zaktualizuj tytu� na starcie
    }

    void Update()
    {
        UpdateSongTitle(); // Aktualizuj tytu� w ka�dej klatce (na wypadek zmiany utworu)
    }

    void UpdateSongTitle()
    {
        if (musicPlayer != null && textMeshProText != null)
        {
            string currentTrackName = musicPlayer.GetCurrentTrackName(); // Pobierz nazw� aktualnego utworu
            textMeshProText.text = currentTrackName; // Ustaw nazw� utworu w TextMeshPro Text
        }
    }
}