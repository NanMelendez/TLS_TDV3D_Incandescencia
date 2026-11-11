using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI countdownText;
	[SerializeField] private GameObject pausePanel;
	[SerializeField] private TextMeshProUGUI messageTxt;
	[SerializeField][Min(0.01f)] private float messageDisplayTimer = 2.5f;
	[SerializeField] private Image currentCollectibleImg;

	private Coroutine messageDisplayCoroutine = null;

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

	public void HandleMessageText(string text)
	{
		if (messageDisplayCoroutine != null)
            StopCoroutine(messageDisplayCoroutine);

		messageTxt.text = text;
		messageDisplayCoroutine = StartCoroutine(HandleMessageFade());
    }

	public void HandleCollectedImg(bool holding, Sprite sprite = null)
	{
		currentCollectibleImg.gameObject.SetActive(holding);

		if (holding)
			currentCollectibleImg.sprite = sprite;
	}

	private IEnumerator HandleMessageFade()
	{
		float timer = messageDisplayTimer;
        messageTxt.alpha = 1.0f;

		while (timer > 0.0f)
		{
            messageTxt.alpha = timer / messageDisplayTimer;
            timer -= Time.deltaTime;
            yield return null;
        }

		messageTxt.alpha = 0.0f;
		messageDisplayCoroutine = null;
	}
}
