using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MonsterSummonManager monsterSummonManager;
    [SerializeField] private GameplayUIManager gameplayUIManager;
	[SerializeField] private InputActionReference pauseAction;
	[SerializeField] private GameState gameState;

	private bool isPaused = false;
	private bool isGameOver = false;
	private PlayerController player;

	public GameState GameStatus
	{
		get => gameState;
		set
		{
			switch (value)
			{
				case GameState.Playing:
					gameplayUIManager.PausePanelState = false;
					HideCursor();
					Time.timeScale = 1.0f;
					break;
				case GameState.Paused:
					gameplayUIManager.PausePanelState = true;
					ReleaseCursor();
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

	private void OnDestroy()
	{
		ReleaseCursor();
	}

	private void Awake()
	{
		player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
		HideCursor();
	}

	private void Update()
    {
		if (!gameplayUIManager)
			return;

        gameplayUIManager.CountdownText = TimeSpan.FromSeconds(monsterSummonManager.CountdownTimer).ToString(@"mm\:ss");
    }

	private void OnApplicationFocus(bool focus)
	{
		if (focus)
		{
			if (gameState == GameState.Playing)
				HideCursor();
			else
				ReleaseCursor();
		}
	}

	private void OnPauseButtonPressed(InputAction.CallbackContext context)
	{
		PauseToggle();
	}

	public void PauseToggle()
	{
		if (!isGameOver)
			isPaused = !isPaused;

		GameStatus = isPaused ? GameState.Paused : GameState.Playing;
	}

	public void ChangeLevel(string levelName)
	{
		SceneManager.LoadScene(levelName);
	}

	public void TeleportPlayerTo(Transform location)
	{
		player.MovementActive = false;
		player.transform.position = location.position;
		player.MovementActive = true;
	}

	private void HideCursor()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void ReleaseCursor()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
}
