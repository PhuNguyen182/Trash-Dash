using System;
using System.Collections;
using System.Collections.Generic;
using TrashDash.Scripts.Common.Databases;
using TrashDash.Scripts.Common.Enumerations;
using TrashDash.Scripts.Common.DataStructs.Messages;
using UniRx;
using UnityEngine;
using TrashDash.Scripts.Common.Gameplay.Mainhome;

namespace TrashDash.Scripts.Common.GameSystem.Managers
{
    public class ConsumableManager : Singleton<ConsumableManager>
    {
        [Serializable]
        private class ConsumableUsageData
        {
            
        }

        [SerializeField] private ShopItemDatabase itemDatabase;

        public Theme CurrentTheme { get; private set; }
        public int CharacterDecor { get; private set; }

        protected override void OnAwake()
        {
            MessageBroker.Default.Receive<UseItemMessage>()
                                 .Subscribe(UseItem)
                                 .AddTo(this);
        }

        public void UseItem(UseItemMessage item)
        {
            switch (item.ItemType)
            {
                case ItemTypeInventory.Item:
                    break;
                case ItemTypeInventory.Character:
                    FindCharacter(item);
                    break;
                case ItemTypeInventory.Accessories:
                    FindAccessories(item);
                    break;
                case ItemTypeInventory.Theme:
                    FindTheme(item);
                    break;
            }
        }

        private void FindCharacter(UseItemMessage item)
        {
            for (int i = 0; i < itemDatabase.Characters.Length; i++)
            {
                if(string.CompareOrdinal(item.ID, itemDatabase.Characters[i].ID) == 0)
                {
                    GameObject obj = itemDatabase.Characters[i].Preview;
                    
                    if(obj.TryGetComponent<SkinnedMeshRenderer>(out var smr))
                    {
                        MainMenu.Instance.CharacterPreview.ShowCharacter(smr.sharedMesh, smr.sharedMaterial, obj.transform.localPosition, obj.transform.localRotation);
                    }
                }
            }
        }

        private void FindAccessories(UseItemMessage item)
        {
            if (!item.IsUsed)
            {
                CharacterDecor = -1;
                MainMenu.Instance.CharacterPreview.ShowAccessories(null, null, Vector3.zero, Quaternion.identity);
            }

            else
            {
                for (int i = 0; i < itemDatabase.Accessories.Length; i++)
                {
                    if (string.CompareOrdinal(item.ID, itemDatabase.Accessories[i].ID) == 0)
                    {
                        GameObject obj = itemDatabase.Accessories[i].Preview;
                        CharacterDecor = i;

                        if (obj.TryGetComponent<SkinnedMeshRenderer>(out var smr))
                        {
                            MainMenu.Instance.CharacterPreview.ShowAccessories(smr.sharedMesh, smr.sharedMaterial, obj.transform.localPosition, obj.transform.localRotation);
                        }
                    }
                }
            }
        }

        private void FindTheme(UseItemMessage item)
        {
            for (int i = 0; i < itemDatabase.Themes.Length; i++)
            {
                if (string.CompareOrdinal(item.ID, itemDatabase.Themes[i].ID) == 0)
                    CurrentTheme = (Theme)i;
            }
        }
    }
}
