using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private InputActionReference moveAction;
	[Min(0.0f)] public float moveSpeed = 1.0f;
	[SerializeField] private Rigidbody rb;

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
		Vector3 finalDir = (transform.forward * moveDir.y) + (transform.right * moveDir.x);

		rb.linearVelocity = new Vector3(finalDir.x * moveSpeed, rb.linearVelocity.y, finalDir.z * moveSpeed);
	}
}
