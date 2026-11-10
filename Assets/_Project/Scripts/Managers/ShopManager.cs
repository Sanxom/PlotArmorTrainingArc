using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [Header("Events")]
    public static Action<int> OnUpgradePurchased;

    [SerializeField] private UpgradeButton upgradeButton;
    [SerializeField] private Transform upgradeButtonsParent;

    [Header("Data")]
    [SerializeField] private UpgradeSO[] upgradeArray;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        SpawnButtons();
    }

    public UpgradeSO[] GetUpgradeArray() => upgradeArray;

    public int GetUpgradeLevel(int upgradeIndex) => PlayerPrefs.GetInt($"Upgrade{upgradeIndex}");

    private double GetUpgradePrice(int upgradeIndex) => upgradeArray[upgradeIndex].GetPrice(GetUpgradeLevel(upgradeIndex));

    private void SaveUpgradeLevel(int upgradeIndex, int upgradeLevel)
    {
        PlayerPrefs.SetInt($"Upgrade{upgradeIndex}", upgradeLevel);
    }

    private void SpawnButtons()
    {
        for (int i = 0; i < upgradeArray.Length; i++)
            SpawnButton(i);
    }

    private void SpawnButton(int index)
    {
        UpgradeButton upgradeButtonInstance = Instantiate(upgradeButton, upgradeButtonsParent);
        UpgradeSO upgrade = upgradeArray[index];
        int upgradeLevel = GetUpgradeLevel(index);
        Sprite icon = upgrade.icon;
        string title = upgrade.title;
        string subtitle = string.Format("Lvl {0} (+{1} Cps)", upgradeLevel, upgrade.cpsPerLevel);
        string price = $"{upgrade.GetPrice(upgradeLevel):F0}";
        upgradeButtonInstance.Configure(icon, title, subtitle, price);
        upgradeButtonInstance.GetButton().onClick.AddListener(() => UpgradeButtonClickedCallback(index));
    }

    private void UpgradeButtonClickedCallback(int upgradeIndex)
    {
        if (CarrotManager.Instance.TryPurchase(GetUpgradePrice(upgradeIndex)))
            IncreaseUpgradeLevel(upgradeIndex);
        else
            print("Not enough Carrots!");
    }

    private void IncreaseUpgradeLevel(int upgradeIndex)
    {
        int currentUpgradeLevel = GetUpgradeLevel(upgradeIndex);
        currentUpgradeLevel++;

        SaveUpgradeLevel(upgradeIndex, currentUpgradeLevel);

        UpdateVisuals(upgradeIndex);
        OnUpgradePurchased?.Invoke(upgradeIndex);
    }

    private void UpdateVisuals(int upgradeIndex)
    {
        UpgradeButton upgradeButton = upgradeButtonsParent.GetChild(upgradeIndex).GetComponent<UpgradeButton>();
        UpgradeSO upgrade = upgradeArray[upgradeIndex];
        int upgradeLevel = GetUpgradeLevel(upgradeIndex);
        string subtitle = string.Format("Lvl {0} (+{1} Cps)", upgradeLevel, upgrade.cpsPerLevel);
        string price = $"{upgrade.GetPrice(upgradeLevel):F0}";
        upgradeButton.UpdateVisuals(subtitle, price);
    }
}