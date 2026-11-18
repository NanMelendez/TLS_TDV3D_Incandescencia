using TMPro;
using UnityEngine;

public class PasswordDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pwdText;

    public string PasswordText
    {
        get => pwdText.text;
        set
        {
            pwdText.text = value;
        }
    }
}
