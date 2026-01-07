using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TestAPI : MonoBehaviour
{
    public string apiBaseUrl = "https://api.jeu.gallou-online.fr"; // change avec ton API

    private void Start()
    {
        // Test GET leaderboard
        StartCoroutine(GetLeaderboard());

        // Test POST score (facultatif)
        //StartCoroutine(SendScore("Loic", 42.5f));
    }

    private IEnumerator GetLeaderboard()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiBaseUrl + "/leaderboard");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erreur GET leaderboard: " + request.error);
        }
        else
        {
            string json = request.downloadHandler.text;
            Debug.Log("Leaderboard JSON reçu : " + json);
        }
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
        {
            Debug.LogError("Erreur POST score: " + request.error);
        }
        else
        {
            Debug.Log("Score envoyé avec succès ! Réponse : " + request.downloadHandler.text);
        }
    }
}

// Classe pour sérialiser le score
[System.Serializable]
public class PlayerScore
{
    public string player_name;
    public float time;
}
