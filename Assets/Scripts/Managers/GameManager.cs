using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MonsterSummonManager monsterSummonManager;
    [SerializeField] private GameplayUIManager gameplayUIManager;

    private void Update()
    {
        gameplayUIManager.CountdownText = TimeSpan.FromSeconds(monsterSummonManager.CountdownTimer).ToString(@"mm\:ss");
    }
}
