using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusParticleManager : MonoBehaviour
{
    public static BonusParticleManager Instance { get; private set; }

    [Header("Elements")]
    [SerializeField] private ObjectPooler bonusParticlePool;
    private GameObject bonusParticlePrefab;

    public ObjectPooler BonusParticlePool => bonusParticlePool;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        InputManager.OnCarrotClickedPosition += CarrotClickedCallback;
    }

    private void OnDestroy()
    {
        InputManager.OnCarrotClickedPosition -= CarrotClickedCallback;
    }

    private void CarrotClickedCallback(Vector2 clickedPosition)
    {
        bonusParticlePrefab = bonusParticlePool.GetPooledObject(transform);
        bonusParticlePrefab.GetComponent<BonusParticle>().Configure(CarrotManager.Instance.GetCurrentMultiplier());
        bonusParticlePrefab.transform.SetLocalPositionAndRotation(clickedPosition, Quaternion.identity);
        bonusParticlePrefab.SetActive(true);
    }
}