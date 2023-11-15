using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrashDash.Scripts.Common.Enumerations;

namespace TrashDash.Scripts.Common.DataStructs.Messages
{
    public struct UseItemMessage
    {
        public string ID;
        public bool IsUsed;
        public ItemTypeInventory ItemType;
    }
}
