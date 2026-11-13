using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    public TMP_Text textoUI;

    public void SetText(string nuevoTexto)
    {
        textoUI.text = nuevoTexto;
    }
}