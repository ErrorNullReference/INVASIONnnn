﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SOPRO;
public class Enemy : LivingBeing
{
    public bool Destroy;
    public bool Recycling;
    public float randomSpawnTimer;
    [HideInInspector]
    public SOPool Pool;
    private float HUDTimer;
    [SerializeField]
    private float HUDTimerShow;
    private GameNetworkObject networkId;
    HUDManager hudManager;
    Image healthImage;
    public ulong Killer;
    public GameObject EnergyPrefab;

    public GameNetworkObject NetworkId
    {
        get
        {
            return networkId;
        }
    }

    private void Start()
    {
        hudManager = GetComponentInChildren<HUDManager>();
        healthImage = hudManager.GetComponent<Image>();
        healthImage.enabled = false;
        hudManager.InputAssetHUD = Stats;
    }

    private void OnEnable()
    {
        if (!networkId)
            networkId = GetComponent<GameNetworkObject>();

        randomSpawnTimer = Random.Range(0f, 5.0f);
        Life = Stats.MaxHealth;
        Destroy = false;
        Recycling = false;
    }
    private void OnDisable()
    {
        networkId.ResetNetworkId();
    }
    private void Awake()
    {
        randomSpawnTimer = Random.Range(0f, 5.0f);
        Destroy = false;
        Recycling = false;
    }

    public void Reset()
    {
        Life = Stats.MaxHealth;
    }

    public void DestroyAndRecycle()
    {
        Destroy = true;
        Recycling = true;
    }

    public void DecreaseLife()
    {
        healthImage.enabled = true;
        HUDTimer = HUDTimerShow;
        Life--;
    }

    private void Update()
    {
        if (HUDTimer > 0)
        {
            HUDTimer -= Time.deltaTime;
            if (HUDTimer <= 0)
            {
                healthImage.enabled = false;
            }
        }
    }

    public void DropEnergy()
    {
        int randomAmount = UnityEngine.Random.Range(1, 6);
        for (int i = 0; i < randomAmount; i++)
        {
            Vector3 pos = new Vector3(transform.position.x + UnityEngine.Random.Range(0f, 1.0f), transform.position.y+0.5f, transform.position.z + UnityEngine.Random.Range(0f, 1.0f));
            Instantiate(EnergyPrefab, pos, Quaternion.identity);
        }
    }

}
