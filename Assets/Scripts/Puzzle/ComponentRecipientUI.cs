using TMPro;
using UnityEngine;

public class ComponentRecipientUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI stateText;
	[SerializeField] private PuzzleComponentRecipient pcr;

	private void Update()
	{
		stateText.text = $"Pieces: {pcr.CurrentlyCollectedCount} / {pcr.ExpectedCount}";
	}
}
