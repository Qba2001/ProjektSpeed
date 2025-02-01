using System.Collections;
using UnityEngine;
using TMPro;

public class LapTimeManager : MonoBehaviour
{
    public static int MinuteCount;
    public static int SecondCount;
    public static float MilliCount;
    public static string MsDisplay;

    public TextMeshProUGUI MinuteBox;
    public TextMeshProUGUI SecondBox;
    public TextMeshProUGUI MilliBox;

    private bool isCountdownComplete = false;

    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    void Update()
    {
        if (isCountdownComplete)
        {
            MilliCount += Time.deltaTime * 10;
            MsDisplay = MilliCount.ToString("F0");
            MilliBox.text = "" + MsDisplay;

            if (MilliCount >= 10)
            {
                MilliCount = 0;
                SecondCount += 1;
            }

            if (SecondCount <= 9)
            {
                SecondBox.text = "0" + SecondCount + ".";
            }
            else
            {
                SecondBox.text = "" + SecondCount + ".";
            }

            if (SecondCount >= 60)
            {
                SecondCount = 0;
                MinuteCount += 1;
            }

            if (MinuteCount <= 9)
            {
                MinuteBox.text = "0" + MinuteCount + ":";
            }
            else
            {
                MinuteBox.text = "" + MinuteCount + ":";
            }
        }
    }

    IEnumerator StartCountdown()
    {
        int countdown = 3;

        while (countdown > 0)
        {
            Debug.Log("Countdown: " + countdown);
            yield return new WaitForSeconds(1f);
            countdown--;
        }
        isCountdownComplete = true;
        Debug.Log("Countdown complete!");
    }
}