using TMPro;
using UnityEngine;

public class CodeDisplay : MonoBehaviour
{
    [SerializeField] private PuzzlePasswordReceiver pR;
    [SerializeField] private TMP_Text tmpText; // Acepta TextMeshPro (3D) y TextMeshProUGUI (UI)

    [Tooltip("Si está activo, el texto siempre muestra el código esperado del receiver.")]
    [SerializeField] private bool showExpectedCode = true;

    private void Awake()
    {
        pR.OnInputChanged += UpdateText;
    }

    private void Start()
    {
        UpdateText(pR.CurrentInput);
    }

    private void OnDestroy()
    {
        if (pR != null)
            pR.OnInputChanged -= UpdateText;
    }

    private void UpdateText(string input)
    {
        if (showExpectedCode)
        {
            tmpText.text = pR.ExpectedPassword;
            return;
        }

        tmpText.text = input.Length > 0
            ? input
            : new string('#', pR.ExpectedPassword.Length);
    }
}