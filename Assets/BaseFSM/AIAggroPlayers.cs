﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOPRO;
public class AIAggroPlayers : AIVision
{
    [SerializeField]
    private SOListPlayerContainer possibleTargets;

    public override void AIUpdate()
    {
        this.currentTarget = possibleTargets.Elements.Count == 0 ? null : possibleTargets[Random.Range(0, possibleTargets.Elements.Count)].gameObject;
        if (currentTarget)
            OnSpottedTarget.Raise();
    }

    public override void OnStateEnter()
    {
    }

    public override void OnStateExit()
    {
    }
}
