using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Teleporter : MonoBehaviour
{
    [Tooltip("Téléporteur lié")]
    public Teleporter linkedTeleporter;

    [Tooltip("Point d'apparition sur le téléporteur lié (optionnel)")]
    public Transform exitPoint;

    [Tooltip("Durée pendant laquelle l'objet ne peut pas être re-teleporté (secondes)")]
    public float cooldown = 0.5f;

    [Tooltip("Conserver la vélocité du Rigidbody")]
    public bool preserveVelocity = true;

    [Tooltip("Ajuster la rotation du joueur à celle du téléporteur de sortie")]
    public bool matchRotation = true;

    // Objets temporairement ignorés (empêche le rebond)
    private HashSet<GameObject> ignored = new HashSet<GameObject>();

    private void Reset()
    {
        // s'assurer que le collider est trigger
        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (linkedTeleporter == null) return;
        if (ignored.Contains(other.gameObject)) return;

        // Optionnel: filtrer par tag "Player"
        // if (!other.CompareTag("Player")) return;

        // Teleportation
        Vector3 destPos = linkedTeleporter.exitPoint != null ? linkedTeleporter.exitPoint.position : linkedTeleporter.transform.position;
        Quaternion destRot = linkedTeleporter.transform.rotation;

        // Si l'objet a un Rigidbody, manipuler position/velocity via Rigidbody
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            // Calculer transformation de la vélocité si on veut la conserver et ajuster la direction
            Vector3 oldVelocity = rb.velocity;
            rb.position = destPos;
            rb.rotation = matchRotation ? destRot : rb.rotation;

            if (preserveVelocity)
            {
                // Appliquer la rotation relative entre téléporteurs pour orienter la vélocité
                Quaternion deltaRot = linkedTeleporter.transform.rotation * Quaternion.Inverse(transform.rotation);
                rb.velocity = deltaRot * oldVelocity;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            // Sans Rigidbody, déplacer directement le transform
            other.transform.position = destPos;
            if (matchRotation) other.transform.rotation = destRot;
        }

        // Empêcher la ré-teleportation immédiate : ajouter l'objet à la liste d'ignorés du téléporteur de sortie
        linkedTeleporter.AddIgnoredTemporary(other.gameObject, cooldown);
        // Aussi s'ignorer soi-même pour l'objet pendant un court instant pour éviter re-entry si nécessaire
        AddIgnoredTemporary(other.gameObject, cooldown);
    }

    public void AddIgnoredTemporary(GameObject obj, float duration)
    {
        if (obj == null) return;
        if (ignored.Contains(obj)) return;
        ignored.Add(obj);
        StartCoroutine(RemoveAfterDelay(obj, duration));
    }

    private IEnumerator RemoveAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null) ignored.Remove(obj);
    }
}
