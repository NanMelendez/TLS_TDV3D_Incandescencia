using UnityEngine;

public class PuzzleLaserReceiver : PuzzleComponent
{
    private bool isHitThisFrame = false;

    private void LateUpdate()
    {
        if (isHitThisFrame)
        {
            if (!isActive)
            {
                AudioManager.Instance.SFXActivationBeamReceiver();
                Activate();
            }
        }
        else
        {
            if (isActive)
            {
                AudioManager.Instance.SFXDeactivation();
                Deactivate();
            }
        }
        isHitThisFrame = false;
    }

    public void OnLaserHit()
    {
        isHitThisFrame = true;
    }
}
