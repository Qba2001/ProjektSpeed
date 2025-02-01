using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class RandomMenuMusicPlayer : MonoBehaviour
{
    public static RandomMenuMusicPlayer Instance;

    public string menuMusicFolder = "MenuMusic";
    public float musicVolume = 1.0f;

    private List<string> menuMusicFiles = new List<string>();
    private List<string> shuffledMusicFiles = new List<string>(); // Lista przetasowanych utworów
    private int currentTrackIndex = -1;
    private AudioSource audioSource;

    void Awake()
    {
        // Implementacja wzorca Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Nie usuwaj tego obiektu podczas zmiany sceny
        }
        else
        {
            Destroy(gameObject); // Zniszcz ten obiekt, jeœli istnieje ju¿ inna instancja
        }
    }

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = musicVolume;

        // Za³aduj muzykê menu na start
        LoadMusic(menuMusicFolder, menuMusicFiles);

        // Zarejestruj metodê do obs³ugi zmiany sceny
        SceneManager.sceneLoaded += OnSceneLoaded;
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
                // Skopiuj oryginaln¹ listê do listy przetasowanej
                shuffledMusicFiles.AddRange(musicList);
                ShuffleMusicFiles(shuffledMusicFiles);

                // Rozpocznij odtwarzanie pierwszego utworu z przetasowanej listy
                PlayNextTrack();
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

    public void PlayNextTrack()
    {
        if (shuffledMusicFiles.Count == 0)
        {
            Debug.LogError("No menu music files loaded.");
            return;
        }

        currentTrackIndex = (currentTrackIndex + 1) % shuffledMusicFiles.Count;
        StartCoroutine(PlayTrack(shuffledMusicFiles[currentTrackIndex]));
    }

    IEnumerator PlayTrack(string path)
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

                Debug.Log("Now playing: " + clip.name);
            }
        }
    }

    public string GetCurrentTrackName()
    {
        if (currentTrackIndex >= 0 && currentTrackIndex < shuffledMusicFiles.Count)
        {
            string fileName = Path.GetFileNameWithoutExtension(shuffledMusicFiles[currentTrackIndex]);
            return fileName;
        }
        else
        {
            return "Brak utworu"; // Mo¿na zwróciæ domyœln¹ wartoœæ, jeœli nie ma odtwarzanego utworu
        }
    }

    public void StopMusic()
    {
        // Ustaw g³oœnoœæ na zero i zatrzymaj odtwarzanie muzyki
        audioSource.volume = 0;
        audioSource.Stop();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Zatrzymaj odtwarzanie muzyki po zmianie sceny
        StopMusic();
    }

    void OnDestroy()
    {
        // Upewnij siê, ¿e wyrejestrowujemy metodê z obs³ugi zdarzenia po zniszczeniu obiektu
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}