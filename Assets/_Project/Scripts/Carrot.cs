using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carrot : MonoBehaviour
{
    [Header("Events")]
    public static Action OnFrenzyModeStarted;
    public static Action OnFrenzyModeFinished;

    [Header("Elements")]
    [SerializeField] private Transform carrotSpriteTransform;
    [SerializeField] private Image fillImage;
    [SerializeField] private float defaultScale = 0.8f;
    [SerializeField] private float clickScaleDownAmount = 0.7f;
    [SerializeField] private float clickScaleDuration = 0.15f;

    [Header("Settings")]
    [SerializeField] private float fillRate;
    private bool isFrenzyModeActive;

    private void Start()
    {
        InputManager.OnCarrotClicked += CarrotClickedCallback;
    }

    private void OnDestroy()
    {
        InputManager.OnCarrotClicked -= CarrotClickedCallback;
    }

    private void CarrotClickedCallback()
    {
        Animate();

        if (!isFrenzyModeActive)
            Fill();
    }

    private void Fill()
    {
        fillImage.fillAmount += fillRate;

        if (fillImage.fillAmount >= 1)
        {
            StartFrenzyMode();
        }
    }

    private void StartFrenzyMode()
    {
        isFrenzyModeActive = true;
        DOVirtual.Float(1f, 0f, 5f, (value) => fillImage.fillAmount = value)
            .OnComplete(StopFrenzyMode);
        OnFrenzyModeStarted?.Invoke();
    }

    private void StopFrenzyMode()
    {
        isFrenzyModeActive = false;
        OnFrenzyModeFinished?.Invoke();
    }

    private void Animate()
    {
        carrotSpriteTransform.localScale = Vector3.one * defaultScale;
        carrotSpriteTransform.DOKill();
        carrotSpriteTransform.DOScale(Vector3.one * clickScaleDownAmount, clickScaleDuration)
            .SetLoops(2, LoopType.Yoyo);
    }
}