using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSelfLight : MonoBehaviour
{
    [SerializeField] private LightSrcComponent lightSrc;
    [SerializeField] private InputActionReference toggleAction;

    private GameManager gm;

    public bool IsOn => lightSrc.IsOn;

    public Light LightSrc => lightSrc.LightSrc;

	private void Awake()
	{
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
	}

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
        if (gm.GameStatus == GameState.Playing)
            lightSrc.IsOn = !lightSrc.IsOn;
    }
}
