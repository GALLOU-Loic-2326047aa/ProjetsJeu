using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Chest : MonoBehaviour
{
    [Header("Clé")]
    [Tooltip("Clé requise pour ouvrir le coffre (vide = coffre libre)")]
    [SerializeField] private string requiredKeyId;

    [Header("Ouverture")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject lid; // couvercle (optionnel)

    private bool isOpen = false;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen) return;

        // On vérifie que c'est le joueur
        if (!other.TryGetComponent<PlayerController>(out var player))
            return;

        // Vérification de la clé (COMME LE TP)
        if (!string.IsNullOrEmpty(requiredKeyId) && !player.HasKey(requiredKeyId))
        {
            Debug.Log("Coffre verrouillé (clé requise : " + requiredKeyId + ")");
            return;
        }

        Open();
    }

    private void Open()
    {
        isOpen = true;

        Debug.Log("Coffre ouvert");

        // Animation
        if (animator != null)
            animator.SetTrigger("Open");

        // Ou ouverture simple
        if (lid != null)
            Destroy(lid);
    }
}
