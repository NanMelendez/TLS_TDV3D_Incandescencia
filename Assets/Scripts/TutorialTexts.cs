using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialTexts : MonoBehaviour
{
    private static readonly HashSet<string> shownTutorials = new();

    [SerializeField] private PuzzleComponent puzzle;
    [SerializeField] private string tutorialId = "laser";
    [SerializeField] private GameObject textObject;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField, TextArea(2, 5)] private string message;

    private IPuzzleActivator activator;
    private bool shown;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics() => shownTutorials.Clear();

    private void Awake()
    {
        if (textObject) textObject.SetActive(false);
        if (textLabel) textLabel.text = message;

        activator = puzzle as IPuzzleActivator;
        if (activator != null)
            activator.OnStateChanged += OnStateChanged;
    }

    private void OnDestroy()
    {
        if (activator != null)
            activator.OnStateChanged -= OnStateChanged;
    }

    private void OnStateChanged(bool isActive)
    {
        if (!isActive || shown)
            return;

        shown = true;

        if (!shownTutorials.Add(tutorialId))
            return;

        if (textObject) textObject.SetActive(true);
    }
}