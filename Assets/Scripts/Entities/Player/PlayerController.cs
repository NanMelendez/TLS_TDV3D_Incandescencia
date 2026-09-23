using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cmCamera;
    [SerializeField] private CinemachineInputAxisController cmiac;
    [SerializeField] private PlayerHealth healthManager;
    [SerializeField] private PlayerMovement movementManager;
    [SerializeField] private PlayerAim aimManager;
    [SerializeField] private PlayerLightCtrl lightToggleManager;

    private bool healthStatus;

    public bool IsAlive
    {
        get => healthManager.IsAlive;
    }

    private void Awake()
    {
        healthStatus = healthManager.IsAlive;
    }

    private void Update()
    {
        if (healthStatus && !healthManager.IsAlive)
        {
            Debug.Log("Y... murió!");
            cmCamera.Follow = null;
            cmCamera.LookAt = null;
            cmiac.enabled = false;

            healthStatus = false;

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("GiantMoth"))
        {
            healthManager.TakeDamage(1);
        }
    }
}
