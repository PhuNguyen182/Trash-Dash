using System.Collections;
using System.Collections.Generic;
using TrashDash.Scripts.Common.DataStructs.Datas;
using TrashDash.Scripts.Common.Enumerations;
using TrashDash.Scripts.Common.GameSystem.Config;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TrashDash.Scripts.Common.Databases
{
    [CreateAssetMenu(fileName = "Theme Database", menuName = "Scriptable Objects/Databases/Theme Database")]
    public class ThemeDatabase : ScriptableObject
    {
        [Header("Sky Themes")]
        [SerializeField] private SkyData daySky;
        [SerializeField] private SkyData nightSky;

        [Header("Bundle Themes")]
        [SerializeField] private AssetReference[] dayTheme;
        [SerializeField] private AssetReference[] nightTheme;

        public SkyData GetThemeSky(Theme theme)
        {
            SkyData sky;

            switch (theme)
            {
                case Theme.Day:
                    sky = daySky;
                    break;
                case Theme.Night:
                    sky = nightSky;
                    break;
                default:
                    sky = default;
                    break;
            }

            return sky;
        }

        public AssetReference GetRandomBundle(int bundleRange = 0)
        {
            AssetReference bundle;
            Bundle bundleType = (Bundle)bundleRange;
            int min = 0, max = 0;

            switch (bundleType)
            {
                case Bundle.Industries:
                    min = 0; max = 9;
                    break;
                case Bundle.FreeRoad:
                    min = 9; max = 12;
                    break;
                case Bundle.Urban:
                    min = 12; max = 19;
                    break;
                case Bundle.Houses:
                    min = 19; max = 29;
                    break;
            }

            switch (PlayerConfig.Current.CurrentTheme)
            {
                case Theme.Day:
                    bundle = GetRandomDayBundle(min, max);
                    break;
                case Theme.Night:
                    bundle = GetRandomNightBundle(min, max);
                    break;
                default:
                    bundle = null;
                    break;
            }

            return bundle;
        }

        private AssetReference GetRandomDayBundle(int min, int max)
        {
            if (dayTheme.Length > 0)
            {
                int rand = Random.Range(min, max);
                return dayTheme[rand];
            }

            return null;
        }

        private AssetReference GetRandomNightBundle(int min, int max)
        {
            if (nightTheme.Length > 0)
            {
                int rand = Random.Range(min, max);
                return nightTheme[rand];
            }

            return null;
        }
    }
}
