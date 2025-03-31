using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

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
    private static bool isInitialized = false;

    void Awake()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            DontDestroyOnLoad(gameObject); // Upewniamy siê, ¿e skrypt nie zostanie zduplikowany
        }
        else
        {
            Destroy(gameObject); // Jeœli instancja ju¿ istnieje, usuñ now¹
            return;
        }
    }

    void Start()
    {
        // Zapobieganie duplikacji obiektów
        EnsureSingleton(ref logo1);
        EnsureSingleton(ref logo2);
        EnsureSingleton(ref musicDisplay);
        ShowMenu(true);
        fwdButton.onClick.AddListener(() => LoadScene("1"));
        rwdButton.onClick.AddListener(() => LoadScene("2"));
        awdButton.onClick.AddListener(() => LoadScene("3"));
        fourWdButton.onClick.AddListener(() => LoadScene("4"));
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Zapewnia, ¿e obiekt istnieje tylko raz i nie dubluje siê po powrocie
    private void EnsureSingleton(ref GameObject obj)
    {
        if (obj != null)
        {
            GameObject existing = GameObject.Find(obj.name);
            if (existing == null)
            {
                DontDestroyOnLoad(obj);
            }
            else if (existing != obj) // Jeœli istnieje duplikat, usuñ nowy obiekt
            {
                Destroy(obj);
                obj = existing;
            }
        }
    }

    // £adowanie sceny asynchronicznie, aby unikn¹æ zaciêæ
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        ShowMenu(false); 

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    public void OpenSettings()
    {
        Debug.Log("Opening settings...");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    // Obs³uga powrotu z wyœcigu
    public void SetReturningFromRace(bool value)
    {
        isReturningFromRace = value;
    }

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

    // Obs³uga za³adowania sceny
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "1") // Jeœli wracamy do menu
        {
            Debug.Log("Powrót do menu, resetowanie UI");
            isReturningFromRace = false; // Reset flagi
            ShowMenu(true); // Przywrócenie widocznoœci menu

            // Sprawdzenie, czy istniej¹ modele aut i ich odœwie¿enie
            RotateCarModel[] carModels = FindObjectsOfType<RotateCarModel>();
            foreach (RotateCarModel carModel in carModels)
            {
                carModel.OnResumeFromMenu();
            }
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}