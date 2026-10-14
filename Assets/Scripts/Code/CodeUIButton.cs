using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CodeUIButton : MonoBehaviour
{
    public enum Kind { Digit, Clear, Confirm }

    [SerializeField] CodePanel panel;
    [SerializeField] Kind kind = Kind.Digit;
    [SerializeField, Range(0, 9)] int digit;

    void Awake()
    {
        if (panel == null) panel = FindFirstObjectByType<CodePanel>();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        switch (kind)
        {
            case Kind.Digit: panel.AddDigit(digit); break;
            case Kind.Clear: panel.ClearCode(); break;
            case Kind.Confirm: panel.Confirm(); break;
        }
    }
}