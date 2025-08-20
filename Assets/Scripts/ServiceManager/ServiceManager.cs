using EEA.BaseServices.Interfaces;
using EEA.BaseServices.LevelServices;
using EEA.BaseServices.PoolServices;
using EEA.BaseServices.SaveServices;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EEA.BaseServices
{
    public class ServiceManager : MonoBehaviour
    {
        [SerializeField] private ResolveSettings settings;

        public Action OnServicesReady { get; set; }

        private LevelService levelService;
        private SaveService saveService;
        private PoolService poolService;

        private List<ITickable> tickables = new();

        private static ServiceManager instance;

        public ResolveSettings Settings => settings;
        public static ILevelService LevelService => instance.levelService;
        public static ISaveService SaveService => instance.saveService;
        public static IPoolService PoolService => instance.poolService;
        public static ServiceManager Instance => instance;

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
            saveService = BindServiceInterfaces<SaveService>(new SaveService(new EncryptedSaveHandler()));

            levelService = BindServiceInterfaces<LevelService>(new LevelService(settings.levelServiceSettings));
            poolService = BindServiceInterfaces<PoolService>(new PoolService(settings.poolServiceSettings));

            OnServicesReady?.Invoke();

            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }

        private void Update()
        {
            foreach (var t in tickables)
            {
                t.Tick();
            }
        }

        private T BindServiceInterfaces<T>(BaseService baseService) where T : BaseService
        {
            if (baseService is ITickable)
            {
                tickables.Add((ITickable)baseService);
            }

            return (T)baseService;
        }
    }
}