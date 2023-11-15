using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TrashDash.Scripts.Common.DataStructs.Datas;
using TrashDash.Scripts.Common.Databases;
using TrashDash.Scripts.Common.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TrashDash.Scripts.Common.GameSystem.Managers;
using UniRx;
using TrashDash.Scripts.Common.DataStructs.Messages;
using TrashDash.Scripts.Common.Enumerations;

namespace TrashDash.Scripts.Common.UI.Main.Shop
{
    public class ShopPanel : MonoBehaviour, IPanelUI
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private GoodItem shopItem;
        [SerializeField] private Transform itemContainer;
        [SerializeField] private ShopItemDatabase shopItemDatabase;

        [Header("Currency")]
        [SerializeField] private TMP_Text coinText;
        [SerializeField] private TMP_Text premiumText;

        [Header("Tab Button")]
        [SerializeField] private Button itemButton;
        [SerializeField] private Button characterButton;
        [SerializeField] private Button accessoriesButton;
        [SerializeField] private Button themeButton;

        private List<GoodItem> goodItems = new List<GoodItem>();
        private Button[] _shopButtons;

        public async UniTask Close()
        {
            OnCLose();
            await UniTask.CompletedTask;
            gameObject.SetActive(false);
        }

        public async UniTask OnAppear()
        {
            
        }

        public void OnCLose()
        {
            
        }

        private void Awake()
        {
            _shopButtons = new Button[] { itemButton, characterButton, accessoriesButton, themeButton };

            closeButton.onClick.AddListener(() => Close().Forget());

            itemButton.onClick.AddListener(OnItemButton);
            characterButton.onClick.AddListener(OnCharacterButton);
            accessoriesButton.onClick.AddListener(OnAccessoriesButton);
            themeButton.onClick.AddListener(OnThemeButton);

            SimplePool.Preload(shopItem.gameObject, 10, itemContainer);

            MessageBroker.Default.Receive<UpdateCurrencyMessage>()
                                 .Subscribe(_ => ShowCurrency())
                                 .AddTo(this);
        }

        private void OnEnable()
        {
            OnAppear().Forget();
            itemButton.onClick.Invoke();
            ShowCurrency();
        }

        private void OnItemButton()
        {
            SwitchButton(0);
            ShowShopTab(shopItemDatabase.Powerups);
            MessageBroker.Default.Publish(new CheckInventoryMessage { ItemType = ItemTypeInventory.Item });
        }

        private void OnCharacterButton()
        {
            SwitchButton(1);
            ShowShopTab(shopItemDatabase.Characters);
            MessageBroker.Default.Publish(new CheckInventoryMessage { ItemType = ItemTypeInventory.Character });
        }

        private void OnAccessoriesButton()
        {
            SwitchButton(2);
            ShowShopTab(shopItemDatabase.Accessories);
            MessageBroker.Default.Publish(new CheckInventoryMessage { ItemType = ItemTypeInventory.Accessories });
        }

        private void OnThemeButton()
        {
            SwitchButton(3);
            ShowShopTab(shopItemDatabase.Themes);
            MessageBroker.Default.Publish(new CheckInventoryMessage { ItemType = ItemTypeInventory.Theme });
        }

        private void ShowShopTab(ConsumableItemData[] consumables)
        {
            for (int i = 0; i < goodItems.Count; i++)
            {
                SimplePool.Despawn(goodItems[i].gameObject);
            }

            goodItems.Clear();

            for (int i = 0; i < consumables.Length; i++)
            {
                GoodItem item = SimplePool.Spawn(shopItem);
                
                item.transform.position = itemContainer.position;
                item.transform.localScale = Vector3.one;
                item.transform.SetParent(itemContainer, true);

                item.SetItemData(consumables[i]);
                goodItems.Add(item);
            }
        }

        private void ShowCurrency()
        {
            coinText.text = $"{GameDataManager.CurrentData.Coins}";
            premiumText.text = $"{GameDataManager.CurrentData.PremiumCoins}";
        }

        private void SwitchButton(int index)
        {
            for (int i = 0; i < _shopButtons.Length; i++)
            {
                _shopButtons[i].interactable = i != index;
            }
        }
    }
}
