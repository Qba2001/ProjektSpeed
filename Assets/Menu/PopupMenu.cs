using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject popupMenu; 

    private bool isPaused = false;

    void Start()
    {
        if (popupMenu != null)
        {
            popupMenu.SetActive(false); 
        }
        else
        {
            Debug.LogError("Popup Menu is not assigned.");
        }

        Time.timeScale = 1;
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

        Cursor.visible = isPaused; 
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        popupMenu.SetActive(true);
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        popupMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }

    public void QuitGame()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene("1", LoadSceneMode.Single);
    }

    public void Menu()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene("1", LoadSceneMode.Single);
    }
}