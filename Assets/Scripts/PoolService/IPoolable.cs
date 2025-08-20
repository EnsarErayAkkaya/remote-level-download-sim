﻿using UnityEngine;

namespace EEA.BaseServices.PoolServices
{
    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
        GameObject gameObject { get; }
        Transform transform{ get; }
    }
}