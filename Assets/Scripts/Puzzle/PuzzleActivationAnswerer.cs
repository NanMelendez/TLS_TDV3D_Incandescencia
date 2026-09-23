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
        if (puzzleActivators.All(activator => activator.ComponentIsActive))
        {
            if (previousActivationState != isActive)
            {

            }
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
