using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

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

        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00.00}";
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetTime() => timeElapsed;

    public string GetTimeString()
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        return $"{minutes:00}:{seconds:00}";
    }
}
