using UnityEngine;

public class PauseMenuButtons : MonoBehaviour
{
    public void ResumeGame()
    {
        // Trouve le PauseManager dans la scène du jeu
        PauseManager pauseManager = FindObjectOfType<PauseManager>();

        if (pauseManager != null)
        {
            pauseManager.ResumeGame();
        }
        else
        {
            Debug.LogWarning("PauseManager introuvable !");
        }
    }
}