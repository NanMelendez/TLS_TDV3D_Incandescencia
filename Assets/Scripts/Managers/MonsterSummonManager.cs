using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSummonManager : MonoBehaviour
{
	[SerializeField] [Min(0.0f)] private float countdown;
    [SerializeField] private MothSpawner spawner;

    private List<LightSrcComp> lights = new();
    private bool monsterHasBeenSummoned = false;

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

		if (!monsterHasBeenSummoned && countdown == 0.0f)
		{
			SummonMonster();
			monsterHasBeenSummoned = true;
		}
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

	private void SummonMonster()
	{
		Debug.Log("RUN BRO, RUUUUUUUUUUUUUUN!");
        spawner.Spawn();
	}
}
