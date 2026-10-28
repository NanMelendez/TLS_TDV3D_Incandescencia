using TMPro;
using UnityEngine;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    public string CountdownText
    {
        get => countdownText.text;
        set
        {
            countdownText.text = value;
        }
    }
}
