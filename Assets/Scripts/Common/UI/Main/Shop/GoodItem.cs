using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrashDash.Scripts.Common.GameSystem.Managers;
using TrashDash.Scripts.Common.DataStructs.Datas;
using TrashDash.Scripts.Common.DataStructs.Messages;
using TrashDash.Scripts.Common.Enumerations;
using TMPro;
using UniRx;

namespace TrashDash.Scripts.Common.UI.Main.Shop
{
    public class GoodItem : MonoBehaviour
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private TMP_Text itemName;
        [SerializeField] private TMP_Text coinPrice;
        [SerializeField] private TMP_Text premiumPrice;
        [SerializeField] private TMP_Text quantity;
        [SerializeField] private TMP_Text buyButtonText;
        [SerializeField] private Button buyButton;
        [SerializeField] private Button useButton;

        private string _itemId = "";
        private int _coinPrice = 0;
        private int _premiumPrice = 0;
        private bool _hasPurchased = false;
        private bool _canBuy = false;
        private bool _isUse = false;

        private ConsumableItemData _consumableItemData;

        private void Awake()
        {
            useButton.onClick.AddListener(Use);
            buyButton.onClick.AddListener(Purchase);

            MessageBroker.Default.Receive<UpdateCurrencyMessage>()
                                 .Subscribe(_ => CheckCurrency())
                                 .AddTo(this);

            MessageBroker.Default.Receive<CheckItemPurchaseMessage>()
                                 .Subscribe(value =>
                                 {
                                     if (string.CompareOrdinal(value.ID, _itemId) == 0)
                                     {
                                         quantity.text = $"{value.BuyCount}";
                                         CheckPurchase(value.HasPurchased);
                                     }
                                 })
                                 .AddTo(this);
        }

        public void SetItemData(ConsumableItemData itemData)
        {
            _consumableItemData = itemData;

            _itemId = itemData.ID;
            _coinPrice = itemData.CoinPrice;
            _premiumPrice = itemData.PremiumPrice;
            itemIcon.sprite = itemData.ItemIcon;

            itemName.text = itemData.ItemName;
            coinPrice.text = $"{_coinPrice}";
            premiumPrice.text = $"{_premiumPrice}";

            CheckCurrency();
            useButton.gameObject.SetActive(_consumableItemData.ItemType != ItemTypeInventory.Item 
                                           && _consumableItemData.ItemType != ItemTypeInventory.Theme);
        }

        private void CheckCurrency()
        {
            int coins = GameDataManager.CurrentData.Coins;
            int premiums = GameDataManager.CurrentData.PremiumCoins;

            coinPrice.color = coins >= _coinPrice ? new Color(0.2f, 0.2f, 0.2f, 1) 
                                                  : new Color(1, 0.13f, 0.13f, 1);

            premiumPrice.color = premiums >= _premiumPrice ? new Color(0.2f, 0.2f, 0.2f, 1) 
                                                           : new Color(1, 0.13f, 0.13f, 1);

            _canBuy = coins >= _coinPrice && premiums >= _premiumPrice;
            buyButton.interactable = _canBuy && !_hasPurchased;
        }

        private void Purchase()
        {
            GameDataManager.AddCoins(_coinPrice, CurrencyUsage.Spend);
            GameDataManager.AddPremiumCoin(_premiumPrice, CurrencyUsage.Spend);

            MessageBroker.Default.Publish(new BuyItemMessage { ID = _itemId, ItemType = _consumableItemData.ItemType });
        }

        private void Use()
        {
            _isUse = !_isUse;

            MessageBroker.Default.Publish(new UseItemMessage
            {
                ID = _itemId,
                IsUsed = true,
                ItemType = _consumableItemData.ItemType
            });
        }

        private void CheckPurchase(bool hasPurchased)
        {
            _hasPurchased = hasPurchased;
            buyButton.interactable = _canBuy && !_hasPurchased;
            useButton.interactable = hasPurchased;
            buyButtonText.text = hasPurchased ? "Owned" : "Buy";
        }
    }
}
