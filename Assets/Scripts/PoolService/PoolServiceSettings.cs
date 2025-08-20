using System.Collections.Generic;
using UnityEngine;

namespace EEA.BaseServices.PoolServices
{
    [CreateAssetMenu(fileName = "PoolServiceSettings", menuName = "Base Scriptable Objects/Pooling/Pool Settings", order = 0)]
    public class PoolServiceSettings : ScriptableObject
    {
        public int defaultPoolCapacity = 100;
        public int defaultPoolPreload = 5;
    }
}
