using System;
using UnityEngine;

namespace EEA.LoggerService
{
    public static class EEALogger
    {
        public static void Log(string s)
        {
            // save file or send to server in a real app but for now directly log
            if (ServiceManager.Instance.Settings.debugLog)
            {
                try
                {
                    Debug.Log(s);
                }
                catch
                {
                }
            }
        }

        public static void LogError(string s)
        {
            // save file or send to server in a real app but for now directly log

            try
            {
                Debug.LogError(s);
            }
            catch 
            {
            }
        }
    }
}