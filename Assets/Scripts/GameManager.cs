using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Victoire")]
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameTimer gameTimer;

    private bool hasWon = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (winUI != null)
            winUI.SetActive(false);
    }

    public void WinGame()
    {
        if (hasWon) return;

        hasWon = true;

        Debug.Log("Victoire !");

        if (gameTimer != null)
            gameTimer.StopTimer();

        if (winUI != null)
            winUI.SetActive(true);
    }
}
