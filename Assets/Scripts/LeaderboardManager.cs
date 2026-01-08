using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    public string apiBaseUrl = "https://api.jeu.gallou-online.fr";

    public Transform content;
    public GameObject leaderboardItemPrefab;

    void OnEnable()
    {
        StartCoroutine(GetLeaderboard());
    }

    IEnumerator GetLeaderboard()
    {
        UnityWebRequest request =
            UnityWebRequest.Get(apiBaseUrl + "/leaderboard");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            yield break;
        }

        string json = request.downloadHandler.text;

        json = "{\"leaderboard\":" + json + "}";

        LeaderboardResponse response =
            JsonUtility.FromJson<LeaderboardResponse>(json);

        Display(response.leaderboard);
    }

    void Display(PlayerScore[] scores)
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);

        for (int i = 0; i < scores.Length; i++)
        {
            GameObject item =
                Instantiate(leaderboardItemPrefab, content);

            item.transform.Find("RankText")
                .GetComponent<TextMeshProUGUI>()
                .text = (i + 1).ToString();

            item.transform.Find("NameText")
                .GetComponent<TextMeshProUGUI>()
                .text = scores[i].player_name;

            item.transform.Find("ScoreText")
                .GetComponent<TextMeshProUGUI>()
                .text = scores[i].time.ToString("0.00") + " s";
        }
    }
}
