using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TrashDash.Scripts.Common.DataStructs.Messages;
using TrashDash.Scripts.Common.Enumerations;
using TrashDash.Scripts.Common.GameSystem.Managers;
using TrashDash.Scripts.Common.GameSystem.Scenes;
using TrashDash.Scripts.Common.Interfaces;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TrashDash.Scripts.Common.UI.Gameplay
{
    public class ContinuePanel : MonoBehaviour, IProgress<float>, IPanelUI
    {
        [SerializeField] private Button rebornButton;
        [SerializeField] private Button gameOverButton;

        private void Awake()
        {
            rebornButton.onClick.AddListener(UniTask.UnityAction(async () =>
            {
                await Reborn();
            }));

            gameOverButton.onClick.AddListener(UniTask.UnityAction(async () =>
            {
                await GameOver();
            }));
        }

        private void OnEnable()
        {
            OnAppear().Forget();
            rebornButton.interactable = GameDataManager.CurrentData.PremiumCoins >= 10;
        }

        private async UniTask Reborn()
        {
            GameDataManager.AddPremiumCoin(10, CurrencyUsage.Spend);
            await Close();
            MessageBroker.Default.Publish(new RebornMessage { });
        }

        private async UniTask GameOver()
        {
            await SceneLoader.LoadScene(SceneLoader.MAINHOME, this);
        }

        public void Report(float value)
        {
            
        }

        public async UniTask OnAppear()
        {
            
        }

        public async UniTask Close()
        {
            OnCLose();
            await UniTask.CompletedTask;
            gameObject.SetActive(false);
        }

        public void OnCLose()
        {
            
        }
    }
}
