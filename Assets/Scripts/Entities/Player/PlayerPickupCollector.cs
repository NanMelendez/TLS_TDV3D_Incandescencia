using UnityEngine;

public class PlayerPickupCollector : MonoBehaviour, IInteractable
{
    private CollectibleData piece = null;

    public CollectibleType Type => piece.type;
    public bool HoldingPiece => piece != null;
    public CollectibleData HeldPiece => piece;

    public bool TryPickUp(CollectibleData data)
    {
        if (HoldingPiece)
        {
            Debug.Log("Ya se está cargando con una pieza");
            return false;
        }

        piece = data;
        return true;
    }

    public CollectibleData Consume()
    {
        CollectibleData temp = piece;
        piece = null;
        return temp;
    }

    public void Interact()
    {
        // ...
    }
}
