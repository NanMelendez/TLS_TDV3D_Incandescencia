using TMPro;
using UnityEngine;

public class PuzzleUITextDisplay : MonoBehaviour
{
	[SerializeField] private PuzzleComponent puzzleComponent;
	[SerializeField] private TextMeshProUGUI uiText;

	private IPuzzleProgressProvider progressProvider;
	private PuzzleButton puzzleButton;

    private void Start()
    {
        if (puzzleComponent == null)
            return;

        progressProvider = puzzleComponent as IPuzzleProgressProvider;
        if (progressProvider != null)
        {
            progressProvider.OnProgressChanged += HandleProgressChanged;
            HandleProgressChanged(progressProvider.CurrentProgress, progressProvider.MaxProgress);
            return;
        }

        puzzleButton = puzzleComponent as PuzzleButton;
        if (puzzleButton != null)
        {
            puzzleButton.OnStateChanged += HandleButtonStateChanged;
            HandleButtonStateChanged(puzzleButton.IsActive);
            return;
        }

        puzzleComponent.OnStateChanged += HandleGenericStateChanged;
        HandleGenericStateChanged(puzzleComponent.IsActive);
    }

    private void OnDestroy()
    {
        if (progressProvider != null)
            progressProvider.OnProgressChanged -= HandleProgressChanged;

        if (puzzleButton != null)
            puzzleButton.OnStateChanged -= HandleButtonStateChanged;

        if (puzzleComponent != null)
            puzzleComponent.OnStateChanged -= HandleGenericStateChanged;
    }

    private void Update()
    {
        if (puzzleButton != null && puzzleButton.IsActive && puzzleButton.ToggleBackDuration > 0.0f)
        {
            float remaining = puzzleButton.RemainingTime;
            if (remaining > 0.0f)
                uiText.text = $"{remaining:F1}s";
        }
    }

    private void HandleProgressChanged(int current, int total)
    {
        uiText.text = $"{current} / {total}";
    }

    private void HandleButtonStateChanged(bool isActive)
    {
        uiText.text = isActive ? "ON" : "OFF";
    }

    private void HandleGenericStateChanged(bool isActive)
    {
        uiText.text = isActive ? "ON" : "OFF";
    }


}
