using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoClickManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform rotator;
    [SerializeField] private ObjectPooler bunnyPool;
    private GameObject bunnyPrefab;

    [Header("Data")]
    [SerializeField] private float carrotsPerSeconds;
    [SerializeField] private float cpsMultiplier = 0.1f;
    [SerializeField] private int level;

    [Header("Settings")]
    [SerializeField] private float rotatorSpeed;
    [SerializeField] private float rotatorRadius;
    [SerializeField] private int numBunniesPerRow = 36;
    [SerializeField] private int maxNumOfBunniesAllowed = 108;
    [SerializeField] private int maxNumBunnyRowsAllowed = 3;
    [SerializeField] private float rotatorNextRowDistanceMultiplier = 1.15f;
    [SerializeField] private float bunnyAnimationMoveAmount = -0.5f;
    [SerializeField] private float bunnyAnimationDurationTime = 0.25f;
    private Tween myDelayedCall;
    private int currentBunnyIndex;

    private void Awake()
    {
        LoadData();
        carrotsPerSeconds = level * cpsMultiplier;
    }

    private void Start()
    {
        InvokeRepeating(nameof(AddCarrots), 1f, 1f);

        SpawnBunnies();

        StartAnimatingBunnies();
    }

    private void Update()
    {
        rotator.Rotate(rotatorSpeed * Time.deltaTime * Vector3.forward);
    }

    public void Upgrade()
    {
        level++;
        carrotsPerSeconds = level * cpsMultiplier;

        SaveData();

        if (level <= maxNumOfBunniesAllowed)
        {
            SpawnBunnies();
            StartAnimatingBunnies();
        }
    }

    private void StartAnimatingBunnies()
    {
        if (rotator.childCount <= 0)
            return;

        // Stops the DelayedCall running on this GameObject from ResetBunniesAnimation()
        KillDelay();

        // Stops all the below Tweens before starting them again
        for (int i = 0; i < rotator.childCount; i++)
            rotator.GetChild(i).GetChild(0).DOKill();

        rotator.GetChild(currentBunnyIndex).GetChild(0).DOLocalMoveY(bunnyAnimationMoveAmount, bunnyAnimationDurationTime)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(AnimateNextBunny);
    }

    private void AnimateNextBunny()
    {
        currentBunnyIndex++;

        if (currentBunnyIndex >= rotator.childCount)
            ResetBunniesAnimation();
        else
            StartAnimatingBunnies();
    }

    private void ResetBunniesAnimation()
    {
        currentBunnyIndex = 0;

        float delay = Mathf.Max(10 - rotator.childCount, 0);
        myDelayedCall = DOVirtual.DelayedCall(delay, StartAnimatingBunnies);
    }

    private void KillDelay()
    {
        if (myDelayedCall != null && myDelayedCall.IsActive())
            myDelayedCall.Kill();
    }

    private void SpawnBunnies()
    {
        //int bunnyCount = Mathf.Min(level, maxNumOfBunniesAllowed);

        //for (int i = 0; i < bunnyCount; i++)
        //{
        //    if (i < maxNumOfBunniesAllowed * 0.5f)
        //    {
        //        float angle = i * spawnAngleMultiplier;
        //        Vector2 position = new()
        //        {
        //            x = rotatorRadius * Mathf.Cos(angle * Mathf.Deg2Rad),
        //            y = rotatorRadius * Mathf.Sin(angle * Mathf.Deg2Rad)
        //        };
        //        bunnyPrefab = bunnyPool.GetPooledObject(rotator.transform);
        //        bunnyPrefab.transform.SetPositionAndRotation(position, Quaternion.identity);
        //        bunnyPrefab.transform.up = position.normalized;
        //        bunnyPrefab.SetActive(true);
        //    }
        //    else
        //    {
        //        float angle = (i * spawnAngleMultiplier) + spawnAngleSecondRowOffset;
        //        Vector2 position = new()
        //        {
        //            x = rotatorRadius * rotatorSecondRowDistanceMultiplier * Mathf.Cos(angle * Mathf.Deg2Rad),
        //            y = rotatorRadius * rotatorSecondRowDistanceMultiplier * Mathf.Sin(angle * Mathf.Deg2Rad)
        //        };
        //        bunnyPrefab = bunnyPool.GetPooledObject(rotator.transform);
        //        bunnyPrefab.transform.SetPositionAndRotation(position, Quaternion.identity);
        //        bunnyPrefab.transform.up = position.normalized;
        //        bunnyPrefab.SetActive(true);
        //    }
        //}

        while (rotator.childCount > 0)
        {
            Transform bunny = rotator.GetChild(0);
            rotator.GetChild(0).GetChild(0).SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            bunnyPool.ReturnObjectToPool(bunny.gameObject);
        }

        int maxBunniesForThreeRows = numBunniesPerRow * maxNumBunnyRowsAllowed;
        int bunnyCount = Mathf.Min(level, maxNumOfBunniesAllowed, maxBunniesForThreeRows);

        float angleSpacing = 10f;
        float staggerOffset = 5f;

        for (int i = 0; i < bunnyCount; i++)
        {
            int rowIndex = i / numBunniesPerRow;
            int indexInRow = i % numBunniesPerRow;

            float angle = indexInRow * angleSpacing;

            if (rowIndex % 2 != 0)
            {
                angle += staggerOffset;
            }

            float distanceMultiplier = 1f + (rowIndex * (rotatorNextRowDistanceMultiplier - 1f));
            float finalRadius = rotatorRadius * distanceMultiplier;

            Vector2 position = new()
            {
                x = finalRadius * Mathf.Cos(angle * Mathf.Deg2Rad),
                y = finalRadius * Mathf.Sin(angle * Mathf.Deg2Rad)
            };

            bunnyPrefab = bunnyPool.GetPooledObject(rotator.transform);
            bunnyPrefab.transform.SetPositionAndRotation(position, Quaternion.identity);
            bunnyPrefab.transform.up = position.normalized;
            bunnyPrefab.SetActive(true);
        }
    }

    private void AddCarrots()
    {
        CarrotManager.Instance.AddCarrots(carrotsPerSeconds);
        print($"Adding {carrotsPerSeconds} carrots.");
    }

    private void LoadData()
    {
        level = PlayerPrefs.GetInt("AutoClickLevel");
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("AutoClickLevel", level);
    }
}