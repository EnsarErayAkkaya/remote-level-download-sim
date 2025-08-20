using EEA.BaseServices.LevelServices;
using EEA.BaseServices.PoolServices;
using UnityEngine;

namespace EEA.BaseServices
{
    [CreateAssetMenu(fileName = "ResolveSettings", menuName = "Base Scriptable Objects/Resolve/ResolveSettings", order = 0)]
    public class ResolveSettings : ScriptableObject
    {
        public LevelServiceSettings levelServiceSettings;
        public PoolServiceSettings poolServiceSettings;

        public bool debugLog = true;
    }
}