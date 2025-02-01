using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // Initialization or other settings can be added here if needed
    private void Start()
    {
        // Any initialization code that doesn't involve LocalizationSettings
    }
}