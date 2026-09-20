using UnityEngine;

public class LightSrcComp : MonoBehaviour
{
	[SerializeField] private Light lightSrc;

	private bool isOn = false;

	public bool IsOn
	{
		get => isOn;
		set
		{
			if (isOn != value)
			{
				isOn = value;
				SetLightComponents();
			}
		}
	}

	private void Start()
	{
		SetLightComponents();
	}

	public void Toggle()
	{
		IsOn = !IsOn;
	}

	private void SetLightComponents()
	{
		lightSrc.enabled = isOn;
	}
}
