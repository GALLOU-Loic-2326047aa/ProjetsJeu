using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider))]
public class Coin : MonoBehaviour
{
    [Header("Valeur de la pièce")]
    [SerializeField] private int value = 1;

    [Header("Référence UI")]
    [SerializeField] private TextMeshProUGUI counterText;

    [Header("Options")]
    [SerializeField] private bool destroyOnCollect = true;

    private static int totalCoins = 0; // compteur global, peut être modifié selon le joueur

    private void Awake()
    {
        // S'assurer que le collider est trigger
        var col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;

        // Mettre le compteur initial
        UpdateCounterText();
    }

    private void OnTriggerEnter(Collider other)
    {
        // On ne prend que le joueur
        if (!other.TryGetComponent<PlayerController>(out var player))
            return;

        // Ajouter les pièces
        totalCoins += value;
        UpdateCounterText();

        // Détruire ou désactiver la pièce
        if (destroyOnCollect)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }

    private void UpdateCounterText()
    {
        if (counterText != null)
            counterText.text = "Pièces : " + totalCoins;
    }

    // Pour remettre à zéro si nécessaire (ex: nouvelle partie)
    public static void ResetCounter()
    {
        totalCoins = 0;
    }
}
