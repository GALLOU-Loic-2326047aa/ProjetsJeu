using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    void Start()
    {
        // Met le temps du jeu en pause
        Time.timeScale = 0f;
        Debug.Log("Le jeu est en pause au lancement !");
    }
}