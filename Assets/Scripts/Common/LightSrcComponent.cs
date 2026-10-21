using System;
using UnityEngine;

public class LightSrcComponent : MonoBehaviour
{
	[SerializeField] private Light lightSrc;
	[SerializeField] private bool isOn = false;

	public event Action<bool> OnStateChanged;

	public bool IsOn
	{
		get => isOn;
		set
		{
			if (isOn != value)
			{
				isOn = value;
				UpdateLightState();
				OnStateChanged?.Invoke(isOn);
			}
		}
	}

	public void Activate()
	{
		IsOn = true;
	}

	public void Deactivate()
	{
		IsOn = false;
	}

	private void UpdateLightState()
	{
		lightSrc.enabled = isOn;
	}
}
