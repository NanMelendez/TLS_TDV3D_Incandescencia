using UnityEngine;

public class BasePuzzleComponent : MonoBehaviour
{
    protected bool isActive = false;

    public bool ComponentIsActive
    {
        get => isActive;
    }

    public virtual void Activate()
    {
        isActive = true;
    }

    protected virtual void Deactivate()
    {
        isActive = false;
    }
}
