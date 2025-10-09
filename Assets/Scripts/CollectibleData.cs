using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleData", menuName = "Game/CollectibleData")]
public class CollectibleData : ScriptableObject
{
    public CollectibleType type;
    public int value = 1;
    public Color color = Color.white;
}