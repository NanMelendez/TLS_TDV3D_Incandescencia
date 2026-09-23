using System;
using UnityEngine;

public class MothSpawner : MonoBehaviour
{
    [SerializeField] private GameObject mothEntity;
    [SerializeField] private Collider col;

    public void Spawn()
    {
        col.enabled = false;
        Instantiate(mothEntity, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
