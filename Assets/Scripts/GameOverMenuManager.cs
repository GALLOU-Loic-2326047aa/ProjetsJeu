using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuManager : MonoBehaviour
{
    public void RestartGame()
    {
        // Recharge la scène MiniGameloic pour relancer la partie
        SceneManager.LoadScene("MiniGameloic");
        Time.timeScale = 1f; // Remet le temps normal
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("AcceuilDuJeu");
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Debug.Log("Le jeu est fermé !"); // Utile pour tester dans l’éditeur
        Application.Quit();
    }
}
