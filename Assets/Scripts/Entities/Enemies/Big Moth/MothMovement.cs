using UnityEngine;

public class MothMovement : MonoBehaviour
{
    [SerializeField] [Min(0.0f)] private float speed;
    [SerializeField] private Rigidbody rb;

    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;

            rb.linearVelocity = new Vector3(direction.x * speed, rb.linearVelocity.y, direction.z * speed);
        }
        else
            rb.linearVelocity = Vector2.zero;
    }
}
