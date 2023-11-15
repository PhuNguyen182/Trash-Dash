using TrashDash.Scripts.Common.Enumerations;

namespace TrashDash.Scripts.Common.DataStructs.Messages
{
    public struct PurchaseItemMessage
    {
        public string ID;
    }

    public struct CheckItemPurchaseMessage
    {
        public string ID;
        public int BuyCount;
        public bool HasPurchased;
    }

    public struct CheckInventoryMessage
    {
        public ItemTypeInventory ItemType;
    }
}
