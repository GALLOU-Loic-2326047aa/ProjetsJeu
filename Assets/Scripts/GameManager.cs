using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Victoire")]
    [SerializeField] private GameTimer gameTimer;

    [Header("UI Victoire")]
    [SerializeField] private GameObject victoryUI;           
    [SerializeField] private TMP_Text timeText;        
    [SerializeField] private TMP_InputField nameInput; 

    [Header("API")]
    [SerializeField] private string apiBaseUrl = "https://api.jeu.gallou-online.fr";
    [SerializeField] private MenuManager menuManager;

    


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
        if (victoryUI != null)
            victoryUI.SetActive(false);
    }

    public void WinGame()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (hasWon) return;
        hasWon = true;

        if (gameTimer != null)
        {
            gameTimer.StopTimer();

            if (timeText != null)
                timeText.text = "Temps : " + gameTimer.GetTimeString();
        }

        // Affiche le Canvas Victoire
        if (victoryUI != null)
            victoryUI.SetActive(true);

        Debug.Log("Victoire !");
    }

    public void SubmitScore()
    {
        string playerName = nameInput.text;
        float time = gameTimer.GetTime();

        StartCoroutine(SendScore(playerName, time));
    }

    private IEnumerator SendScore(string playerName, float time)
    {
        string json = JsonUtility.ToJson(new PlayerScore { player_name = playerName, time = time });

        UnityWebRequest request = new UnityWebRequest(apiBaseUrl + "/score", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            Debug.LogError("Erreur POST score: " + request.error);
        else
            Debug.Log("Score envoyé avec succès !");
        menuManager.MainMenu();
    }

    public void LoadDeathMenu()
    {
        if(menuManager != null)
            menuManager.DeathMenu();
    }
}


