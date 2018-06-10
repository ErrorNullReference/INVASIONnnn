﻿using UnityEngine;
[CreateAssetMenu(menuName = "NetIdDipsenser")]
public class NetIdDispenser : ScriptableObject
{
    [SerializeField]
    protected int nextDispensedId = 0;
    public virtual int GetNewNetId()
    {
        int res = nextDispensedId;
        nextDispensedId = nextDispensedId == int.MaxValue ? 0 : nextDispensedId + 1;
        return res;
    }
}