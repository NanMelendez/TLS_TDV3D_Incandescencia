using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
	[SerializeField][Min(0.01f)] private float rayLength = 1.0f;
	[SerializeField] private Transform camTransform;
	[SerializeField] private InputActionReference interactAction;
	[SerializeField] private CinemachineInputAxisController cmiac;
	[SerializeField] private Transform lookAtTransform;
	public float mouseSensitivity = 1.0f;

	private RaycastHit lastHit;
	private bool hasHitSomething;
	private IInteractable currentInteractable;

	private void OnEnable()
	{
		interactAction.action.Enable();
		interactAction.action.performed += OnInteraction;
	}

	private void OnDisable()
	{
		interactAction.action.Disable();
		interactAction.action.performed -= OnInteraction;
	}

	private void Update()
	{
		DoRaycast();
		UpdateCameraSensitivity();
		MatchCameraDirection();
	}

	private void DoRaycast()
	{
		Vector3 rayDirection = camTransform.forward;

		if (Physics.Raycast(camTransform.position, rayDirection, out lastHit, rayLength))
		{
			Debug.DrawLine(camTransform.position, lastHit.point, Color.green);
			hasHitSomething = true;

			if (lastHit.collider.TryGetComponent(out IInteractable interactable))
				currentInteractable = interactable;
			else
				currentInteractable = null;
		}
		else
		{
			Debug.DrawRay(camTransform.position, rayDirection * rayLength, Color.red);
			hasHitSomething = false;
			currentInteractable = null;
		}
	}

	private void OnInteraction(InputAction.CallbackContext context)
	{
		// Debug.Log("Iniciando interacción...");

		if (hasHitSomething && currentInteractable != null)
            currentInteractable.Interact();
    }

	private void UpdateCameraSensitivity()
	{
		if (cmiac == null || cmiac.Controllers == null)
			return;

		foreach (var c in cmiac.Controllers)
			switch (c.Name)
			{
				case "Look X (Pan)":
					c.Input.Gain = mouseSensitivity;
					break;
				case "Look Y (Tilt)":
					c.Input.Gain = -mouseSensitivity;
					break;
			}
	}

	private void MatchCameraDirection()
	{
		if (cmiac == null || lookAtTransform == null)
			return;

		Vector3 camForward = cmiac.transform.forward;
		lookAtTransform.rotation = Quaternion.LookRotation(camForward);
		camForward.y = 0.0f;

		if (camForward.sqrMagnitude > 0.001f)
			transform.rotation = Quaternion.LookRotation(camForward);
	}
}
