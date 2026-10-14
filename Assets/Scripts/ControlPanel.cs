using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject panelContenedor;
    public MonoBehaviour scriptMovimiento;

    private void Start()
    {
        panelContenedor.SetActive(false);
    }

    public void MostrarPanel()
    {
        panelContenedor.SetActive(true);

        scriptMovimiento.enabled = false;
    }

    public void OcultarPanel()
    {
        panelContenedor.SetActive(false);

        scriptMovimiento.enabled = true;
    }
}