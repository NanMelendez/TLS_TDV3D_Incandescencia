using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroSequence : MonoBehaviour
{
    [Header("Slides")]
    [SerializeField] private Image introImage;
    [SerializeField] private Sprite[] slides;
    [SerializeField] private float slideDuration = 2.5f;

    [Header("Final Fade")]
    [SerializeField] private Image fadeOverlay;
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float blackHoldDuration = 0.5f;

    [Header("Next Scene")]
    [SerializeField] private string nextScene = "GameplayProps";

    private bool isTransitioning;

    private IEnumerator Start()
    {
        if (introImage == null || slides == null ||
            slides.Length == 0 || fadeOverlay == null)
        {
            Debug.LogError("Falta configurar las viñetas o la transición.");
            yield break;
        }

        SetFadeAlpha(0f);

        foreach (Sprite slide in slides)
        {
            if (slide == null)
                continue;

            introImage.sprite = slide;

            yield return new WaitForSecondsRealtime(slideDuration);
        }

        yield return FadeAndLoad();
    }

    public void FinishIntro()
    {
        if (isTransitioning)
            return;

        StopAllCoroutines();
        StartCoroutine(FadeAndLoad());
    }

    private IEnumerator FadeAndLoad()
    {
        if (isTransitioning)
            yield break;

        isTransitioning = true;

        float elapsed = 0f;
        float initialAlpha = fadeOverlay.color.a;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(
                elapsed / Mathf.Max(fadeDuration, 0.01f)
            );

            SetFadeAlpha(Mathf.Lerp(initialAlpha, 1f, progress));

            yield return null;
        }

        SetFadeAlpha(1f);

        yield return new WaitForSecondsRealtime(blackHoldDuration);

        SceneManager.LoadScene(nextScene);
    }

    private void SetFadeAlpha(float alpha)
    {
        Color color = fadeOverlay.color;
        color.a = alpha;
        fadeOverlay.color = color;
    }
}