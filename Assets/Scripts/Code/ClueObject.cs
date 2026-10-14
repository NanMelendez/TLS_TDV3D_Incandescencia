using UnityEngine;

public class ClueObject : MonoBehaviour
{
    [Range(0, 9)] public int digitValue;
    [TextArea] public string verse;
}