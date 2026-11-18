using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    [TextArea]
    public string mensaje;
    public TextDisplay display;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            display.SetText(mensaje);
        }
    }
}