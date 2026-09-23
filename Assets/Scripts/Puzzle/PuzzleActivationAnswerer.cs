using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleActivationAnswerer : BasePuzzleComponent
{
    [SerializeField] private List<BasePuzzleComponent> puzzleActivators;

    private bool previousActivationState;

    private void Awake()
    {
        previousActivationState = isActive;
    }

    private void Update()
    {
        previousActivationState = isActive;
        isActive = puzzleActivators.All(activator => activator.ComponentIsActive);

        if (previousActivationState != isActive)
        {
            if (isActive)
                Activate();
            else
                Deactivate();
        }
    }

    public override void Activate()
    {
        // Activation animation
    }

    protected override void Deactivate()
    {
        // Deactivation animation
    }
}
