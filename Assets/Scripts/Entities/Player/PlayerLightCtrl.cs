using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLightCtrl : MonoBehaviour
{
    [SerializeField] private LightSrcComp lsc;
    [SerializeField] private InputActionReference lightToggleAction;

    private void OnEnable()
    {
        lightToggleAction.action.Enable();
        lightToggleAction.action.performed += OnTogglePress;
    }

    private void OnDisable()
    {
        lightToggleAction.action.Disable();
        lightToggleAction.action.performed -= OnTogglePress;
    }

    private void OnTogglePress(InputAction.CallbackContext context)
    {
        lsc.Toggle();
    }
}
