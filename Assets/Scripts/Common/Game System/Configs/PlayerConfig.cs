using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrashDash.Scripts.Common.Enumerations;
using MyEasySaveSystem;

namespace TrashDash.Scripts.Common.GameSystem.Config
{
    [System.Serializable]
    public class ConfigData
    {
        public int MaxLife;
        public int CurrentTheme;
        public int Decor;
    }

    public class PlayerConfig
    {
        public static PlayerConfig Current;

        public int MaxLife;
        public Theme CurrentTheme;
        public int Decor;


        public static void LoadSelf()
        {
            ConfigData data = BasicSaveSystem<ConfigData>.Load("CurrentPlayerConfig");
            Current = data != null ? new PlayerConfig
            {
                CurrentTheme = (Theme)data.CurrentTheme,
                Decor = data.Decor,
                MaxLife = data.MaxLife
            } : null;
        }

        public static void SaveSelf(PlayerConfig config)
        {
            BasicSaveSystem<ConfigData>.Save("CurrentPlayerConfig", new ConfigData
            {
                CurrentTheme = (int)config.CurrentTheme,
                Decor = config.Decor,
                MaxLife = config.MaxLife
            });
        }

        public static void ClearSelf()
        {
            BasicSaveSystem<ConfigData>.Delete("CurrentPlayerConfig");
        }
    }
}
