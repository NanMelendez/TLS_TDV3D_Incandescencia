using TMPro;
using UnityEngine;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject pausePanel;


	private void OnEnable()
	{
        PausePanelState = false;
	}

	public string CountdownText
    {
        get => countdownText.text;
        set
        {
            countdownText.text = value;
        }
    }

    public bool PausePanelState
    {
        get => pausePanel.activeSelf;
        set
        {
            pausePanel.SetActive(value);
        }
    }
}
