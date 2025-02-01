using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class RandomMusicPlayer : MonoBehaviour
{
    public string raceMusicFolder = "RaceMusic";
    public float musicVolume = 1.0f;

    private List<string> raceMusicFiles = new List<string>();
    private int currentRaceTrackIndex = -1;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = musicVolume;

        // Za�aduj muzyk� wy�cigow� na start
        LoadMusic(raceMusicFolder, raceMusicFiles);
    }

    void LoadMusic(string folder, List<string> musicList)
    {
        string path = Path.Combine(Application.streamingAssetsPath, folder);

        if (Directory.Exists(path))
        {
            string[] files = Directory.GetFiles(path, "*.mp3");
            musicList.AddRange(files);

            if (musicList.Count > 0)
            {
                ShuffleMusicFiles(musicList);
                PlayNextRaceTrack(); // Rozpocznij odtwarzanie pierwszego utworu z muzyki wy�cigowej
            }
            else
            {
                Debug.LogError("No music files found in: " + path);
            }
        }
        else
        {
            Debug.LogError("Music folder not found: " + path);
        }
    }

    void ShuffleMusicFiles(List<string> musicList)
    {
        for (int i = 0; i < musicList.Count; i++)
        {
            string temp = musicList[i];
            int randomIndex = Random.Range(i, musicList.Count);
            musicList[i] = musicList[randomIndex];
            musicList[randomIndex] = temp;
        }
    }

    public void PlayNextRaceTrack()
    {
        if (raceMusicFiles.Count == 0)
        {
            Debug.LogError("No race music files loaded.");
            return;
        }

        currentRaceTrackIndex = (currentRaceTrackIndex + 1) % raceMusicFiles.Count;
        StartCoroutine(PlayTrack(raceMusicFiles[currentRaceTrackIndex], raceMusicFolder));
    }

    IEnumerator PlayTrack(string path, string folder)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///" + path, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error downloading audio: " + www.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = clip;
                audioSource.Play();

                // Testowe wy�wietlenie nazwy utworu w konsoli
                Debug.Log("Now playing: " + clip.name);
            }
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    void Update()
    {
        // Sprawd�, czy muzyka si� sko�czy�a i odtw�rz nast�pny utw�r z muzyki wy�cigowej
        if (!audioSource.isPlaying && raceMusicFiles.Count > 0)
        {
            PlayNextRaceTrack();
        }
    }
}