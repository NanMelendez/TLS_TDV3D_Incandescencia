using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject entity;
    [SerializeField] private Collider col;

    public void Spawn()
    {
        col.enabled = false;
        Instantiate(entity, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
