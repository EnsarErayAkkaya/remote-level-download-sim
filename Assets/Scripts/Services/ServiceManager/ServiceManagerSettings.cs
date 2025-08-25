using EEA.LevelServices;
using UnityEngine;

namespace EEA
{
    [CreateAssetMenu(fileName = "ServiceManagerSettings", menuName = "Services/ServiceManagerSettings", order = 0)]
    public class ServiceManagerSettings : ScriptableObject
    {
        public bool debugLog = true;
    }
}