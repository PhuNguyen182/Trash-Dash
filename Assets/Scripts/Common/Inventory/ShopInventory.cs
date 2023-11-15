using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrashDash.Scripts.Common.Databases;
using TrashDash.Scripts.Common.Enumerations;
using TrashDash.Scripts.Common.DataStructs.Messages;
using TrashDash.Scripts.Common.DataStructs.Datas;
using MyEasySaveSystem;
using UniRx;
using TrashDash.Scripts.Common.GameSystem.Managers;

namespace TrashDash.Scripts.Common.Inventory
{
    public class ShopInventory : Singleton<ShopInventory>
    {
        [System.Serializable]
        private class ShopData
        {
            public Dictionary<string, int> PowerupItemData;
            public Dictionary<string, bool> CharacterItemData;
            public Dictionary<string, bool> AccessoriesItemData;
            public Dictionary<string, bool> ThemeItemData;

            public ShopData()
            {
                PowerupItemData = new();
                CharacterItemData = new();
                AccessoriesItemData = new();
                ThemeItemData = new();
            }
        }

        [SerializeField] private ShopItemDatabase shopItemDatabase;

        private ShopData _shopData;

        private Dictionary<string, int> _powerupItems;
        private Dictionary<string, bool> _characterItems;
        private Dictionary<string, bool> _accessoryItems;
        private Dictionary<string, bool> _themeItems;

        public Dictionary<string, int> PowerupItems
        {
            get
            {
                if (_powerupItems == null)
                {
                    _shopData = BasicSaveSystem<ShopData>.Load("ShopData");

                    if (_shopData != null)
                    {
                        if (_shopData.PowerupItemData != null && _shopData.PowerupItemData.Count > 0)
                            _powerupItems = _shopData.PowerupItemData;
                        else
                        {
                            _powerupItems = new Dictionary<string, int>();
                            for (int i = 0; i < shopItemDatabase.Powerups.Length; i++)
                            {
                                _powerupItems.Add(shopItemDatabase.Powerups[i].ID, 1);
                            }
                        }
                    }

                    else
                    {
                        _shopData = new ShopData();
                        _powerupItems = new Dictionary<string, int>();
                        for (int i = 0; i < shopItemDatabase.Powerups.Length; i++)
                        {
                            _powerupItems.Add(shopItemDatabase.Powerups[i].ID, 1);
                        }
                    }
                }

                _shopData.PowerupItemData = _powerupItems;
                BasicSaveSystem<ShopData>.Save("ShopData", _shopData);

                return _powerupItems;
            }
        }
        public Dictionary<string,bool> CharacterItems
        {
            get
            {
                if (_characterItems == null)
                {
                    _shopData = BasicSaveSystem<ShopData>.Load("ShopData");

                    if (_shopData != null)
                    {
                        if (_shopData.CharacterItemData != null && _shopData.CharacterItemData.Count > 0)
                            _characterItems = _shopData.CharacterItemData;
                        else
                        {
                            _characterItems = new Dictionary<string, bool>();
                            for (int i = 0; i < shopItemDatabase.Characters.Length; i++)
                            {
                                _characterItems.Add(shopItemDatabase.Characters[i].ID, i == 0);
                            }
                        }
                    }

                    else
                    {
                        _shopData = new ShopData();
                        _characterItems = new Dictionary<string, bool>();
                        for (int i = 0; i < shopItemDatabase.Characters.Length; i++)
                        {
                            _characterItems.Add(shopItemDatabase.Characters[i].ID, i == 0);
                        }
                    }

                }

                _shopData.CharacterItemData = _characterItems;
                BasicSaveSystem<ShopData>.Save("ShopData", _shopData);

                return _characterItems;
            }
        }

        public Dictionary<string, bool> AccessoryItems
        {
            get
            {
                if(_accessoryItems == null)
                {
                    _shopData = BasicSaveSystem<ShopData>.Load("ShopData");

                    if (_shopData != null)
                    {
                        if (_shopData.AccessoriesItemData != null && _shopData.AccessoriesItemData.Count > 0)
                            _accessoryItems = _shopData.AccessoriesItemData;
                        else
                        {
                            _accessoryItems = new Dictionary<string, bool>();
                            for (int i = 0; i < shopItemDatabase.Accessories.Length; i++)
                            {
                                _accessoryItems.Add(shopItemDatabase.Accessories[i].ID, i == 0);
                            }
                        }
                    }

                    else
                    {
                        _shopData = new ShopData();
                        _accessoryItems = new Dictionary<string, bool>();
                        for (int i = 0; i < shopItemDatabase.Accessories.Length; i++)
                        {
                            _accessoryItems.Add(shopItemDatabase.Accessories[i].ID, i == 0);
                        }
                    }
                }

                _shopData.AccessoriesItemData = _accessoryItems;
                BasicSaveSystem<ShopData>.Save("ShopData", _shopData);

                return _accessoryItems;
            }
        }

