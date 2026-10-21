using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private CharacterController charController;
	[SerializeField] private InputActionReference movementAction;
	[Min(0.0f)] public float moveSpeed = 5.0f;

	private Vector3 moveDir = Vector2.zero;

	private void OnEnable()
	{
		movementAction.action.Enable();
	}

	private void OnDisable()
	{
		movementAction.action.Disable();
	}

	private void Update()
	{
		moveDir = movementAction ? movementAction.action.ReadValue<Vector2>() : Vector2.zero;
	}

	private void FixedUpdate()
	{
		Vector3 finalDir = (transform.forward * moveDir.y) + (transform.right * moveDir.x);

		charController.Move(moveSpeed * Time.deltaTime * finalDir);
	}
}
