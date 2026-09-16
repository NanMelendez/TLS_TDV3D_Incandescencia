using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [Min(0.0f)] public float moveSpeed = 1.0f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform camTransform;

    private Vector2 moveDir = Vector2.zero;

    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
    }

    private void Update()
    {
		moveDir = moveAction.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        /*
        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;
        forward.y = right.y = 0.0f;
        forward.Normalize();
        right.Normalize();

        Vector3 finalDir = (forward * moveDir.y + right * moveDir.x).normalized;

		Vector3 vel = finalDir * moveSpeed;
        */

        Vector3 vel = moveDir * moveSpeed;

		rb.linearVelocity = new Vector3(vel.x, rb.linearVelocity.y, vel.y);
	}
}
