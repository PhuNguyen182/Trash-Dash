using System.Collections;
using System.Collections.Generic;
using TrashDash.Scripts.Common.GameSystem.Managers;
using TrashDash.Scripts.Common.Inventory;
using UnityEngine;

namespace TrashDash.Scripts.Common.Service
{
    public static class ServiceRegisterOnStart
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnBeforeSceneLoad()
        {
            Register<MusicController>("Music Controller");
            Register<MissionManager>("Mission Manager");
            Register<ShopInventory>("Shop Inventory");
            Register<ConsumableManager>("Consumable Manager");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void OnAfterSceneLoad()
        {

        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void OnBeforeSplashScene()
        {

        }

        private static void Register<T>(string name) where T : Component
        {
            T service = Resources.Load<T>(name);
            T obj =Object.Instantiate(service);
            Object.DontDestroyOnLoad(obj);
        }
    }
}
