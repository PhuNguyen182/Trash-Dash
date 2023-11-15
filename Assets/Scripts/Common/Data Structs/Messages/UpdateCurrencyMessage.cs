using TrashDash.Scripts.Common.Enumerations;

namespace TrashDash.Scripts.Common.DataStructs.Messages
{
    public struct UpdateCurrencyMessage
    {

    }

    public struct BuyItemMessage
    {
        public string ID;
        public ItemTypeInventory ItemType;
    }
}
