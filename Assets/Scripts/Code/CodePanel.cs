using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class CodePanel : MonoBehaviour
{
    [SerializeField] CodePuzzleManager puzzle;

    [Header("Panel físico (en el mundo)")]
    [SerializeField] Collider interactCollider;
    [SerializeField] float interactDistance = 2.5f;
    [SerializeField] Camera cam;

    [Header("Panel UI (en pantalla)")]
    [Tooltip("Raíz del panel UI. Este script NO debe estar dentro de él.")]
    [SerializeField] GameObject uiPanel;
    [Tooltip("Botón que se selecciona al abrir el panel (por ejemplo el 1).")]
    [SerializeField] Selectable firstSelected;
    [SerializeField] TMP_Text codeDisplay;

    [Header("Jugador")]
    [SerializeField] Behaviour[] lockWhileActive;

    [Header("Eventos")]
    public UnityEvent onEnterPanel;
    public UnityEvent onExitPanel;

    public static bool IsOpen { get; private set; }

    readonly List<int> entered = new();
    GameObject lastSelected;
    bool active, solved, busy;

    void Awake()
    {
        IsOpen = false;
        if (cam == null) cam = Camera.main;
        if (uiPanel != null)
        {
            if (transform.IsChildOf(uiPanel.transform))
                Debug.LogError("[CodePanel] Este script está dentro de Ui Panel; muévelo a otro objeto.", this);
            uiPanel.SetActive(false);
        }
        RefreshDisplay();
    }

    void OnDisable() => IsOpen = false;

    void Update()
    {
        if (!active)
        {
            if (!solved && KeyPressed(true) && LookingAtPanel()) Enter();
            return;
        }

        if (KeyPressed(false)) { Exit(); return; }

        var es = EventSystem.current;
        if (es.currentSelectedGameObject == null) es.SetSelectedGameObject(lastSelected);
        else lastSelected = es.currentSelectedGameObject;
    }

    bool LookingAtPanel()
    {
        if (cam == null || interactCollider == null) return false;
        var ray = new Ray(cam.transform.position, cam.transform.forward);
        return Physics.Raycast(ray, out var hit, interactDistance, ~0, QueryTriggerInteraction.Collide)
               && hit.collider == interactCollider;
    }

    void Enter()
    {
        active = true;
        IsOpen = true;
        uiPanel.SetActive(true);

        lastSelected = firstSelected.gameObject;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(lastSelected);

        SetPlayerLocked(true);
        onEnterPanel?.Invoke();
    }

    void Exit()
    {
        active = false;
        IsOpen = false;
        uiPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);

        SetPlayerLocked(false);
        onExitPanel?.Invoke();
    }

    void SetPlayerLocked(bool locked)
    {
        foreach (var b in lockWhileActive)
            if (b != null) b.enabled = !locked;
    }

    public void AddDigit(int d)
    {
        if (busy || entered.Count >= CodePuzzleManager.CodeLength) return;
        entered.Add(d);
        RefreshDisplay();
    }

    public void ClearCode()
    {
        if (busy) return;
        entered.Clear();
        RefreshDisplay();
    }

    public void Confirm()
    {
        if (busy) return;
        if (puzzle.CheckCode(entered))
        {
            solved = true;
            Exit();
            puzzle.Solve();
        }
        else StartCoroutine(WrongRoutine());
    }

    IEnumerator WrongRoutine()
    {
        busy = true;
        if (codeDisplay != null) codeDisplay.text = "ERROR";
        yield return new WaitForSecondsRealtime(0.8f);
        entered.Clear();
        RefreshDisplay();
        busy = false;
    }

    void RefreshDisplay()
    {
        if (codeDisplay == null) return;
        var chars = new string[CodePuzzleManager.CodeLength];
        for (int i = 0; i < chars.Length; i++)
            chars[i] = i < entered.Count ? entered[i].ToString() : "_";
        codeDisplay.text = string.Join(" ", chars);
    }

    static bool KeyPressed(bool interact)
    {
#if ENABLE_INPUT_SYSTEM
        var k = Keyboard.current;
        if (k == null) return false;
        return interact ? k.eKey.wasPressedThisFrame : k.escapeKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(interact ? KeyCode.E : KeyCode.Escape);
#endif
    }
}