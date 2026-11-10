using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI subtitleText;
    [SerializeField] private TextMeshProUGUI priceText;

    public void Configure(Sprite icon, string title, string subtitle, string price)
    {
        this.icon.sprite = icon;
        titleText.text = title;
        UpdateVisuals(subtitle, price);
    }

    public void UpdateVisuals(string subtitle, string price)
    {
        subtitleText.text = subtitle;
        priceText.text = price;
    }

    public Button GetButton() => button;
}