using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class MusicManager : MonoBehaviour
{
    public string musicFolderPath = "MenuMusic"; // Œcie¿ka do folderu z muzyk¹, relatywna do Assets
    public TextMeshProUGUI musicTitleText; // Referencja do TextMeshProUGUI

    private List<AudioClip> musicClips = new List<AudioClip>(); // Lista utworów
    private AudioSource audioSource; // Referencja do komponentu AudioSource
    private int currentTrackIndex = 0;
    private bool isInMainMenu = true; // Flaga wskazuj¹ca, czy jesteœmy w menu g³ównym

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false; // Ustawienie na false, aby nie zapêtlaæ utworów

        LoadMusic();
    }

    private void LoadMusic()
    {
        string path = Path.Combine(Application.streamingAssetsPath, musicFolderPath);

        if (Directory.Exists(path))
        {
            string[] files = Directory.GetFiles(path, "*.mp3");
            foreach (string file in files)
            {
                StartCoroutine(LoadAudioClip(file));
            }
        }
        else
        {
            Debug.LogError("Folder z muzyk¹ nie istnieje: " + path);
        }
    }

    private IEnumerator LoadAudioClip(string filePath)
    {
        string url = "file://" + filePath;
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                if (clip != null)
                {
                    clip.name = Path.GetFileNameWithoutExtension(filePath);
                    musicClips.Add(clip);
                    Debug.Log("Za³adowano utwór: " + clip.name);

                    // Automatycznie odtwórz pierwszy za³adowany utwór
                    if (musicClips.Count == 1)
                    {
                        PlayMusic();
                    }
                }
            }
            else
            {
                Debug.LogError("B³¹d podczas ³adowania utworu: " + request.error);
            }
        }
    }

    private void PlayMusic()
    {
        if (musicClips.Count == 0)
        {
            Debug.LogError("Brak utworów do odtworzenia.");
            return;
        }

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.clip = musicClips[currentTrackIndex];
        audioSource.Play();

        // Aktualizuj tytu³ piosenki w interfejsie u¿ytkownika tylko w menu g³ównym
        UpdateMusicTitle();
    }

    private void UpdateMusicTitle()
    {
        if (isInMainMenu && musicTitleText != null)
        {
            musicTitleText.text = audioSource.clip.name;
        }
    }

    // Metoda do odtwarzania nastêpnego utworu
    public void PlayNextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % musicClips.Count;
        PlayMusic();
    }

    // Metoda do odtwarzania poprzedniego utworu
    public void PlayPreviousTrack()
    {
        currentTrackIndex = (currentTrackIndex - 1 + musicClips.Count) % musicClips.Count;
        PlayMusic();
    }

    private void Update()
    {
        // SprawdŸ, czy utwór siê skoñczy³ i odtwórz nastêpny
        if (!audioSource.isPlaying && musicClips.Count > 0)
        {
            PlayNextTrack();
        }
    }

    // Metoda ustawiaj¹ca, ¿e jesteœmy w menu g³ównym
    public void SetInMainMenu(bool value)
    {
        isInMainMenu = value;

        // Jeœli jesteœmy w menu g³ównym, zatrzymaj odtwarzanie i ustaw pierwszy utwór
        if (isInMainMenu)
        {
            StopMusic();
            currentTrackIndex = 0;
            PlayMusic();
        }
        else // Jeœli nie jesteœmy w menu g³ównym, zatrzymaj odtwarzanie
        {
            StopMusic();
        }
    }

    private void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}