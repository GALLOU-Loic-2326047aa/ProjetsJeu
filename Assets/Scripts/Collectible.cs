using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Collectible : MonoBehaviour
{
    [SerializeField] private CollectibleData data;
    [SerializeField] private bool destroyOnCollect = true;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;

        if (data == null)
        {
            Debug.LogError($"CollectibleData manquant sur l'objet {gameObject.name}. Veuillez assigner un CollectibleData dans l'inspecteur.", this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (data == null) return;

        if (other.TryGetComponent<PlayerController>(out var player))
        {
            player.HandleCollectible(data);
            if (destroyOnCollect) Destroy(gameObject);
            else gameObject.SetActive(false);
        }
    }
}