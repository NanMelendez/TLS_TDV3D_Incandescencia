using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MonsterSummonManager monsterSummonManager;
    [SerializeField] private GameplayUIManager gameplayUIManager;
	[SerializeField] private InputActionReference pauseAction;
	[SerializeField] private GameState gameState;

	private bool isPaused = false;
	private bool isGameOver = false;

	public GameState GameStatus
	{
		get => gameState;
		set
		{
			switch (value)
			{
				case GameState.Playing:
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = true;
					Time.timeScale = 1.0f;
					break;
				case GameState.Paused:
					Cursor.lockState = CursorLockMode.Confined;
					Cursor.visible = false;
					Time.timeScale = 0.0f;
					break;
				case GameState.GameOver:
					Time.timeScale = 0.0f;
					break;
			}

			gameState = value;
		}
	}

	private void OnEnable()
	{
		pauseAction.action.Enable();
		pauseAction.action.performed += OnPauseButtonPressed;
	}

	private void OnDisable()
	{
		pauseAction.action.Disable();
		pauseAction.action.performed -= OnPauseButtonPressed;
	}

	private void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
    {
        gameplayUIManager.CountdownText = TimeSpan.FromSeconds(monsterSummonManager.CountdownTimer).ToString(@"mm\:ss");
    }

	private void OnPauseButtonPressed(InputAction.CallbackContext context)
	{
		if (!isGameOver)
			isPaused = !isPaused;
		
		GameStatus = isPaused ? GameState.Paused : GameState.Playing;
	}
}
