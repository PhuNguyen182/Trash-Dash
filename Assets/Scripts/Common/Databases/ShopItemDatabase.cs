using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrashDash.Scripts.Common.DataStructs.Datas;

namespace TrashDash.Scripts.Common.Databases
{
    [CreateAssetMenu(fileName = "Shop Item Database", menuName = "Scriptable Objects/Databases/Shop Item Database", order = 4)]
    public class ShopItemDatabase : ScriptableObject
    {
        public ConsumableItemData[] Powerups;
        public ConsumableItemData[] Characters;
        public ConsumableItemData[] Accessories;
        public ConsumableItemData[] Themes;
    }
}
