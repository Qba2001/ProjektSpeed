using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject logo1;
    public GameObject logo2;
    public GameObject musicDisplay;

    public Button fwdButton;
    public Button rwdButton;
    public Button awdButton;
    public Button fourWdButton;
    public Button settingsButton;
    public Button quitButton;

    private bool isReturningFromRace = false;

    void Start()
    {
        // Ustawienie logo i muzyki, aby by³y niezniszczalne przy przejœciu do nowych scen
        logo1.SetActive(true);
        logo2.SetActive(true);

        DontDestroyOnLoad(logo1);
        DontDestroyOnLoad(logo2);

        if (musicDisplay != null)
        {
            DontDestroyOnLoad(musicDisplay);
        }

        // Inicjalizacja UI
        ShowMenu(true);

        // Przypisanie funkcji do przycisków
        fwdButton.onClick.AddListener(PlayFWD);
        rwdButton.onClick.AddListener(PlayRWD);
        awdButton.onClick.AddListener(PlayAWD);
        fourWdButton.onClick.AddListener(Play4WD);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);

        // Obs³uga powrotu z wyœcigu (jeœli dotyczy)
        if (isReturningFromRace)
        {
            ShowMenu(true);
        }

        // Rejestracja metody na zdarzenie za³adowania sceny
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Metody dla przycisków - ³adowanie odpowiednich scen
    public void PlayFWD()
    {
        ShowMenu(false);
        SceneManager.LoadScene("1"); // Scena 2
    }

    public void PlayRWD()
    {
        ShowMenu(false);
        SceneManager.LoadScene("2"); // Scena 3
    }

    public void PlayAWD()
    {
        ShowMenu(false);
        SceneManager.LoadScene("3"); // Scena 4
    }

    public void Play4WD()
    {
        ShowMenu(false);
        SceneManager.LoadScene("4"); // Scena 5
    }

    // Otwieranie ustawieñ (na razie tylko debug)
    public void OpenSettings()
    {
        Debug.Log("Opening settings...");
    }

    // Zamykanie aplikacji
    public void QuitGame()
    {
        Application.Quit();
    }

    // Funkcja ustawiaj¹ca, czy wracamy z wyœcigu
    public void SetReturningFromRace(bool value)
    {
        isReturningFromRace = value;
        if (isReturningFromRace)
        {
            ShowMenu(true);
            RotateCarModel[] carModels = FindObjectsOfType<RotateCarModel>();
            foreach (RotateCarModel carModel in carModels)
            {
                carModel.OnResumeFromMenu();
            }
        }
    }

    // W³¹cza/wy³¹cza widocznoœæ menu
    private void ShowMenu(bool show)
    {
        fwdButton.gameObject.SetActive(show);
        rwdButton.gameObject.SetActive(show);
        awdButton.gameObject.SetActive(show);
        fourWdButton.gameObject.SetActive(show);
        settingsButton.gameObject.SetActive(show);
        quitButton.gameObject.SetActive(show);

        if (musicDisplay != null)
        {
            musicDisplay.SetActive(show);
        }

        logo1.SetActive(show);
        logo2.SetActive(show);
    }

    // Metoda uruchamiana po za³adowaniu sceny 1 (menu)
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Jeœli za³adowana scena to menu (scena 1)
        if (scene.name == "1")
        {
            ShowMenu(true); // Poka¿ menu po powrocie do sceny 1
        }
    }

    // Usuwanie rejestracji metody przy zamkniêciu
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}