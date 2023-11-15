using System.Collections;
using System.Collections.Generic;
using TrashDash.Scripts.Common.UI.Gameplay;
using TrashDash.Scripts.Common.Enumerations;
using TrashDash.Scripts.Common.DataStructs.Messages;
using TrashDash.Scripts.Common.Gameplay.GameEntities.Character;
using TrashDash.Scripts.Common.Gameplay.GameEntities.Pickups;
using UnityEngine;
using UniRx;
using TrashDash.Scripts.Common.Gameplay.GameEntities.Tracks;

namespace TrashDash.Scripts.Common.GameSystem.Managers
{
    public class PlayerObserver : MonoBehaviour
    {
        [SerializeField] private GameplayPanel gameplayPanel;
        [SerializeField] private CharacterControllerPivot characterController;
        [SerializeField] private TrackManager trackManager;

        private int _coin = 0;
        private int _premium = 0;
        private int _life = 0;
        private int _multiply = 1;

        public int Coin => _coin;
        public int Premium => _premium;
        public int Life => _life;

        public float Distance => trackManager.RunDistance;
        public float Score => trackManager.Score;

        private void Awake()
        {
            MessageBroker.Default.Receive<MultiplyPowerupMessage>()
                                 .Subscribe(value => GetMultiply(value.IsCompleted))
                                 .AddTo(this);
        }

        private void Start()
        {
            GetMultiply(true);
        }

        private void OnEnable()
        {
            characterController.CharacterCollider.OnGetCoin += OnGetCurrency;
            characterController.CharacterCollider.OnGetDamage += GetHP;
        }

        private void OnDisable()
        {
            characterController.CharacterCollider.OnGetCoin -= OnGetCurrency;
            characterController.CharacterCollider.OnGetDamage -= GetHP;
        }

        private void OnGetCurrency(Coin c)
        {
            if (c.IsPremium)
            {
                _premium += 1;
                gameplayPanel.UpdatePremium(Premium);
            }
            else
            {
                _coin += 1;
                gameplayPanel.UpdateCoin(Coin);
            }
        }

        private void GetHP(int hp)
        {
            _life = hp;
            gameplayPanel.UpdateLife(Life);
        }

        private void GetMultiply(bool isDone)
        {
            _multiply = isDone ? 1 : 2;
            trackManager.SetMultiply(_multiply);
            gameplayPanel.UpdateMultiplier(_multiply);
        }

        public void SaveCurrency()
        {
            GameDataManager.AddCoins(Coin, CurrencyUsage.Add);
            GameDataManager.AddPremiumCoin(Premium, CurrencyUsage.Add);
        }

        public void SaveScore()
        {
            GameDataManager.SaveHighScore((int)Score);
        }
    }
}
