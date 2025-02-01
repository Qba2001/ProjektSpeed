using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class RaceMusicDisplay : MonoBehaviour
{
    public string musicFolderPath = "RaceMusic"; // �cie�ka do folderu z muzyk�, relatywna do Assets
    public GameObject musicTitleObject; // Referencja do GameObject z tekstem

    private List<AudioClip> musicClips = new List<AudioClip>(); // Lista utwor�w
    private AudioSource audioSource; // Referencja do komponentu AudioSource
    private int currentTrackIndex = 0;
    private bool isPlaying = false;
    private Text musicTitleText; // Referencja do komponentu Text wewn�trz GameObject

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false; // Ustawienie na false, aby nie zap�tla� utwor�w

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
                clip.name = Path.GetFileNameWithoutExtension(filePath);
                musicClips.Add(clip);
                Debug.Log("Za�adowano utw�r: " + clip.name);

                // Automatycznie odtw�rz pierwszy za�adowany utw�r
                if (musicClips.Count == 1)
                {
                    PlayMusic();
                }
            }
            else
            {
                Debug.LogError("B��d podczas �adowania utworu: " + request.error);
            }
        }
    }

    public void PlayMusic()
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
        isPlaying = true;

        UpdateMusicTitle();
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        isPlaying = false;
    }

    public void PlayNextTrack()
    {
        if (musicClips.Count == 0)
        {
            Debug.LogError("Brak utwor�w do odtworzenia.");
            return;
        }

        currentTrackIndex = (currentTrackIndex + 1) % musicClips.Count;
        PlayMusic();
    }

    public void PlayPreviousTrack()
    {
        if (musicClips.Count == 0)
        {
            Debug.LogError("Brak utwor�w do odtworzenia.");
            return;
        }

        currentTrackIndex = (currentTrackIndex - 1 + musicClips.Count) % musicClips.Count;
        PlayMusic();
    }

    public string GetCurrentTrackName()
    {
        if (musicClips.Count > 0)
        {
            return musicClips[currentTrackIndex].name;
        }
        return "";
    }

    private void UpdateMusicTitle()
    {
        if (musicTitleText != null)
        {
            musicTitleText.text = audioSource.clip.name;
        }
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }
}