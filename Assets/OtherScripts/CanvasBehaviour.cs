using UnityEngine;

public class ResponsiveCanvas : MonoBehaviour
{
    public RectTransform[] uiElements; // Tablica przechowuj¹ca wszystkie elementy UI

    private Vector2 originalResolution;

    void Start()
    {
        // Przechowaj oryginaln¹ rozdzielczoœæ
        originalResolution = new Vector2(Screen.width, Screen.height);

        // Dostosuj pozycje elementów
        foreach (RectTransform element in uiElements)
        {
            AdjustElement(element);
        }
    }

    void Update()
    {
        // SprawdŸ, czy rozdzielczoœæ siê zmieni³a
        if (Screen.width != originalResolution.x || Screen.height != originalResolution.y)
        {
            // Aktualizuj oryginaln¹ rozdzielczoœæ
            originalResolution = new Vector2(Screen.width, Screen.height);

            // Dostosuj pozycje elementów
            foreach (RectTransform element in uiElements)
            {
                AdjustElement(element);
            }
        }
    }

    private void AdjustElement(RectTransform element)
    {
        // Przechowaj oryginalne ustawienia
        Vector2 originalAnchorMin = element.anchorMin;
        Vector2 originalAnchorMax = element.anchorMax;
        Vector2 originalPivot = element.pivot;
        Vector2 originalSizeDelta = element.sizeDelta;
        Vector2 originalPosition = element.anchoredPosition;

        // Ustawienie kotwic i pivotu na œrodek
        element.anchorMin = new Vector2(0.5f, 0.5f);
        element.anchorMax = new Vector2(0.5f, 0.5f);
        element.pivot = new Vector2(0.5f, 0.5f);

        // Dopasowanie rozmiaru elementu do rozmiaru ekranu
        element.sizeDelta = new Vector2(Screen.width, Screen.height);

        // Dostosowanie pozycji do aktualnej rozdzielczoœci
        float scaleFactorX = Screen.width / originalResolution.x;
        float scaleFactorY = Screen.height / originalResolution.y;
        element.anchoredPosition = new Vector2(originalPosition.x * scaleFactorX, originalPosition.y * scaleFactorY);

        // Przywróæ oryginalne ustawienia
        element.anchorMin = originalAnchorMin;
        element.anchorMax = originalAnchorMax;
        element.pivot = originalPivot;
        element.sizeDelta = originalSizeDelta;
    }
}