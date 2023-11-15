using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrashDash.Scripts.Common.Enumerations;

namespace TrashDash.Scripts.Common.DataStructs.Datas
{
    [Serializable]
    public struct ConsumableItemData
    {
        public int MaxBuyCount;
        public ItemTypeInventory ItemType;
        public string ID;
        public string ItemName;
        public int CoinPrice;
        public int PremiumPrice;
        public Sprite ItemIcon;
        public GameObject Preview;
    }
}
