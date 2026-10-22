using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusParticle : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshPro bonusText;

    public void Configure(int carrotMultiplier)
    {
        bonusText.text = $"+{carrotMultiplier}";
    }

    public void ReturnToObjectPool()
    {
        BonusParticleManager.Instance.BonusParticlePool.ReturnObjectToPool(gameObject);
    }
}