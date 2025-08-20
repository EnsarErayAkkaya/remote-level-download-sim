using EEA.BaseServices.LevelServices;
using UnityEngine;

namespace EEA.BaseServices
{
    [CreateAssetMenu(fileName = "ServiceManagerSettings", menuName = "Services/ServiceManagerSettings", order = 0)]
    public class ServiceManagerSettings : ScriptableObject
    {
        public LevelServiceSettings levelServiceSettings;

        public bool debugLog = true;
    }
}