using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSummonManager : MonoBehaviour
{
	[SerializeField] private List<LightSrcComp> lights = new();
	[SerializeField] private float countdown;

	public float CountdownTimer
	{
		get => countdown;
	}

	private void Awake()
	{
		lights = FindObjectsByType<LightSrcComp>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
	}

	private void Update()
	{
		if (countdown > 0.0f && IsAnyLightOn())
			countdown = Mathf.Max(countdown - Time.deltaTime, 0.0f);
	}

	private bool IsAnyLightOn()
	{
		bool var = false;

		if (lights.Count > 0)
            foreach (LightSrcComp light in lights)
            {
                if (light.IsOn)
                {
                    var = true;
                    break;
                }
            }

        return var;
	}
}
