using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class SpinningSword : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float orbitRadius = 2f;
    [SerializeField] private float orbitSpeed = 180f; // degrés par seconde
    [SerializeField] private float verticalOffset = 1f;
    [SerializeField] private bool spinSelf = true;

    private float angle;

    void Reset()
    {
        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    void Update()
    {
        if (player == null) return;

        angle += orbitSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad)) * orbitRadius;
        transform.position = player.position + offset + Vector3.up * verticalOffset;

        if (spinSelf)
            transform.Rotate(Vector3.up, orbitSpeed * Time.deltaTime, Space.Self);
    }

    // Méthode publique utilisée par PlayerController — corrige "Cannot resolve symbol 'SetPlayer'"
    public void SetPlayer(Transform p)
    {
        player = p;
        // reset angle pour positionner proprement l'épée autour du joueur immédiatement
        angle = 0f;
        if (player != null)
        {
            Vector3 offset = new Vector3(0f, 0f, orbitRadius);
            transform.position = player.position + offset + Vector3.up * verticalOffset;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        if (player != null && other.gameObject == player.gameObject) return;
        if (other.gameObject == gameObject) return;

        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}