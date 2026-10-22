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
    private GameInput gameInput;
    private InputAction clickAction;
    private InputAction pointerPositionAction;
    private Camera mainCam;
    Vector3 pointerPosition;

    private void Awake()
    {
        mainCam = Camera.main;
        gameInput = new();
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

    private void ClickAction_Performed(InputAction.CallbackContext context)
    {
        ThrowRaycast();
    }

    private void ThrowRaycast()
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(mainCam.ScreenPointToRay(pointerPosition), 10f);

        if (hit.collider == null)
            return;
        OnCarrotClicked?.Invoke();
        OnCarrotClickedPosition?.Invoke(hit.point);
    }
}