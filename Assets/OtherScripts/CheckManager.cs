using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LapComplete : MonoBehaviour
{
    public GameObject LapCompleteTrig;
    public GameObject HalfLapTrig;
    public GameObject MinuteDisplay;
    public GameObject SecondDisplay;
    public GameObject MilliDisplay;
    public GameObject LapCounter;
    public GameObject uiElement1;
    public GameObject uiElement2;

    public int lapsToFinish = 0;
    private int lapsDone = 0;
    private float bestLapTime = float.MaxValue;

    void OnTriggerEnter()
    {
        lapsDone++;

        float totalTime = LapTimeManager.MinuteCount * 60 + LapTimeManager.SecondCount + LapTimeManager.MilliCount / 1000f;

        if (totalTime < bestLapTime)
        {
            bestLapTime = totalTime;
        }

        DisplayTime(bestLapTime, MinuteDisplay, SecondDisplay, MilliDisplay);

        LapTimeManager.MinuteCount = 0;
        LapTimeManager.SecondCount = 0;
        LapTimeManager.MilliCount = 0;

        if (lapsDone >= lapsToFinish)
        {
            DisableUIElement(uiElement1);
            DisableUIElement(uiElement2);
            Invoke("TransitionToNextScene", 2f);
        }
        else
        {
            HalfLapTrig.SetActive(true);
            LapCompleteTrig.SetActive(false);
        }
    }

    void DisplayTime(float time, GameObject minuteDisplay, GameObject secondDisplay, GameObject milliDisplay)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);

        LapCounter.GetComponent<Text>().text = "" + lapsDone;
        minuteDisplay.GetComponent<Text>().text = (minutes < 10 ? "0" : "") + minutes + ":";
        secondDisplay.GetComponent<Text>().text = (seconds < 10 ? "0" : "") + seconds + ".";
        milliDisplay.GetComponent<Text>().text = (milliseconds / 100).ToString();
    }

    void DisableUIElement(GameObject element)
    {
        if (element != null)
        {
            element.SetActive(false);
        }
    }

    void TransitionToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}