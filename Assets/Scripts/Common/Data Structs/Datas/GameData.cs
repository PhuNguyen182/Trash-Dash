using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrashDash.Scripts.Common.DataStructs.Datas
{
    [Serializable]
    public class GameData
    {
        public int Coins;
        public int PremiumCoins;

        public float MagnetLevel;
        public float InvincibleLevel;
        public float MultiplyLevel;
        public int Multiplier;

        public List<int> HighScores;
    }
}
