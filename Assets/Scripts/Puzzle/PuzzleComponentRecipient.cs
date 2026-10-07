using System.Collections.Generic;
using UnityEngine;

public class PuzzleComponentRecipient : BasePuzzleComponent
{
    [SerializeField] private List<GameObject> expectedObjects;

    private int currentCollectedCount = 0;

    public int CurrentlyCollectedCount
    {
        get => currentCollectedCount;
    }

    public int ExpectedCount
    {
        get => expectedObjects.Count;
    }

    private void Awake()
    {
        foreach (GameObject piece in expectedObjects)
            piece.SetActive(false);

        ColRecipientManager.Instance.SpawnPieces(expectedObjects.Count);
    }

    public void ReceivePiece()
    {
        Debug.Log("Received piece!");

        if (currentCollectedCount < expectedObjects.Count)
        {
            expectedObjects[currentCollectedCount].SetActive(true);

            currentCollectedCount++;

            if (currentCollectedCount == expectedObjects.Count)
                Activate();
        }
    }

    public override void Activate()
    {
        base.Activate();

        // Activation animation
    }

    protected override void Deactivate()
    {
        base.Deactivate();

        // Deactivation animation

    }
}
