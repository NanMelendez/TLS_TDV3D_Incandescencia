using TMPro;
using UnityEngine;

public class PuzzleUIPassword : MonoBehaviour
{
    [SerializeField] private PuzzlePasswordReceiver passwordReceiver;
    [SerializeField] private TextMeshProUGUI TmpText;

    private void Awake()
    {
        passwordReceiver.OnInputChanged += UpdateText;
    }

    private void UpdateText(string str)
    {
        TmpText.text = str.Length > 0 ? str : new string('#', passwordReceiver.ExpectedPassword.Length);
    }
}
