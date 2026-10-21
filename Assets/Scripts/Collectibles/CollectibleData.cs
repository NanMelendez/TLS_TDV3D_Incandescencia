using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleData", menuName = "Scriptable Objects/CollectibleData")]
public class CollectibleData : ScriptableObject
{
    public CollectibleType type;
    public bool isFake;
}
