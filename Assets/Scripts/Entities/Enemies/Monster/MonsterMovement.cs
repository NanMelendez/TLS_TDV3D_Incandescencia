using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    [SerializeField][Min(0.0f)] private float speed;
    [SerializeField] private Rigidbody rb;

    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if (playerTransform)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            rb.linearVelocity = new Vector3(direction.x * speed, rb.linearVelocity.y, direction.z * speed);
        }
        else
            rb.linearVelocity = new Vector3(0.0f, rb.linearVelocity.y, 0.0f);
    }
}
