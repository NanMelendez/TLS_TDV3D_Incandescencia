using System.Collections.Generic;
using UnityEngine;

public class PuzzleCountAwaiter : BasePuzzleComponent
{
    [SerializeField] private List<GameObject> expectedObjects;

    private int currentCollectedCount = 0;

    private void Awake()
    {
        foreach (GameObject piece in expectedObjects)
            piece.SetActive(false);
    }

    public void ReceivePiece()
    {
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
