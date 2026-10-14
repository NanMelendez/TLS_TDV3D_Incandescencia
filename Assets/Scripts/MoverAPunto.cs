using UnityEngine;

public class MoverAPunto : MonoBehaviour
{
    public Transform objetivo;
    public float velocidad = 5f;

    private bool moviendo = false;

    public void Mover()
    {
        if (objetivo != null && !moviendo)
        {
            moviendo = true;
        }
    }

    void Update()
    {
        if (!moviendo) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            objetivo.position,
            velocidad * Time.deltaTime
        );

        if (transform.position == objetivo.position)
        {
            moviendo = false;
        }
    }
}