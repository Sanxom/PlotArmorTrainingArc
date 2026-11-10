using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShopManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private RectTransform shopPanel;

    [Header("Settings")]
    [SerializeField] private float shopPanelTweenTime = 0.3f;
    private Vector2 openedPosition;
    private Vector2 closedPosition;

    private void Start()
    {
        openedPosition = Vector2.zero;
        closedPosition = new Vector2(shopPanel.rect.width, 0f);
        shopPanel.anchoredPosition = closedPosition;
    }

    public void Open()
    {
        shopPanel.DOKill();
        shopPanel.DOAnchorPos(openedPosition, shopPanelTweenTime).SetEase(Ease.InOutSine);
        InputManager.EnableUIActionsDisablePlayerActions();
    }

    public void Close()
    {
        shopPanel.DOKill();
        shopPanel.DOAnchorPos(closedPosition, shopPanelTweenTime).SetEase(Ease.InOutSine);
        InputManager.EnablePlayerActionsDisableUIActions();
    }
}