using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class MusicManager : MonoBehaviour
{
    public string musicFolderPath = "MenuMusic"; // �cie�ka do folderu z muzyk�, relatywna do Assets
    public TextMeshProUGUI musicTitleText; // Referencja do TextMeshProUGUI

    private List<AudioClip> musicClips = new List<AudioClip>(); // Lista utwor�w
    private AudioSource audioSource; // Referencja do komponentu AudioSource
    private int currentTrackIndex = 0;
    private bool isInMainMenu = true; // Flaga wskazuj�ca, czy jeste�my w menu g��wnym

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false; // Ustawienie na false, aby nie zap�tla� utwor�w

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
            Debug.LogError("Folder z muzyk� nie istnieje: " + path);
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
                    Debug.Log("Za�adowano utw�r: " + clip.name);

                    // Automatycznie odtw�rz pierwszy za�adowany utw�r
                    if (musicClips.Count == 1)
                    {
                        PlayMusic();
                    }
                }
            }
            else
            {
                Debug.LogError("B��d podczas �adowania utworu: " + request.error);
            }
        }
    }

    private void PlayMusic()
    {
        if (musicClips.Count == 0)
        {
            Debug.LogError("Brak utwor�w do odtworzenia.");
            return;
        }

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.clip = musicClips[currentTrackIndex];
        audioSource.Play();

        // Aktualizuj tytu� piosenki w interfejsie u�ytkownika tylko w menu g��wnym
        UpdateMusicTitle();
    }

    private void UpdateMusicTitle()
    {
        if (isInMainMenu && musicTitleText != null)
        {
            musicTitleText.text = audioSource.clip.name;
        }
    }

    // Metoda do odtwarzania nast�pnego utworu
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
        // Sprawd�, czy utw�r si� sko�czy� i odtw�rz nast�pny
        if (!audioSource.isPlaying && musicClips.Count > 0)
        {
            PlayNextTrack();
        }
    }

    // Metoda ustawiaj�ca, �e jeste�my w menu g��wnym
    public void SetInMainMenu(bool value)
    {
        isInMainMenu = value;

        // Je�li jeste�my w menu g��wnym, zatrzymaj odtwarzanie i ustaw pierwszy utw�r
        if (isInMainMenu)
        {
            StopMusic();
            currentTrackIndex = 0;
            PlayMusic();
        }
        else // Je�li nie jeste�my w menu g��wnym, zatrzymaj odtwarzanie
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