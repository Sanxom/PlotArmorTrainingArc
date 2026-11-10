using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private void Start()
    {
        InvokeRepeating(nameof(AddCarrots), 1f, 1f);
    }

    private void AddCarrots()
    {
        UpgradeSO[] upgradeArray = ShopManager.Instance.GetUpgradeArray();

        if (upgradeArray.Length <= 1) return;

        double totalCarrots = 0;

        for (int i = 1; i < upgradeArray.Length; i++)
        {
            double upgradeCarrots = upgradeArray[i].cpsPerLevel * ShopManager.Instance.GetUpgradeLevel(i);
            totalCarrots += upgradeCarrots;
        }

        CarrotManager.Instance.AddCarrots(totalCarrots);
    }
}