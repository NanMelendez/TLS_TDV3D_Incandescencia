using TMPro;
using UnityEngine;

public class ButtonStateUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI stateText;
	[SerializeField] private BasePuzzleComponent puzzlePiece;
	[SerializeField] private LightSrcComp lightComp;

	private void Update()
	{
		stateText.text = $"Status: {((puzzlePiece ? puzzlePiece.ComponentIsActive : lightComp.IsOn) ? "ON" : "OFF")}{(puzzlePiece is PuzzleClickableButton btn && puzzlePiece.ComponentIsActive ? $"\n{Mathf.Ceil(btn.CountdownTimer)}" : "")}";
	}
}
