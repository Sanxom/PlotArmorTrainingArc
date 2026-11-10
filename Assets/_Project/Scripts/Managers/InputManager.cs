using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Events")]
    public static Action OnCarrotClicked;
    public static Action<Vector2> OnCarrotClickedPosition;

    [Header("Data")]
    private static GameInput.PlayerActions playerActions;
    private static GameInput.UIActions uiActions;
    private GameInput gameInput;
    private InputAction clickAction;
    private InputAction pointerPositionAction;
    private Camera mainCam;
    Vector3 pointerPosition;

    private void Awake()
    {
        mainCam = Camera.main;
        gameInput = new();
        playerActions = gameInput.Player;
        uiActions = gameInput.UI;
        clickAction = gameInput.Player.Click;
        pointerPositionAction = gameInput.Player.PointerPosition;
        gameInput.Enable();
    }

    private void OnEnable()
    {
        clickAction.performed += ClickAction_Performed;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (pointerPositionAction.triggered)
            pointerPosition = pointerPositionAction.ReadValue<Vector2>();
    }

    private void OnDisable()
    {
        clickAction.performed -= ClickAction_Performed;
        gameInput.Disable();
    }

    public static void EnableUIActionsDisablePlayerActions()
    {
        playerActions.Disable();
        uiActions.Enable();
    }

    public static void EnablePlayerActionsDisableUIActions()
    {
        uiActions.Disable();
        playerActions.Enable();
    }

    private void ClickAction_Performed(InputAction.CallbackContext context)
    {
        ThrowRaycast();
    }

    private void ThrowRaycast()
    {
        if (!IsValidScreenPosition(pointerPosition))
            return;
        RaycastHit2D hit = Physics2D.GetRayIntersection(mainCam.ScreenPointToRay(pointerPosition), 10f);
        if (hit.collider == null)
            return;
        OnCarrotClicked?.Invoke();
        OnCarrotClickedPosition?.Invoke(hit.point);
    }

    private static bool IsValidScreenPosition(Vector2 position)
    {
        return float.IsFinite(position.x) && float.IsFinite(position.y);
    }
}