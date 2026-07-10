using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public float timeRemaining = 60f;
    public GameObject panelVictoria;
    public AudioSource gameAudio;
    public AudioClip victoryAudio;

    public TMP_Text timerText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimer();
        }
        else if (timeRemaining <= 0)
        {
            ElementSpawner.SetAllVisible(false);
            Cauldron.SetAllVisible(false);
            gameAudio.volume = 0.1f;
            gameAudio.PlayOneShot(victoryAudio);
            panelVictoria.SetActive(true);
        }
        else
        {
            timeRemaining = 0;
        }
    }

    void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
