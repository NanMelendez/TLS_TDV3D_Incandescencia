using UnityEngine;

public class PlayerKeyHolder : MonoBehaviour
{
    [SerializeField] Transform handPoint;
    public Key CurrentKey { get; private set; }

    public bool TryPickUp(Key key)
    {
        if (CurrentKey != null) return false;

        CurrentKey = key;
        key.transform.SetParent(handPoint);
        key.transform.localPosition = Vector3.zero;
        key.transform.localRotation = Quaternion.identity;
        if (key.TryGetComponent(out Rigidbody rb)) rb.isKinematic = true;
        return true;
    }

    public void Drop()
    {
        if (CurrentKey == null) return;
        CurrentKey.transform.SetParent(null);
        if (CurrentKey.TryGetComponent(out Rigidbody rb)) rb.isKinematic = false;
        CurrentKey = null;
    }

    public void ConsumeKey()
    {
        if (CurrentKey == null) return;
        Destroy(CurrentKey.gameObject);
        CurrentKey = null;
    }
}