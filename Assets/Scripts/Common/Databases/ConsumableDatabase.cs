using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrashDash.Scripts.Common.Gameplay.GameEntities.Pickups;
using TrashDash.Scripts.Common.Enumerations;
using TrashDash.Scripts.Common.GameSystem.Config;

namespace TrashDash.Scripts.Common.Databases
{
    [CreateAssetMenu(fileName = "Consumable Database", menuName = "Scriptable Objects/Databases/Consumable Database", order = 1)]
    public class ConsumableDatabase : ScriptableObject
    {
        [Header("Currency")]
        [SerializeField] private Coin dayCoin;
        [SerializeField] private Coin nightCoin;
        [SerializeField] private Coin dayPremiumCoin;
        [SerializeField] private Coin nightPremiumCoin;

        public Coin Coin
        {
            get
            {
                Coin c;

                switch (PlayerConfig.Current.CurrentTheme)
                {
                    case Theme.Day:
                        c = dayCoin;
                        break;
                    case Theme.Night:
                        c = nightCoin;
                        break;
                    default:
                        c = null;
                        break;
                }

                return c;
            }
        }
        public Coin PremiumCoin
        {
            get
            {
                Coin c;

                switch (PlayerConfig.Current.CurrentTheme)
                {
                    case Theme.Day:
                        c = dayPremiumCoin;
                        break;
                    case Theme.Night:
                        c = nightPremiumCoin;
                        break;
                    default:
                        c = null;
                        break;
                }

                return c;
            }
        }
        
        [Header("Powerups")] 
        public Powerup[] DayPowerups;
        public Powerup[] NightPowerups;

        public Powerup GetRandomPowerupWithHeart(Theme theme = Theme.Day)
        {
            Powerup powerup;
            
            switch (theme)
            {
                case Theme.Day:
                    powerup = DayPowerups.Length > 0 ? DayPowerups[Random.Range(0, DayPowerups.Length)] : null;
                    break;
                case Theme.Night:
                    powerup = NightPowerups.Length > 0 ? NightPowerups[Random.Range(0, NightPowerups.Length)] : null;
                    break;
                default:
                    powerup = null;
                    break;
            }

            return powerup;
        }

        public Powerup GetRandomPowerupWithoutHeart(Theme theme = Theme.Day)
        {
            Powerup powerup;

            switch (theme)
            {
                case Theme.Day:
                    powerup = DayPowerups.Length > 0 ? DayPowerups[Random.Range(1, DayPowerups.Length)] : null;
                    break;
                case Theme.Night:
                    powerup = NightPowerups.Length > 0 ? NightPowerups[Random.Range(1, NightPowerups.Length)] : null;
                    break;
                default:
                    powerup = null;
                    break;
            }

            return powerup;
        }
    }
}
