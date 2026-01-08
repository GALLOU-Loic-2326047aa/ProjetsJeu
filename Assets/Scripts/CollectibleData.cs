using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleData", menuName = "Game/CollectibleData")]
public class CollectibleData : ScriptableObject
{
    public CollectibleType type;

    [Header("Valeur")]
    public int value = 1;

    [Header("Couleur")]
    public Color color = Color.white;

    [Header("Clé (utilisé seulement si type = Clef)")]
    [Tooltip("Identifiant unique de la clé (ex: TeleporteurRouge)")]
    public string keyId;
}


