using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarrotManager : MonoBehaviour
{
    public static CarrotManager Instance { get; private set; }

    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI carrotCountText;

    [Header("Data")]
    [SerializeField] private double totalCarrotCount;
    [SerializeField] private int frenzyModeMultiplier;
    private int carrotIncrementAmount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        carrotIncrementAmount = 1;
        LoadData();
    }

    private void Start()
    {
        InputManager.OnCarrotClicked += CarrotClickedCallback;
        Carrot.OnFrenzyModeStarted += FrenzyModeStartedCallback;
        Carrot.OnFrenzyModeFinished += FrenzyModeFinishedCallback;
    }

    private void OnDestroy()
    {
        InputManager.OnCarrotClicked -= CarrotClickedCallback;
        Carrot.OnFrenzyModeStarted -= FrenzyModeStartedCallback;
        Carrot.OnFrenzyModeFinished -= FrenzyModeFinishedCallback;
    }

    public int GetCurrentMultiplier()
    {
        return carrotIncrementAmount;
    }

    public void AddCarrots(float value)
    {
        totalCarrotCount += value;
        UpdateCarrotCountText();
        SaveData();
    }

    private void CarrotClickedCallback()
    {
        totalCarrotCount += carrotIncrementAmount;
        UpdateCarrotCountText();
        SaveData();
    }

    private void FrenzyModeFinishedCallback()
    {
        carrotIncrementAmount = 1;
    }

    private void FrenzyModeStartedCallback()
    {
        carrotIncrementAmount = frenzyModeMultiplier;
    }

    private void SaveData()
    {
        PlayerPrefs.SetString("Carrots", $"{totalCarrotCount}");
    }

    private void LoadData()
    {
        double.TryParse(PlayerPrefs.GetString("Carrots"), out totalCarrotCount);

        UpdateCarrotCountText();
    }

    private void UpdateCarrotCountText()
    {
        carrotCountText.text = $"{totalCarrotCount:F0} Carrots!";
    }
}