        public Dictionary<string, bool> ThemeItems
        {
            get
            {
                if (_themeItems == null)
                {
                    _shopData = BasicSaveSystem<ShopData>.Load("ShopData");

                    if (_shopData != null)
                    {
                        if (_shopData.ThemeItemData != null && _shopData.ThemeItemData.Count > 0)
                            _themeItems = _shopData.ThemeItemData;
                        else
                        {
                            _themeItems = new Dictionary<string, bool>();
                            for (int i = 0; i < shopItemDatabase.Themes.Length; i++)
                            {
                                _themeItems.Add(shopItemDatabase.Themes[i].ID, i == 0);
                            }
                        }
                    }

                    else
                    {
                        _shopData = new ShopData();
                        _themeItems = new Dictionary<string, bool>();
                        for (int i = 0; i < shopItemDatabase.Themes.Length; i++)
                        {
                            _themeItems.Add(shopItemDatabase.Themes[i].ID, i == 0);
                        }
                    }
                }

                _shopData.ThemeItemData = _themeItems;
                BasicSaveSystem<ShopData>.Save("ShopData", _shopData);

                return _themeItems;
            }
        }

        protected override void OnAwake()
        {
            //BasicSaveSystem<ShopData>.Delete("ShopData");

            MessageBroker.Default.Receive<BuyItemMessage>()
                                 .Subscribe(BuyItem)
                                 .AddTo(this);

            MessageBroker.Default.Receive<CheckInventoryMessage>()
                                 .Subscribe(CheckInventory)
                                 .AddTo(this);
        }

        private void Start()
        {
            GameDataManager.SaveMagnetLevel(PowerupItems["Powerup_Magnet"]);
            GameDataManager.SaveInvincibleLevel(PowerupItems["Powerup_Invincible"]);
            GameDataManager.SaveMultiplyLevel(PowerupItems["Powerup_Multiply"]);
        }

        private void BuyItem(BuyItemMessage item)
        {
            switch (item.ItemType)
            {
                case ItemTypeInventory.Item:
                    PowerupItems[item.ID] += 1;
                    _shopData.PowerupItemData = PowerupItems;
                    
                    for (int i = 0; i < shopItemDatabase.Powerups.Length; i++)
                    {
                        ConsumableItemData consumable = shopItemDatabase.Powerups[i];
                        if(string.CompareOrdinal(consumable.ID, item.ID) == 0)
                        {
                            CheckItemPurchase(new CheckItemPurchaseMessage
                            {
                                ID = consumable.ID,
                                BuyCount = PowerupItems[item.ID],
                                HasPurchased = PowerupItems[item.ID] >= consumable.MaxBuyCount
                            });
                        }
                    }

                    break;
                case ItemTypeInventory.Character:
                    CharacterItems[item.ID] = true;
                    _shopData.CharacterItemData = CharacterItems;
                    CheckItemPurchase(new CheckItemPurchaseMessage { ID = item.ID, HasPurchased = true });
                    break;
                case ItemTypeInventory.Accessories:
                    AccessoryItems[item.ID] = true;
                    _shopData.AccessoriesItemData = AccessoryItems;
                    CheckItemPurchase(new CheckItemPurchaseMessage { ID = item.ID, HasPurchased = true });
                    break;
                case ItemTypeInventory.Theme:
                    ThemeItems[item.ID] = true;
                    _shopData.ThemeItemData = ThemeItems;
                    CheckItemPurchase(new CheckItemPurchaseMessage { ID = item.ID, HasPurchased = true });
                    break;
            }

            BasicSaveSystem<ShopData>.Save("ShopData", _shopData);
        }

        private void CheckItemPurchase(CheckItemPurchaseMessage message)
        {
            MessageBroker.Default.Publish(message);
        }

        private void CheckInventory(CheckInventoryMessage message)
        {
            Dictionary<string, int> powerupConsumables = null;
            Dictionary<string, bool> consumableCollection = null;
            ConsumableItemData[] consumableItemDatas = null;
            
            switch (message.ItemType)
            {
                case ItemTypeInventory.Item:
                    powerupConsumables = PowerupItems;
                    consumableItemDatas = shopItemDatabase.Powerups;
                    break;
                case ItemTypeInventory.Character:
                    consumableCollection = CharacterItems;
                    consumableItemDatas = shopItemDatabase.Characters;
                    break;
                case ItemTypeInventory.Accessories:
                    consumableCollection = AccessoryItems;
                    consumableItemDatas = shopItemDatabase.Accessories;
                    break;
                case ItemTypeInventory.Theme:
                    consumableCollection = ThemeItems;
                    consumableItemDatas = shopItemDatabase.Themes;
                    break;
            }

            if (powerupConsumables != null)
            {
                for (int i = 0; i < consumableItemDatas.Length; i++)
                {
                    ConsumableItemData consumable = consumableItemDatas[i];
                    MessageBroker.Default.Publish(new CheckItemPurchaseMessage
                    {
                        ID = consumable.ID,
                        BuyCount = powerupConsumables[consumable.ID],
                        HasPurchased = powerupConsumables[consumable.ID] >= consumable.MaxBuyCount
                    });
                }
            }
            else
            {
                for (int i = 0; i < consumableItemDatas.Length; i++)
                {
                    ConsumableItemData consumable = consumableItemDatas[i];
                    MessageBroker.Default.Publish(new CheckItemPurchaseMessage
                    {
                        ID = consumable.ID,
                        HasPurchased = consumableCollection[consumable.ID]
                    });
                }
            }

            MessageBroker.Default.Publish(new UpdateCurrencyMessage { });
        }
    }
}
