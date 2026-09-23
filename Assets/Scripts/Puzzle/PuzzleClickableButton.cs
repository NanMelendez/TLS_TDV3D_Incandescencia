using System.Collections;
using UnityEngine;

public class PuzzleClickableButton : BasePuzzleComponent
{
    [SerializeField] private bool willToggleBackAfterTrigger = true;
    [SerializeField] [Min(0.01f)] private float toggleBackTimer = 5.0f;

    private float timer = 0.0f;

    public override void Activate()
    {
        base.Activate();

        timer = toggleBackTimer;
        if (willToggleBackAfterTrigger)
            StartCoroutine(nameof(ToggleBackCountdown));

        // Activation animation
    }

    protected override void Deactivate()
    {
        base.Deactivate();

        // Deactivation animation
    }

    private IEnumerator ToggleBackCountdown()
    {

        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        timer = 0.0f;
        Deactivate();
    }
}
