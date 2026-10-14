using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RoomTeleport : MonoBehaviour
{
    private bool teleporting;

    [SerializeField] private Transform destination;
    [SerializeField] private string playerTag = "Player";

    [Header("Teleport Settings")]
    [SerializeField] private float verticalOffset = 0.15f;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (teleporting)
            return;

        if (!other.CompareTag(playerTag))
            return;

        if (destination == null)
        {
            Debug.LogWarning(
                $"No entry point assigned in {name}.", this);
            return;
        }

        Teletransportar(other);
    }

    private void Teletransportar(Collider player)
    {
        teleporting = true;

        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller == null)
        {
            controller = player.GetComponentInParent<CharacterController>();
        }

        Transform playerTransform = controller != null ? controller.transform : player.transform;

        if (controller != null)
            controller.enabled = false;

        Vector3 spawnPosition = destination.position;
        spawnPosition.y += verticalOffset;

        playerTransform.SetPositionAndRotation(spawnPosition, destination.rotation);

        Physics.SyncTransforms();

        if (controller != null)
            controller.enabled = true;

        teleporting = false;
    }
}
