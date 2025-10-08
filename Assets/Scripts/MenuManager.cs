using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MiniGame");
        Time.timeScale = 1f;
    }
    
    public void QuitGame()
    {
        Debug.Log("Le jeu est fermé !"); // Utile pour tester dans l’éditeur
        Application.Quit();
    }
}