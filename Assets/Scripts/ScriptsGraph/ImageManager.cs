
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    [SerializeField] private RectTransform imageContainer;
    [SerializeField] private Image imagePrefab;

    private readonly Dictionary<string, Image> activeImages = new();

    public void ShowImage(string id, Sprite sprite, Vector2 position, Vector2 size, float duration)
    {
        if (string.IsNullOrWhiteSpace(id) || sprite == null)
            return;

        if (activeImages.TryGetValue(id, out Image existing))
        {
            StopAllCoroutines();
            existing.sprite = sprite;
            existing.rectTransform.anchoredPosition = position;
            existing.rectTransform.sizeDelta = size;
            StartCoroutine(FadeIn(existing, duration));
            return;
        }

        Image image = Instantiate(imagePrefab, imageContainer);
        image.sprite = sprite;
        image.preserveAspect = true;

        RectTransform rect = image.rectTransform;
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        CanvasGroup group = image.GetComponent<CanvasGroup>();

        if (group == null)
            group = image.gameObject.AddComponent<CanvasGroup>();

        group.alpha = 0f;

        activeImages.Add(id, image);
        StartCoroutine(FadeIn(image, duration));
    }

    public void HideImage(string id, float duration)
    {
        if (!activeImages.TryGetValue(id, out Image image))
            return;

        StartCoroutine(FadeOut(id, image, duration));
    }

    private IEnumerator FadeIn(Image image, float duration)
    {
        CanvasGroup group = image.GetComponent<CanvasGroup>();

        if (duration <= 0f)
        {
            group.alpha = 1f;
            yield break;
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            group.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        group.alpha = 1f;
    }

    private IEnumerator FadeOut(string id, Image image, float duration)
    {
        CanvasGroup group = image.GetComponent<CanvasGroup>();

        if (duration > 0f)
        {
            float elapsed = 0f;
            float initialAlpha = group.alpha;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                group.alpha = Mathf.Lerp(initialAlpha, 0f, Mathf.Clamp01(elapsed / duration));

                yield return null;
            }
        }

        activeImages.Remove(id);
        Destroy(image.gameObject);
    }
}