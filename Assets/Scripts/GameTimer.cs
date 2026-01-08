using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private float timeElapsed;
    private bool isRunning;

    void Start()
    {
        timeElapsed = 0f;
        isRunning = true;
    }

    void Update()
    {
        if (!isRunning) return;

        timeElapsed += Time.deltaTime;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        float seconds = timeElapsed % 60;

        timerText.text =
            minutes.ToString("00") + ":" + seconds.ToString("00.00");
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetFinalTime()
    {
        return timeElapsed;
    }
}
