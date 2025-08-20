using EEA.BaseServices.LevelServices;
using EEA.BaseServices.SaveServices;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EEA.BaseServices
{
    public class ServiceManager : MonoBehaviour
    {
        [SerializeField] private ServiceManagerSettings settings;

        public Action OnServicesReady { get; set; }

        private LevelService levelService;
        private SaveService saveService;

        private static ServiceManager instance;

        #region GETTERS
        public ServiceManagerSettings Settings => settings;
        public static ILevelService LevelService => instance.levelService;
        public static ISaveService SaveService => instance.saveService;
        public static ServiceManager Instance => instance;
        #endregion

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(instance);
            }

            instance = this;
        }
        
        private void Start()
        {
            saveService = new SaveService(new EncryptedSaveHandler());

            levelService = new LevelService(settings.levelServiceSettings);

            OnServicesReady?.Invoke();

            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }
    }
}