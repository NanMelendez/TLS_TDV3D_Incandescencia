using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSummonManager : MonoBehaviour
{
    [SerializeField][Min(0.0f)] private float countdown;
    [SerializeField] private MonsterSpawner spawner;
    public int monsterTeleportStep = 26;

    private List<LightSrcComponent> lights = new();
    private bool monsterHasBeenSummoned = false;
    private int activeLightsCount = 0;
    private float backupCountdown;

    public float CountdownTimer => countdown;

    private void Awake()
    {
        backupCountdown = countdown;
        lights = FindObjectsByType<LightSrcComponent>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();

        foreach (LightSrcComponent light in lights)
        {
            light.OnStateChanged += HandleLightsStateChanged;
        }
    }

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

    private void HandleLightsStateChanged(bool isOn)
    {
        if (isOn)
            activeLightsCount++;
        else
            activeLightsCount = Mathf.Max(0, activeLightsCount - 1);
    }

    private void Summon()
    {
        Debug.Log("¡CORRE HVÓN!");
        spawner.Spawn();
    }

    public void ResetTimer()
    {
        countdown = backupCountdown;

        Transform spawnerTransform = spawner.transform.parent;

		spawnerTransform.position = new Vector3(spawnerTransform.position.x, spawnerTransform.position.y, spawnerTransform.position.z + monsterTeleportStep);
    }
}
