using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSelfLight : MonoBehaviour
{
    [SerializeField] private LightSrcComponent lightSrc;
    [SerializeField] private InputActionReference toggleAction;

    public bool IsOn => lightSrc.IsOn;

    private void OnEnable()
    {
        toggleAction.action.Enable();
        toggleAction.action.performed += OnTogglePress;
    }

    private void OnDisable()
    {
        toggleAction.action.Disable();
        toggleAction.action.performed -= OnTogglePress;
    }

    private void OnTogglePress(InputAction.CallbackContext context)
    {
        lightSrc.IsOn = !lightSrc.IsOn;
    }
}
