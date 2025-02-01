using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownScript : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public AudioClip countdownSound;
    private AudioSource audioSource;
    private int countdown = 3;
    private bool countdownStarted = false;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (!countdownStarted)
        {
            StartCoroutine(StartCountdown());
            countdownStarted = true;
        }
    }

    IEnumerator StartCountdown()
    {
        PlayCountdownSound();
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }
        countdownText.text = "";
    }

    void PlayCountdownSound()
    {
        if (countdownSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(countdownSound);
        }
    }
}