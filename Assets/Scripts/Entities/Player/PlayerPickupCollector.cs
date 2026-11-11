using UnityEngine;

public class PlayerPickupCollector : MonoBehaviour, IInteractable
{
    private CollectibleData piece = null;

    public CollectibleType Type => piece.type;
    public bool HoldingPiece => piece != null;
    public CollectibleData HeldPiece => piece;
    [SerializeField] private GameplayUIManager guim;

    public bool TryPickUp(CollectibleData data)
    {
        if (HoldingPiece)
        {
            if (guim)
                guim.HandleMessageText("Ya tengo una pieza a la mano...");
            return false;
        }

        piece = data;
        if (guim)
            guim.HandleCollectedImg(true, piece.sprite);
        return true;
    }

    public CollectibleData Consume()
    {
        CollectibleData temp = piece;
        if (guim)
            guim.HandleCollectedImg(false);
        piece = null;
        return temp;
    }

    public void Interact()
    {
        // ...
    }
}
