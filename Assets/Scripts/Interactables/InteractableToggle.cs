using UnityEngine;

public class InteractableToggle : MonoBehaviour
{
	[SerializeField] private Light lightSrc;

	private bool isOn = false;

	private void Start()
	{
		SetLightComponents();
	}

	public void ToggleLight()
	{
		isOn = !isOn;
		SetLightComponents();
	}

	private void SetLightComponents()
	{
		lightSrc.enabled = isOn;
	}
}
