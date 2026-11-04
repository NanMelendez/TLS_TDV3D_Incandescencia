using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleSignalReceiver : PuzzleComponent, IPuzzleProgressProvider
{
	[SerializeField] private List<PuzzleComponent> puzzleActivators = new();
	public UnityEvent OnActivated;
	public UnityEvent OnDeactivated;

    public event Action<int, int> OnProgressChanged;
	public int MaxProgress => puzzleActivators.Count;
	public int CurrentProgress => puzzleActivators.Cast<IPuzzleActivator>().Count(activator => activator.IsActive);

	private void Awake()
	{
		foreach (IPuzzleActivator activator in puzzleActivators.Cast<IPuzzleActivator>())
			activator.OnStateChanged += OnActivatorStateChanged;
    }

    private void OnDestroy()
    {
		foreach (IPuzzleActivator activator in puzzleActivators.Cast<IPuzzleActivator>())
			activator.OnStateChanged -= OnActivatorStateChanged;
    }

    private void OnActivatorStateChanged(bool newState)
	{
		OnProgressChanged?.Invoke(CurrentProgress, MaxProgress);
		EvaluateState();
	}

	private void EvaluateState()
	{
		bool allActive = puzzleActivators.Count > 0 && puzzleActivators.Cast<IPuzzleActivator>().All(activator => activator.IsActive);

		if (allActive != isActive)
		{
			if (allActive)
				Activate();
			else
				Deactivate();

			if (isActive)
				OnActivated?.Invoke();
			else
				OnDeactivated?.Invoke();
		}
	}
}
