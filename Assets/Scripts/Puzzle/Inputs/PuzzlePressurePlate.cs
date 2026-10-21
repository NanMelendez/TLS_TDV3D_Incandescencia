using System;
using UnityEngine;

public class PuzzlePressurePlate : PuzzleComponent, IPuzzleActivator
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private string expectedColliderTag = "Player";

    private int objectCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(expectedColliderTag))
        {
            objectCount++;
            if (objectCount == 1)
                Activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(expectedColliderTag))
        {
            objectCount = Mathf.Max(objectCount - 1, 0);
            if (objectCount == 0)
                Deactivate();
        }
    }
}
