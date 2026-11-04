using UnityEngine;

public class PuzzlePieceLogic : MonoBehaviour, IInteractable
{
    [SerializeField] private CollectibleData data;

    public CollectibleType Type => data.type;
    public bool IsFake => data.isFake;

    public void Interact()
    {
        Debug.Log("Interactuando con pieza");

        PlayerPickupCollector collector = FindFirstObjectByType<PlayerPickupCollector>();

        if (collector && collector.TryPickUp(data))
            Destroy(gameObject);
    }
}
