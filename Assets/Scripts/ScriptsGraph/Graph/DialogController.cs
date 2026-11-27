using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    private readonly HashSet<string> activatedTriggers = new HashSet<string>();
    private readonly Dictionary<string, TaskCompletionSource<bool>> pendingTriggers = new Dictionary<string, TaskCompletionSource<bool>>();
    
    [SerializeField] private TextMeshProUGUI dialogText;

    [SerializeField] private Image tutorialImage;

    [SerializeField] private GenericRuntimeGraph graph;

    [SerializeField] private TutorialManager tutorialManager;

    private INodeResolver<TextRuntimeNode> textResolver;
    private INodeResolver<ImageRuntimeNode> imageResolver;
    private INodeResolver<TutorialRuntimeNode> tutorialResolver;
    private INodeResolver<WaitForTriggerRuntimeNode> triggerResolver;

    private void Start()
    {
        textResolver = new TextNodeResolver();
        imageResolver = new ImageNodeResolver();
        tutorialResolver = new TutorialNodeResolver();
        triggerResolver = new WaitForTriggerResolver();


        Resolve();
    }

    public async void Resolve()
    {
        if (graph == null)
        {
            Debug.LogError("No se ha asignado el RuntimeGraph.");
            return;
        }

        await Task.Yield();

        foreach (GenericRuntimeNode node in graph.nodes)
        {
            switch (node)
            {
                case TextRuntimeNode textNode:
                    await textResolver.Resolve(this, textNode);
                    break;

                case ImageRuntimeNode imageNode:
                    await imageResolver.Resolve(this, imageNode);
                    break;

                case TutorialRuntimeNode tutorialNode:
                    await tutorialResolver.Resolve(this, tutorialNode);
                    break;
                case WaitForTriggerRuntimeNode triggerNode:
                        await triggerResolver.Resolve(this, triggerNode);
                        break;

                default:
                    Debug.LogWarning(
                        $"No existe un resolver para: {node.GetType().Name}"
                    );
                    break;
            }
        }
    }

    public void DisplayText(string text)
    {
        if (dialogText != null)
            dialogText.text = text;
    }

    public void DisplayImage(Sprite image)
    {
        if (tutorialImage == null)
            return;

        tutorialImage.sprite = image;
        tutorialImage.enabled = image != null;
    }

    public async Task ExecuteTutorialStep(TutorialRuntimeNode node)
    {
        if (tutorialManager == null)
        {
            Debug.LogError("No se ha asignado TutorialManager.");
            return;
        }

        await tutorialManager.ExecuteStep(node);
    }

    public async Task WaitForTrigger(string colliderTag)
    {
        if (string.IsNullOrWhiteSpace(colliderTag))
        {
            Debug.LogError("WaitForTrigger: El Collider Tag está vacío.");
            return;
        }

        if (activatedTriggers.Contains(colliderTag))
        {
            activatedTriggers.Remove(colliderTag);
            return;
        }

        if (pendingTriggers.ContainsKey(colliderTag))
        {
            Debug.LogError($"Ya existe una espera activa para el tag: {colliderTag}");
            return;
        }

        TaskCompletionSource<bool> completion =
            new TaskCompletionSource<bool>();

        pendingTriggers.Add(colliderTag, completion);

        Debug.Log($"Esperando al collider con tag: {colliderTag}");

        await completion.Task;

        pendingTriggers.Remove(colliderTag);
        activatedTriggers.Remove(colliderTag);

        Debug.Log($"Collider activado: {colliderTag}. Continuando secuencia.");
    }

    public void ActivateTrigger(string colliderTag)
    {
        if (string.IsNullOrWhiteSpace(colliderTag))
            return;

        if (pendingTriggers.TryGetValue(colliderTag, out TaskCompletionSource<bool> completion))
        {
            completion.TrySetResult(true);
        }
        else
        {
            activatedTriggers.Add(colliderTag);
        }
    }
}