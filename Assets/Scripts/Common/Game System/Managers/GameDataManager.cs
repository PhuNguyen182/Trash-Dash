using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEasySaveSystem;
using TrashDash.Scripts.Common.DataStructs.Datas;
using TrashDash.Scripts.Common.Enumerations;
using TrashDash.Scripts.Common.DataStructs.Messages;
using UniRx;

namespace TrashDash.Scripts.Common.GameSystem.Managers
{
    public static class GameDataManager
    {
        private const string GAME_DATA_KEY = "GameData";

        private static GameData _currentData;

        private static GameData _defaultData = new GameData
        {
            Coins = 0,
            PremiumCoins = 0,

            MagnetLevel = 0,
            InvincibleLevel = 0,
            MultiplyLevel = 0,
            Multiplier = 1,
            
            HighScores = new List<int>()
        };

        public static GameData CurrentData
        {
            get
            {
                if (_currentData == null)
                    GetCurrentData();

                return _currentData;
            }
        }

        public static void GetCurrentData()
        {
            _currentData = BasicSaveSystem<GameData>.Load(GAME_DATA_KEY, _defaultData);
        }

        public static void AddCoins(int coins, CurrencyUsage usage)
        {
            switch (usage)
            {
                case CurrencyUsage.Add:
                    CurrentData.Coins += coins;
                    break;
                case CurrencyUsage.Spend:
                    CurrentData.Coins -= coins;

                    if (CurrentData.Coins < 0)
                        CurrentData.Coins = 0;
                    break;
            }

            SaveData();
            MessageBroker.Default.Publish(new UpdateCurrencyMessage { });
        }

        public static void AddPremiumCoin(int premiumCoins, CurrencyUsage usage)
        {
            switch (usage)
            {
                case CurrencyUsage.Add:
                    CurrentData.PremiumCoins += premiumCoins;
                    break;
                case CurrencyUsage.Spend:
                    CurrentData.PremiumCoins -= premiumCoins;

                    if (CurrentData.PremiumCoins < 0)
                        CurrentData.PremiumCoins = 0;
                    break;
            }

            SaveData();
            MessageBroker.Default.Publish(new UpdateCurrencyMessage { });
        }

        public static void SaveMagnetLevel(int level)
        {
            CurrentData.MagnetLevel = level;
            SaveData();
        }

        public static void SaveMultiplyLevel(int level)
        {
            CurrentData.MultiplyLevel = level;
            SaveData();
        }

        public static void AddMultiplier()
        {
            CurrentData.Multiplier += 1;
            SaveData();
        }

        public static void SaveInvincibleLevel(int level)
        {
            CurrentData.InvincibleLevel = level;
            SaveData();
        }

        public static void SaveHighScore(int highScore)
        {
            if (CurrentData.HighScores.Count == 0)
                CurrentData.HighScores.Add(highScore);

            else
            {
                int max = Mathf.Max(_currentData.HighScores.ToArray());

                if (highScore > max)
                {
                    CurrentData.HighScores.Add(highScore);
                    CurrentData.HighScores.Sort();
                    CurrentData.HighScores.Reverse();

                    if (CurrentData.HighScores.Count > 15)
                        CurrentData.HighScores.RemoveAt(15);
                }
            }

            SaveData();
        }

        public static void SaveData()
        {
            BasicSaveSystem<GameData>.Save(GAME_DATA_KEY, CurrentData);
        }

        public static void ClearData()
        {
            BasicSaveSystem<GameData>.Delete(GAME_DATA_KEY);
        }
    }
}
