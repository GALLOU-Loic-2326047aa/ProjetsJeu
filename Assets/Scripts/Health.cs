using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public int maxHealth = 200;
    [SerializeField] private bool destroyOnDeath = true;
    [SerializeField] private bool isPlayer = false; // <-- nouveau champ pour différencier joueur et ennemis

    private int currentHealth;

    [Header("Events")]
    public UnityEvent OnDeath;
    public UnityEvent<int> OnHealthChanged;

    private GameManager gameManager;

    void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);

        // GameManager si nécessaire
        gameManager = GameManager.Instance;
        if (gameManager == null)
            Debug.LogWarning("Health: GameManager introuvable !");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;

    private void Die()
    {
        // Déclencher l'événement OnDeath pour tout le monde
        OnDeath?.Invoke();

        // Si c'est le joueur, charger GameOver
        if (isPlayer)
        {
            MenuManager menuManager = FindObjectOfType<MenuManager>();
            if (menuManager != null)
            {
                menuManager.DeathMenu();
            }
            else
            {
                Debug.LogError("MenuManager introuvable dans la scène !");
            }
        }

        // Détruire l'objet si demandé
        if (destroyOnDeath)
            Destroy(gameObject);
    }
}
