using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject popupMenu; // Referencja do obiektu menu pauzy

    private bool isPaused = false;

    void Start()
    {
        if (popupMenu != null)
        {
            popupMenu.SetActive(false); // Ustaw menu pauzy jako niewidoczne na starcie
        }
        else
        {
            Debug.LogError("Popup Menu is not assigned.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        // Sprawdzamy, czy menu pauzy jest aktywne, aby odpowiednio ustawiaæ widocznoœæ kursora myszy
        if (isPaused)
        {
            Cursor.visible = true; // Kursor jest widoczny, gdy menu pauzy jest otwarte
        }
        else
        {
            Cursor.visible = false; // Kursor jest ukryty, gdy gra jest w trybie normalnym
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        popupMenu.SetActive(true);
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        popupMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }

    public void QuitGame()
    {
        // Wczytaj ponownie scenê "1" z zachowaniem obiektów i skryptów
        SceneManager.LoadScene("1", LoadSceneMode.Single);
    }

    public void Menu()
    {
        // Wczytaj ponownie scenê "1" z zachowaniem obiektów i skryptów
        SceneManager.LoadScene("1", LoadSceneMode.Single);
    }
}