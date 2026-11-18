using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSummonManager : MonoBehaviour
{
    [SerializeField][Min(0.0f)] private float countdown;
    [SerializeField] private MonsterSpawner spawner;

    private List<LightSrcComponent> lights = new();
    private bool monsterHasBeenSummoned = false;
    private int activeLightsCount = 0;

    public float CountdownTimer {
        get => countdown;
        set
        {
            countdown = value;
        }
    }

    /*
    private void Awake()
    {
        Init();
    }
    */

    private void Update()
    {
        if (!spawner)
            return;

        if (countdown > 0.0f && activeLightsCount > 0)
        {
            countdown = Mathf.Max(countdown - Time.deltaTime, 0);
            // Debug.Log($"Restante: {countdown}");
        }

        if (!monsterHasBeenSummoned && countdown == 0.0f)
        {
            Summon();
            monsterHasBeenSummoned = true;
        }
    }

    public void Init(float countdown, MonsterSpawner spawner = null)
    {
        Configure(countdown, spawner);
    }

    public void Configure(float countdown, MonsterSpawner spawner = null)
    {
        lights.Clear();
        activeLightsCount = 0;
        this.spawner = spawner;
        this.countdown = countdown;
        lights = FindObjectsByType<LightSrcComponent>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();

        foreach (LightSrcComponent light in lights)
            light.OnStateChanged += HandleLightsStateChanged;
    }

    private void HandleLightsStateChanged(bool isOn)
    {
        if (isOn)
            activeLightsCount++;
        else
            activeLightsCount = Mathf.Max(0, activeLightsCount - 1);
    }

    private void Summon()
    {
        spawner.Spawn();
    }
}
