using UnityEngine;

public class PuzzleLaserReceiver : PuzzleComponent
{
    private bool isHitThisFrame = false;

    private void LateUpdate()
    {
        if (isHitThisFrame)
        {
            if (!isActive)
                Activate();
        }
        else
        {
            if (isActive)
                Deactivate();
        }
        isHitThisFrame = false;
    }

    public void OnLaserHit()
    {
        isHitThisFrame = true;
    }
}
