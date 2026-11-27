using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private List<KeyCode> pendingKeys;
    private TaskCompletionSource<bool> inputCompletion;

    private bool isWaitingForInput;

    public int CurrentStep { get; private set; }

    public async Task ExecuteStep(TutorialRuntimeNode node)
    {
        if (isWaitingForInput)
            return;

        CurrentStep = node.stepIndex;
        pendingKeys = new List<KeyCode>(node.requiredKeys);

        inputCompletion = new TaskCompletionSource<bool>();
        isWaitingForInput = true;

        if (pendingKeys.Count == 0)
            inputCompletion.TrySetResult(true);

        await inputCompletion.Task;

        isWaitingForInput = false;
        inputCompletion = null;
        pendingKeys.Clear();

        Debug.Log($"Tutorial: paso {CurrentStep} completado.");
    }

    private void Update()
    {
        if (!isWaitingForInput)
            return;

        for (int i = pendingKeys.Count - 1; i >= 0; i--)
        {
            if (Input.GetKeyDown(pendingKeys[i]))
                pendingKeys.RemoveAt(i);
        }

        if (pendingKeys.Count == 0)
            inputCompletion?.TrySetResult(true);
    }
}