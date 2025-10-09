using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Collectible : MonoBehaviour
{
    [SerializeField] private CollectibleData data;
    [SerializeField] private bool destroyOnCollect = true;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            player.HandleCollectible(data);
            if (destroyOnCollect) Destroy(gameObject);
            else gameObject.SetActive(false);
        }
    }
}