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
        // Ustawienie logo i muzyki, aby by�y niezniszczalne przy przej�ciu do nowych scen
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

        // Przypisanie funkcji do przycisk�w
        fwdButton.onClick.AddListener(PlayFWD);
        rwdButton.onClick.AddListener(PlayRWD);
        awdButton.onClick.AddListener(PlayAWD);
        fourWdButton.onClick.AddListener(Play4WD);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);

        // Obs�uga powrotu z wy�cigu (je�li dotyczy)
        if (isReturningFromRace)
        {
            ShowMenu(true);
        }

        // Rejestracja metody na zdarzenie za�adowania sceny
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Metody dla przycisk�w - �adowanie odpowiednich scen
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

    // Otwieranie ustawie� (na razie tylko debug)
    public void OpenSettings()
    {
        Debug.Log("Opening settings...");
    }

    // Zamykanie aplikacji
    public void QuitGame()
    {
        Application.Quit();
    }

    // Funkcja ustawiaj�ca, czy wracamy z wy�cigu
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

    // W��cza/wy��cza widoczno�� menu
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

    // Metoda uruchamiana po za�adowaniu sceny 1 (menu)
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Je�li za�adowana scena to menu (scena 1)
        if (scene.name == "1")
        {
            ShowMenu(true); // Poka� menu po powrocie do sceny 1
        }
    }

    // Usuwanie rejestracji metody przy zamkni�ciu
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}