using UnityEngine;
using UnityEngine.Events;

public class Lock : MonoBehaviour
{
    [SerializeField] int maxFailedAttempts = 3;

    public UnityEvent onOpened;
    public UnityEvent onWrongKey;
    public UnityEvent onLockedOut;

    int failedAttempts;
    bool isLocked;
    bool isOpen;

    public void TryOpen(PlayerKeyHolder player)
    {
        if (isLocked || isOpen) return;
        if (player.CurrentKey == null) return;

        bool correct = player.CurrentKey.isReal;
        player.ConsumeKey();

        if (correct)
        {
            isOpen = true;
            onOpened.Invoke();
            return;
        }

        failedAttempts++;
        onWrongKey.Invoke();

        if (failedAttempts >= maxFailedAttempts)
        {
            isLocked = true;
            onLockedOut.Invoke();
        }
    }
}