using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
	[SerializeField] [Min(0.01f)] private float rayLength = 1.0f;
	[SerializeField] private Transform origin;
	[SerializeField] private InputActionReference interactAction;

	private RaycastHit lastHit;
	private bool hasHitSomething;

	private void OnEnable()
	{
		interactAction.action.Enable();
		interactAction.action.performed += OnInteractionClick;
	}

	private void OnDisable()
	{
		interactAction.action.Disable();
		interactAction.action.performed -= OnInteractionClick;
	}

	private void Update()
	{
		Vector3 rayDirection = origin.forward;

		if (Physics.Raycast(origin.position, rayDirection, out lastHit, rayLength))
		{
			Debug.DrawLine(origin.position, lastHit.point, Color.green);
			hasHitSomething = true;
		}
		else
		{
			Debug.DrawRay(origin.position, rayDirection * rayLength, Color.red);
			hasHitSomething = false;
		}
	}

	private void OnInteractionClick(InputAction.CallbackContext context)
	{
		if (hasHitSomething)
		{
			// Debug.Log("TAG: " + lastHit.collider.tag);
			if (lastHit.collider.CompareTag("LightInteractable"))
			{
				Debug.Log("Interactuando...");
				lastHit.collider.GetComponent<InteractableToggle>().ToggleLight();
			}
		}
	}
}
