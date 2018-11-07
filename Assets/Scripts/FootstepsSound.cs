﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundEmitter))]
public class FootstepsSound : MonoBehaviour
{
    public float FootstepInterval = 0.2f, StepTreshold = 0.1f;
    public SoundEmitter emitter;
    Rigidbody bodY;
    float t;
    Vector3 oldPos;

    // Use this for initialization
    void Start()
    {
        bodY = GetComponent<Rigidbody>();	
    }

    void OnEnable()
    {
        oldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - oldPos).sqrMagnitude > StepTreshold)
        {
            t += Time.deltaTime;
            if (t > FootstepInterval)
            {
                t = 0;
                if (emitter != null)
                    emitter.EmitSound();
            }
        }
        else
            t = 0;

        oldPos = transform.position;
    }
}
