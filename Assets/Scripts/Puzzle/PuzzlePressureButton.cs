using UnityEngine;

public class PuzzlePressureButton : BasePuzzleComponent
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private string expectedColliderTag;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(expectedColliderTag))
            Activate();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(expectedColliderTag))
            Deactivate();
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
