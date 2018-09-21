﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using Steamworks;

public class NukeBurst : MonoBehaviour
{
    private PostProcessingBehaviour post;
    [SerializeField]
    private bool Burst;
    public BloomModel.Settings UpdatedSetting;
    public AnimationCurve IntesnityCurve;
    public AnimationCurve DeltaTimeCurve;
    public EnemySpawner Spawner;

    private float AnimationTime;
    private bool update, init;

    // Use this for initialization
    void Start()
    {
        post = GetComponent<PostProcessingBehaviour>();
        init = false;
    }

    void ManageNuke(byte[] HostData, uint length, CSteamID IDictionary)
    {
        update = true;
        Burst = true;

        foreach (Enemy item in Spawner.enemyPool.objs.Values)
            item.SetLife(0);
    }

    void Reset()
    {
        UpdatedSetting.bloom.intensity = IntesnityCurve.Evaluate(0);
        UpdatedSetting.bloom.antiFlicker = true;
        post.profile.bloom.settings = UpdatedSetting;
    }

    void OnDestroy()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!init)
        {
            Client.AddCommand(PacketType.Nuke, ManageNuke);
            init = true;
        }

        if (!update)
            return;

        if (Burst)
        {
            if (AnimationTime <= 1)
            {
                AnimationTime += Time.deltaTime;
            }
            if (AnimationTime >= 1)
            {
                Burst = false;
            }
        }
        Time.timeScale = DeltaTimeCurve.Evaluate(AnimationTime);
        if (!Burst)
        {
            if (AnimationTime >= 0)
            {
                AnimationTime -= Time.deltaTime;
            }
            else if (AnimationTime < 0)
            {
                AnimationTime = 0;
                update = false;
            }
        }
        UpdatedSetting.bloom.intensity = IntesnityCurve.Evaluate(AnimationTime);
        UpdatedSetting.bloom.antiFlicker = true;
        post.profile.bloom.settings = UpdatedSetting;
    }
}