using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("LeaderBoard");
        Time.timeScale = 1f;
    }
    
    public void PlayGame2()
    {
        SceneManager.LoadScene("MiniGameloic");
        Time.timeScale = 1f;
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