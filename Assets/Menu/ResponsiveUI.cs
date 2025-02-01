using UnityEngine;

public class ResponsiveUIManager : MonoBehaviour
{
    public RectTransform mainMenuPanel;
    public RectTransform settingsMenuPanel;

    void Start()
    {
        AdjustAnchors(mainMenuPanel);
        AdjustAnchors(settingsMenuPanel);
    }

    private void AdjustAnchors(RectTransform panel)
    {
        panel.anchorMin = new Vector2(0.5f, 0.5f);
        panel.anchorMax = new Vector2(0.5f, 0.5f);
        panel.pivot = new Vector2(0.5f, 0.5f);

        panel.sizeDelta = new Vector2(Screen.width, Screen.height);

        panel.anchoredPosition = Vector2.zero;
    }
}