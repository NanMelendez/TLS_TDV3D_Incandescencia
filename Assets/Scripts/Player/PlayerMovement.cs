using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [Min(0.0f)] public float moveSpeed = 1.0f;
    [SerializeField] private Rigidbody rb;

    private Vector2 horizontalDir = Vector2.zero;

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
        horizontalDir = moveAction.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector2 temp = horizontalDir.normalized * moveSpeed;
        rb.linearVelocity = new Vector3(temp.x, rb.linearVelocity.y, temp.y);
    }
}
