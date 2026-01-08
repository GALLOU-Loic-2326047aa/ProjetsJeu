using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Teleporter : MonoBehaviour
{
    [Header("Lien")]
    public Teleporter linkedTeleporter;
    public Transform exitPoint;

    [Header("Clé")]
    [Tooltip("Clé requise pour activer ce téléporteur (vide = toujours actif)")]
    public string requiredKeyId;

    [Header("Options")]
    public float cooldown = 0.5f;
    public bool preserveVelocity = true;
    public bool matchRotation = true;

    private HashSet<GameObject> ignored = new HashSet<GameObject>();

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (linkedTeleporter == null) return;
        if (ignored.Contains(other.gameObject)) return;

        // Seulement le joueur
        if (!other.TryGetComponent<PlayerController>(out var player))
            return;

        // Vérification de la clé
        if (!string.IsNullOrEmpty(requiredKeyId) && !player.HasKey(requiredKeyId))
        {
            Debug.Log("Téléporteur verrouillé (clé requise : " + requiredKeyId + ")");
            return;
        }

        Teleport(other);
    }

    private void Teleport(Collider other)
    {
        Vector3 destPos = linkedTeleporter.exitPoint != null
            ? linkedTeleporter.exitPoint.position
            : linkedTeleporter.transform.position;

        Quaternion destRot = linkedTeleporter.transform.rotation;

        Rigidbody rb = other.attachedRigidbody;

        if (rb != null)
        {
            Vector3 oldVelocity = rb.velocity;
            rb.position = destPos;
            rb.rotation = matchRotation ? destRot : rb.rotation;

            if (preserveVelocity)
            {
                Quaternion deltaRot =
                    linkedTeleporter.transform.rotation *
                    Quaternion.Inverse(transform.rotation);

                rb.velocity = deltaRot * oldVelocity;
            }
            else rb.velocity = Vector3.zero;
        }
        else
        {
            other.transform.position = destPos;
            if (matchRotation)
                other.transform.rotation = destRot;
        }

        linkedTeleporter.AddIgnoredTemporary(other.gameObject, cooldown);
        AddIgnoredTemporary(other.gameObject, cooldown);
    }

    public void AddIgnoredTemporary(GameObject obj, float duration)
    {
        if (ignored.Contains(obj)) return;
        ignored.Add(obj);
        StartCoroutine(RemoveAfterDelay(obj, duration));
    }

    private IEnumerator RemoveAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        ignored.Remove(obj);
    }
}
