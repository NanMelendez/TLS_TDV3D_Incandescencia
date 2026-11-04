using System;
using UnityEngine;

public abstract class PuzzleComponent : MonoBehaviour, IPuzzleActivator
{
    protected bool isActive = false;

    public bool IsActive => isActive;
    public event Action<bool> OnStateChanged;

    protected virtual void Activate()
    {
        SetState(true);
    }

    protected virtual void Deactivate()
    {
        SetState(false);
    }

    protected void SetState(bool newState)
    {
        if (isActive == newState)
            return;

        isActive = newState;
        OnStateChanged?.Invoke(isActive);
    }
}
