using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    [SerializeField] private DialogController dialogController;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        dialogController.ActivateTrigger(gameObject.tag);
    }
}